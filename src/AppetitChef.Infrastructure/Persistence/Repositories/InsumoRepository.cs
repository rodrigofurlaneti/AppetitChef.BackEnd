using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppetitChef.Infrastructure.Persistence.Repositories;

public class InsumoRepository(AppetitChefDbContext ctx) : BaseRepository<Insumo>(ctx), IInsumoRepository
{
    // Corrigido para bater com a Interface (GetCriticosAsync)
    public async Task<IEnumerable<Insumo>> GetCriticosAsync(CancellationToken ct = default) =>
        await DbSet.Where(i => i.EstoqueAtual <= i.EstoqueMinimo).ToListAsync(ct);
}