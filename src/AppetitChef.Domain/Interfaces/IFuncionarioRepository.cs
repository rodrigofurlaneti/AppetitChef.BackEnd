using AppetitChef.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Interfaces
{
    public interface IFuncionarioRepository : IRepository<Funcionario>
    {
        Task<Funcionario?> GetByLoginAsync(string login, CancellationToken ct = default);
        Task<Funcionario?> GetByCpfAsync(string cpf, CancellationToken ct = default);
    }
}
