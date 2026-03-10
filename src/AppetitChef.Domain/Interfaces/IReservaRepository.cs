using AppetitChef.Domain.Entities;
namespace AppetitChef.Domain.Interfaces
{

    public interface IReservaRepository : IRepository<Reserva>
    {
        Task<IEnumerable<Reserva>> GetByDataAsync(DateTime data, int filialId, CancellationToken ct = default);
    }
}
