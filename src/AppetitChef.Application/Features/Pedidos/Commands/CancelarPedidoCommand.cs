using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Exceptions;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Pedidos.Commands
{
    public record CancelarPedidoCommand(int PedidoId) : IRequest<Result<bool>>;

    public class CancelarPedidoHandler(IUnitOfWork uow) : IRequestHandler<CancelarPedidoCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(CancelarPedidoCommand req, CancellationToken ct)
        {
            var pedido = await uow.Pedidos.GetByIdAsync(req.PedidoId, ct);
            if (pedido == null) return Result<bool>.Failure("Pedido não encontrado.");

            pedido.Cancelar();
            uow.Pedidos.Update(pedido);
            await uow.CommitAsync(ct);

            return Result<bool>.Success(true);
        }
    }
}

