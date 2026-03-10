using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppetitChef.Infrastructure.Persistence.Repositories;

public class FuncionarioRepository(AppetitChefDbContext ctx) : BaseRepository<Funcionario>(ctx), IFuncionarioRepository
{
    // Corrigido para bater com a Interface (GetByLoginAsync)
    public async Task<Funcionario?> GetByLoginAsync(string login, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(f => f.Login == login, ct);

    // Corrigido para bater com a Interface (GetByCpfAsync)
    public async Task<Funcionario?> GetByCpfAsync(string cpf, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(f => f.Cpf == cpf, ct);

    // Método extra se sua interface possuir:
    public async Task<bool> LoginExisteAsync(string login, CancellationToken ct = default) =>
        await DbSet.AnyAsync(f => f.Login == login, ct);
}