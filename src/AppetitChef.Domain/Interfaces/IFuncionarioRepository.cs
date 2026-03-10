using AppetitChef.Domain.Entities;

namespace AppetitChef.Domain.Interfaces;

public interface IFuncionarioRepository : IRepository<Funcionario>
{
    Task<Funcionario?> GetByLoginAsync(string login, CancellationToken ct = default);
    Task<Funcionario?> GetByCpfAsync(string cpf, CancellationToken ct = default);
}
