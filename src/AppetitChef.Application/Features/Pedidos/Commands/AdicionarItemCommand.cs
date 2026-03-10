using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Application.Features.Pedidos.Commands
{
    public record AdicionarItemCommand(int PedidoId, int? ProdutoId, int? ComboId,
        int Quantidade, string? Observacao) : IRequest<Result<int>>;

    public class AdicionarItemHandler(IUnitOfWork uow) : IRequestHandler<AdicionarItemCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(AdicionarItemCommand request, CancellationToken ct)
        {
            var pedido = await uow.Pedidos.GetComItensAsync(request.PedidoId, ct)
                ?? throw new NotFoundException(nameof(Pedido), request.PedidoId);

            decimal preco = 0;
            if (request.ProdutoId.HasValue)
            {
                var produto = await uow.Produtos.GetByIdAsync(request.ProdutoId.Value, ct)
                    ?? throw new NotFoundException(nameof(Produto), request.ProdutoId.Value);
                preco = produto.PrecoVenda;
            }

            var item = pedido.AdicionarItem(request.ProdutoId, request.ComboId, request.Quantidade, preco);
            uow.Pedidos.Update(pedido);
            await uow.CommitAsync(ct);
            return Result<int>.Success(item.Id);
        }
    }
}
