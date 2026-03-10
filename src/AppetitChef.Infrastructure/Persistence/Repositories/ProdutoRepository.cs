using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppetitChef.Infrastructure.Persistence.Repositories;

public class ProdutoRepository(AppetitChefDbContext ctx) : BaseRepository<Produto>(ctx), IProdutoRepository
{
    public async Task<Produto?> GetByCodigoAsync(string codigo, CancellationToken ct = default) =>
        await DbSet.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Codigo == codigo, ct);

    public async Task<IEnumerable<Produto>> GetDisponiveisAsync(CancellationToken ct = default) =>
        await DbSet.Include(p => p.Categoria)
                   .Where(p => p.Disponivel)
                   .OrderBy(p => p.Nome)
                   .ToListAsync(ct);

    public async Task<IEnumerable<Produto>> GetByCategoriaAsync(int categoriaId, CancellationToken ct = default) =>
        await DbSet.Include(p => p.Categoria)
                   .Where(p => p.CategoriaId == categoriaId)
                   .ToListAsync(ct);

    public override async Task<IEnumerable<Produto>> GetAllAsync(CancellationToken ct = default) =>
        await DbSet.Include(p => p.Categoria).OrderBy(p => p.Nome).ToListAsync(ct);
}
