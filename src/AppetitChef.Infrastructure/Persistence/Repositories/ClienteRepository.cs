using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppetitChef.Infrastructure.Persistence.Repositories;

public class ClienteRepository(AppetitChefDbContext ctx) : BaseRepository<Cliente>(ctx), IClienteRepository
{
    public async Task<Cliente?> GetByCpfAsync(string cpf, CancellationToken ct) =>
        await DbSet.FirstOrDefaultAsync(c => c.Cpf == cpf, ct);

    public async Task<Cliente?> GetByEmailAsync(string email, CancellationToken ct) =>
        await DbSet.FirstOrDefaultAsync(c => c.Email == email, ct);
}