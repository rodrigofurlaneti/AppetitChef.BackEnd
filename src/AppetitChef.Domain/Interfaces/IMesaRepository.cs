using AppetitChef.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Interfaces
{
    public interface IMesaRepository : IRepository<Mesa>
    {
        Task<IEnumerable<Mesa>> GetByFilialAsync(int filialId, CancellationToken ct = default);
        Task<Mesa?> GetByAreaENumeroAsync(int areaId, int numero, CancellationToken ct = default);
    }
}
