using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Application.Features.Produtos.Commands;

    public record AtualizarPrecoProdutoCommand(int ProdutoId, decimal NovoPreco) : IRequest<Result<bool>>;

    public class AtualizarPrecoProdutoHandler(IUnitOfWork uow) : IRequestHandler<AtualizarPrecoProdutoCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AtualizarPrecoProdutoCommand req, CancellationToken ct)
        {
            var produto = await uow.Produtos.GetByIdAsync(req.ProdutoId, ct);
            if (produto == null) return Result<bool>.Failure("Produto não encontrado.");

            produto.AtualizarPreco(req.NovoPreco);
            uow.Produtos.Update(produto);
            await uow.CommitAsync(ct);
            return Result<bool>.Success(true);
        }
    }

