using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Reservas.Commands;

public record ConfirmarReservaCommand(int ReservaId) : IRequest<Result<bool>>;

public class ConfirmarReservaHandler(IUnitOfWork uow) : IRequestHandler<ConfirmarReservaCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ConfirmarReservaCommand req, CancellationToken ct)
    {
        var reserva = await uow.Reservas.GetByIdAsync(req.ReservaId, ct)
            ?? throw new NotFoundException(nameof(Reserva), req.ReservaId);
        reserva.Confirmar();
        uow.Reservas.Update(reserva);
        await uow.CommitAsync(ct);
        return Result<bool>.Success(true);
    }
}
