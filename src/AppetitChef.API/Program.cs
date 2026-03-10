using AppetitChef.API.Extensions;
using AppetitChef.API.Middlewares;
using AppetitChef.Application;
using AppetitChef.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/appetitchef-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddSwaggerConfiguration();

    builder.Services.AddCors(opts =>
        opts.AddPolicy("AppetitChefPolicy", p =>
            p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

    builder.Services.AddHealthChecks();

    var app = builder.Build();

    app.UseGlobalExceptionHandling();
    app.UseSerilogRequestLogging();

    // Swagger disponivel em todos os ambientes para facilitar testes
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppetitChef API v1");
        c.RoutePrefix = "swagger";
    });

    app.UseHttpsRedirection();
    app.UseCors("AppetitChefPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks("/health");

    Log.Information("AppetitChef API iniciando...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host encerrou inesperadamente.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
