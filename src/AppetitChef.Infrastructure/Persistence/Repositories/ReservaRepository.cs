using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Enums;
using AppetitChef.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppetitChef.Infrastructure.Persistence.Repositories;

public class ReservaRepository(AppetitChefDbContext ctx) : BaseRepository<Reserva>(ctx), IReservaRepository
{
    public async Task<IEnumerable<Reserva>> GetByDataAsync(DateTime data, int filialId, CancellationToken ct = default) =>
        await DbSet.Include(r => r.Cliente).Include(r => r.Mesa)
            .Where(r => r.FilialId == filialId && r.DataReserva.Date == data.Date)
            .OrderBy(r => r.DataReserva).ToListAsync(ct);
}
