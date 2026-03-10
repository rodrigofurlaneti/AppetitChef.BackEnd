using AppetitChef.Domain.Entities;

namespace AppetitChef.Domain.Interfaces;

public interface IInsumoRepository : IRepository<Insumo>
{
    Task<IEnumerable<Insumo>> GetCriticosAsync(CancellationToken ct = default);
}
