using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppetitChef.Infrastructure.Persistence.Repositories;

public class ProdutoRepository(AppetitChefDbContext ctx) : BaseRepository<Produto>(ctx), IProdutoRepository
{
    // Corresponde ao IProdutoRepository.GetByCodigoAsync
    public async Task<Produto?> GetByCodigoAsync(string codigo, CancellationToken ct = default) =>
        await DbSet.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Codigo == codigo, ct);

    // Corresponde ao IProdutoRepository.GetDisponiveisAsync
    public async Task<IEnumerable<Produto>> GetDisponiveisAsync(CancellationToken ct = default) =>
        await DbSet.Include(p => p.Categoria)
                   .Where(p => p.Disponivel)
                   .OrderBy(p => p.Nome)
                   .ToListAsync(ct);

    // Corresponde ao IProdutoRepository.GetByCategoriaAsync
    public async Task<IEnumerable<Produto>> GetByCategoriaAsync(int categoriaId, CancellationToken ct = default) =>
        await DbSet.Include(p => p.Categoria)
                   .Where(p => p.CategoriaId == categoriaId)
                   .ToListAsync(ct);

    // Sobrescreve o GetAllAsync do BaseRepository para incluir a Categoria (Eager Loading)
    public override async Task<IEnumerable<Produto>> GetAllAsync(CancellationToken ct = default) =>
        await DbSet.Include(p => p.Categoria).OrderBy(p => p.Nome).ToListAsync(ct);
}