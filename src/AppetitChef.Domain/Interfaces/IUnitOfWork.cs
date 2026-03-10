using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPedidoRepository Pedidos { get; }
        IMesaRepository Mesas { get; }
        IClienteRepository Clientes { get; }
        IProdutoRepository Produtos { get; }
        IReservaRepository Reservas { get; }
        IInsumoRepository Insumos { get; }
        IFuncionarioRepository Funcionarios { get; }
        Task<int> CommitAsync(CancellationToken ct = default);
    }
}
