using AppetitChef.Domain.Entities;

namespace AppetitChef.Domain.Interfaces;

public interface IPedidoRepository : IRepository<Pedido>
{
    Task<Pedido?> GetComItensAsync(int pedidoId, CancellationToken ct = default);
    Task<IEnumerable<Pedido>> GetAbertosAsync(int filialId, CancellationToken ct = default);
}
