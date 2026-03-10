using AppetitChef.Domain.Entities;

namespace AppetitChef.Domain.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente?> GetByCpfAsync(string cpf, CancellationToken ct = default);
    Task<Cliente?> GetByEmailAsync(string email, CancellationToken ct = default);
}
