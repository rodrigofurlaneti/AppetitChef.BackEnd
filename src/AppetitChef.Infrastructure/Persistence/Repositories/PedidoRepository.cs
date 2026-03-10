using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppetitChef.Infrastructure.Persistence.Repositories;

public class PedidoRepository(AppetitChefDbContext ctx) : BaseRepository<Pedido>(ctx), IPedidoRepository
{
    public async Task<Pedido?> GetComItensAsync(int id, CancellationToken ct = default) =>
        await DbSet
            .Include(p => p.Itens).ThenInclude(i => i.Produto)
            .Include(p => p.Cliente)
            .Include(p => p.Mesa)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IEnumerable<Pedido>> GetAbertosAsync(int filialId, CancellationToken ct = default) =>
        await DbSet
            .Include(p => p.Cliente)
            .Include(p => p.Mesa)
            .Include(p => p.Itens)
            .Where(p => p.FilialId == filialId && p.Status == Domain.Enums.StatusPedido.Aberto)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);
}
