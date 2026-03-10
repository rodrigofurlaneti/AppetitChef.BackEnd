using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace AppetitChef.Application.Features.Reservas;

// ─── DTOs ─────────────────────────────────────────────────────────────────────
public record ReservaResumoDto(int Id, int FilialId, string NomeContato, string Telefone,
    DateTime DataReserva, int NumPessoas, string Status, string? Observacao);

// ─── Commands ─────────────────────────────────────────────────────────────────
public record CancelarReservaFeatureCommand(int ReservaId) : IRequest<Result<bool>>;

public class CancelarReservaFeatureHandler(IUnitOfWork uow) : IRequestHandler<CancelarReservaFeatureCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CancelarReservaFeatureCommand req, CancellationToken ct)
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
