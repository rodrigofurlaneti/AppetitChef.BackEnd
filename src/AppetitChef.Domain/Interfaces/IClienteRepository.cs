using AppetitChef.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente?> GetByCpfAsync(string cpf, CancellationToken ct = default);
        Task<Cliente?> GetByEmailAsync(string email, CancellationToken ct = default);
    }
}
