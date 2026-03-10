using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Application.Features.Pedidos.Dtos;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Application.Features.Pedidos.Queries
{
    public record GetPedidoByIdQuery(int PedidoId) : IRequest<Result<PedidoDto>>;

    public class GetPedidoByIdHandler(IUnitOfWork uow) : IRequestHandler<GetPedidoByIdQuery, Result<PedidoDto>>
    {
        public async Task<Result<PedidoDto>> Handle(GetPedidoByIdQuery request, CancellationToken ct)
        {
            var p = await uow.Pedidos.GetComItensAsync(request.PedidoId, ct)
                ?? throw new NotFoundException(nameof(Pedido), request.PedidoId);

            var dto = new PedidoDto(
                p.Id, p.FilialId, p.MesaId,
                p.Cliente?.Nome, p.FuncionarioId.ToString(),
                p.Tipo.ToString(), p.Status.ToString(),
                p.Subtotal, p.Desconto, p.TaxaServico, p.Total,
                p.Observacao, p.CreatedAt,
                p.Itens.Select(i => new ItemPedidoDto(
                    i.Id, i.Produto?.Nome, null,
                    i.Quantidade, i.PrecoUnitario, i.Subtotal,
                    i.Status.ToString(), i.Observacao)));

            return Result<PedidoDto>.Success(dto);
        }
    }
}
