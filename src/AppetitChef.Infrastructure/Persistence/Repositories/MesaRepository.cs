using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Enums;
using AppetitChef.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppetitChef.Infrastructure.Persistence.Repositories;

public class MesaRepository(AppetitChefDbContext ctx) : BaseRepository<Mesa>(ctx), IMesaRepository
{
    public async Task<IEnumerable<Mesa>> GetByFilialAsync(int filialId, CancellationToken ct) =>
        await DbSet.Include(m => m.Area).Where(m => m.Area.FilialId == filialId).ToListAsync(ct);

    public async Task<Mesa?> GetByAreaENumeroAsync(int areaId, int numero, CancellationToken ct) =>
        await DbSet.FirstOrDefaultAsync(m => m.AreaId == areaId && m.Numero == numero, ct);
}