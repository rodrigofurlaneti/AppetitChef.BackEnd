using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Exceptions;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Pedidos.Commands
{
    public record FecharPedidoCommand(int PedidoId, bool AplicarTaxaServico = true) : IRequest<Result<decimal>>;

    public class FecharPedidoHandler(IUnitOfWork uow) : IRequestHandler<FecharPedidoCommand, Result<decimal>>
    {
        public async Task<Result<decimal>> Handle(FecharPedidoCommand request, CancellationToken ct)
        {
            var pedido = await uow.Pedidos.GetComItensAsync(request.PedidoId, ct)
                ?? throw new NotFoundException(nameof(Pedido), request.PedidoId);

            pedido.Fechar(request.AplicarTaxaServico);

            if (pedido.MesaId.HasValue)
            {
                var mesa = await uow.Mesas.GetByIdAsync(pedido.MesaId.Value, ct);
                mesa?.Liberar();
                if (mesa != null) uow.Mesas.Update(mesa);
            }

            uow.Pedidos.Update(pedido);
            await uow.CommitAsync(ct);
            return Result<decimal>.Success(pedido.Total);
        }
    }
}