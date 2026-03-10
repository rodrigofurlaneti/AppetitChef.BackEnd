using AppetitChef.Domain.Entities;

namespace AppetitChef.Domain.Interfaces;

public interface IMesaRepository : IRepository<Mesa>
{
    Task<IEnumerable<Mesa>> GetByFilialAsync(int filialId, CancellationToken ct = default);
    Task<Mesa?> GetByAreaENumeroAsync(int areaId, int numero, CancellationToken ct = default);
}
