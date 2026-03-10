using System.Net;
using System.Text.Json;
using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Domain.Exceptions;

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

        HttpStatusCode status;
        string title;
        List<string> erros;

        switch (ex)
        {
            case ValidationException ve:
                status = HttpStatusCode.BadRequest;
                title = "Erro de validacao";
                erros = ve.Errors.SelectMany(e => e.Value).ToList();
                break;
            case NotFoundException nfe:
                status = HttpStatusCode.NotFound;
                title = "Recurso nao encontrado";
                erros = new List<string> { nfe.Message };
                break;
            case BusinessRuleException bre:
                status = HttpStatusCode.UnprocessableEntity;
                title = "Regra de negocio violada";
                erros = new List<string> { bre.Message };
                break;
            case UnauthorizedException uae:
                status = HttpStatusCode.Unauthorized;
                title = "Nao autorizado";
                erros = new List<string> { uae.Message };
                break;
            case UnauthorizedAccessException:
                status = HttpStatusCode.Unauthorized;
                title = "Nao autorizado";
                erros = new List<string> { "Acesso negado." };
                break;
            case InvalidOperationException ioe:
                status = HttpStatusCode.BadRequest;
                title = "Operacao invalida";
                erros = new List<string> { ioe.Message };
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                title = "Erro interno do servidor";
                erros = new List<string> { "Ocorreu um erro inesperado." };
                break;
        }

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
