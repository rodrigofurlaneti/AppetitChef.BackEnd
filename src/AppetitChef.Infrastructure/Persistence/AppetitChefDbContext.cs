using AppetitChef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AppetitChef.Domain.Common;
namespace AppetitChef.Infrastructure.Persistence;

public class AppetitChefDbContext(DbContextOptions<AppetitChefDbContext> options) : DbContext(options)
{
    public DbSet<Filial> Filiais => Set<Filial>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Mesa> Mesas => Set<Mesa>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<CategoriaProduto> CategoriasProduto => Set<CategoriaProduto>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Insumo> Insumos => Set<Insumo>();
    public DbSet<MovimentoEstoque> MovimentosEstoque => Set<MovimentoEstoque>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();
    public DbSet<FormaPagamento> FormasPagamento => Set<FormaPagamento>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfigurationsFromAssembly(typeof(AppetitChefDbContext).Assembly);
        base.OnModelCreating(mb);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.GetType().GetProperty("UpdatedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
        }
        return base.SaveChangesAsync(ct);
    }
}