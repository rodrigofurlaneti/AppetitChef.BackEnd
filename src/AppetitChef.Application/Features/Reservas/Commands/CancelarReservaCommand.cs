using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Reservas.Commands;

public record CancelarReservaCommand(int ReservaId) : IRequest<Result<bool>>;

public class CancelarReservaHandler(IUnitOfWork uow) : IRequestHandler<CancelarReservaCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CancelarReservaCommand req, CancellationToken ct)
    {
        var reserva = await uow.Reservas.GetByIdAsync(req.ReservaId, ct)
            ?? throw new NotFoundException(nameof(Reserva), req.ReservaId);

        if (reserva.MesaId.HasValue)
        {
            var mesa = await uow.Mesas.GetByIdAsync(reserva.MesaId.Value, ct);
            mesa?.Liberar();
            if (mesa is not null) uow.Mesas.Update(mesa);
        }

        reserva.Cancelar();
        uow.Reservas.Update(reserva);
        await uow.CommitAsync(ct);
        return Result<bool>.Success(true);
    }
}
