using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Produtos.Commands;

    public record CriarProdutoCommand(int CategoriaId, string Codigo, string Nome,
        decimal PrecoVenda, decimal CustoEstimado, int TempoPreparo,
        string? Descricao) : IRequest<Result<int>>;

    public class CriarProdutoHandler(IUnitOfWork uow) : IRequestHandler<CriarProdutoCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CriarProdutoCommand req, CancellationToken ct)
        {
            var produto = Produto.Criar(req.CategoriaId, req.Codigo, req.Nome,
                req.PrecoVenda, req.CustoEstimado, req.TempoPreparo, req.Descricao);
            await uow.Produtos.AddAsync(produto, ct);
            await uow.CommitAsync(ct);
            return Result<int>.Success(produto.Id);
        }
    }

