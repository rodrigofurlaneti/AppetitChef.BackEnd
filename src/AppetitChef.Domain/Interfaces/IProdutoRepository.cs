using AppetitChef.Domain.Entities;

namespace AppetitChef.Domain.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<Produto?> GetByCodigoAsync(string codigo, CancellationToken ct = default);
    Task<IEnumerable<Produto>> GetDisponiveisAsync(CancellationToken ct = default);
    Task<IEnumerable<Produto>> GetByCategoriaAsync(int categoriaId, CancellationToken ct = default);
}
