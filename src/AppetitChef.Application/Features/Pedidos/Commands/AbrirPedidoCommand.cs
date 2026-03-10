using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Enums;
using AppetitChef.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Application.Features.Pedidos.Commands
{
    // ── Abrir Pedido ──
    public record AbrirPedidoCommand(int FilialId, int FuncionarioId, TipoPedido Tipo,
        int? MesaId, int? ClienteId, string? Observacao) : IRequest<Result<int>>;

    public class AbrirPedidoHandler(IUnitOfWork uow) : IRequestHandler<AbrirPedidoCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(AbrirPedidoCommand request, CancellationToken ct)
        {
            if (request.MesaId.HasValue)
            {
                var mesa = await uow.Mesas.GetByIdAsync(request.MesaId.Value, ct)
                    ?? throw new NotFoundException(nameof(Mesa), request.MesaId.Value);
                mesa.Ocupar();
                uow.Mesas.Update(mesa);
            }

            var pedido = Pedido.Criar(request.FilialId, request.FuncionarioId,
                request.Tipo, request.MesaId, request.ClienteId, request.Observacao);

            await uow.Pedidos.AddAsync(pedido, ct);
            await uow.CommitAsync(ct);
            return Result<int>.Success(pedido.Id);
        }
    }
}
