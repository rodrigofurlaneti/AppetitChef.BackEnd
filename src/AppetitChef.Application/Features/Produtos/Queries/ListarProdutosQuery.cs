using AppetitChef.Application.Features.Produtos.Dtos;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Produtos.Queries
{
    public record ListarProdutosQuery(bool? SomenteDisponiveis) : IRequest<IEnumerable<ProdutoDto>>;

    public class ListarProdutosHandler(IUnitOfWork uow) : IRequestHandler<ListarProdutosQuery, IEnumerable<ProdutoDto>>
    {
        public async Task<IEnumerable<ProdutoDto>> Handle(ListarProdutosQuery request, CancellationToken ct)
        {
            var produtos = await uow.Produtos.GetAllAsync(ct);

            return produtos.Select(p => new ProdutoDto(
                p.Id, p.Codigo, p.Nome, p.Descricao,
                p.PrecoVenda, p.CustoEstimado, p.TempoPreparo,
                true, 
                p.VendidoNoBar, p.VendidoNoSalao, p.VendidoDelivery,
                p.Categoria?.Nome ?? "Geral", p.ImagemUrl));
        }
    }
}
