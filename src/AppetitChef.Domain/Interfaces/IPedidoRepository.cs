using AppetitChef.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido?> GetComItensAsync(int pedidoId, CancellationToken ct = default);
        Task<IEnumerable<Pedido>> GetAbertosAsync(int filialId, CancellationToken ct = default);
    }
}
