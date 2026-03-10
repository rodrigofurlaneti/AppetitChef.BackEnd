using AppetitChef.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Interfaces
{
    public interface IInsumoRepository : IRepository<Insumo>
    {
        Task<IEnumerable<Insumo>> GetCriticosAsync(CancellationToken ct = default);
    }
}
