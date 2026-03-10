using AppetitChef.Application.Common.Interfaces;
using AppetitChef.Domain.Interfaces;
using AppetitChef.Infrastructure.Persistence;
using AppetitChef.Infrastructure.Persistence.Repositories;
using AppetitChef.Infrastructure.Security;
using AppetitChef.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppetitChef.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connStr = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' nao encontrada.");

        services.AddDbContext<AppetitChefDbContext>(opts =>
            opts.UseMySql(connStr, ServerVersion.AutoDetect(connStr),
                mysql => mysql.EnableRetryOnFailure(3)));

        // UoW e Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IMesaRepository, MesaRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
        services.AddScoped<IInsumoRepository, InsumoRepository>();
        services.AddScoped<IReservaRepository, ReservaRepository>();

        // Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}