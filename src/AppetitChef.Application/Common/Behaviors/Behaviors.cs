using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using AppetitChef.Application.Common.Exceptions;
using ValidationException = AppetitChef.Application.Common.Exceptions.ValidationException;

namespace AppetitChef.Application.Common.Behaviors;

// ── Validation Behavior ──────────────────────────────────────────────────────
public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, ct)));
        var failures = results.SelectMany(r => r.Errors).Where(e => e is not null).ToList();

        if (failures.Count != 0) throw new ValidationException(failures);

        return await next();
    }
}

// ── Logging Behavior ─────────────────────────────────────────────────────────
public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("[CQRS] Handling {RequestName}: {@Request}", requestName, request);

        try
        {
            var response = await next();
            logger.LogInformation("[CQRS] Handled {RequestName} successfully.", requestName);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[CQRS] Error handling {RequestName}", requestName);
            throw;
        }
    }
}

// ── Performance Behavior ─────────────────────────────────────────────────────
public sealed class PerformanceBehavior<TRequest, TResponse>(
    ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int SlowRequestThresholdMs = 500;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        if (sw.ElapsedMilliseconds > SlowRequestThresholdMs)
            logger.LogWarning("[PERFORMANCE] Slow request: {RequestName} took {ElapsedMs}ms. {@Request}",
                typeof(TRequest).Name, sw.ElapsedMilliseconds, request);

        return response;
    }
}
