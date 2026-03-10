using System.Net;
using System.Text.Json;
using AppetitChef.Domain.Exceptions;
using FluentValidation;

namespace AppetitChef.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro nao tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (status, title, erros) = ex switch
        {
            ValidationException ve => (
                HttpStatusCode.BadRequest,
                "Erro de validacao",
                ve.Errors.Select(e => e.ErrorMessage).ToList()
            ),
            NotFoundException nfe => (
                HttpStatusCode.NotFound,
                "Recurso nao encontrado",
                new List<string> { nfe.Message }
            ),
            BusinessRuleException bre => (
                HttpStatusCode.UnprocessableEntity,
                "Regra de negocio violada",
                new List<string> { bre.Message }
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Nao autorizado",
                new List<string> { "Acesso negado." }
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "Erro interno do servidor",
                new List<string> { "Ocorreu um erro inesperado." }
            )
        };

        context.Response.StatusCode = (int)status;
        var response = new
        {
            status = (int)status,
            title,
            erros,
            timestamp = DateTime.UtcNow
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();
}