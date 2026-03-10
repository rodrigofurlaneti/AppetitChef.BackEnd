using AppetitChef.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetDisponiveisAsync(CancellationToken ct = default);
        Task<IEnumerable<Produto>> GetByCategoriaAsync(int categoriaId, CancellationToken ct = default);
    }
}
