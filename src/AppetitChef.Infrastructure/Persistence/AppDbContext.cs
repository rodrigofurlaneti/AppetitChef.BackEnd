using AppetitChef.Domain.Common;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppetitChef.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<Filial> Filiais => Set<Filial>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Mesa> Mesas => Set<Mesa>();
    public DbSet<Cargo> Cargos => Set<Cargo>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<FidelidadeTransacao> FidelidadeTransacoes => Set<FidelidadeTransacao>();
    public DbSet<CategoriaProduto> CategoriasProduto => Set<CategoriaProduto>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<Insumo> Insumos => Set<Insumo>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(mb);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await base.SaveChangesAsync(ct);
        await DispatchDomainEventsAsync(ct);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var evt in events)
            await mediator.Publish(evt, ct);
    }
}
