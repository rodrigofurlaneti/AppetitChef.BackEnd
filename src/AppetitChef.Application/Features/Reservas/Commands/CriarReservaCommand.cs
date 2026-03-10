using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace AppetitChef.Application.Features.Reservas.Commands;

public record CriarReservaCommand(int FilialId, int FuncionarioId, string NomeContato,
    string TelefoneContato, DateTime DataReserva, int NumPessoas,
    int? ClienteId, int? MesaId, string? Observacao) : IRequest<Result<int>>;

public class CriarReservaValidator : AbstractValidator<CriarReservaCommand>
{
    public CriarReservaValidator()
    {
        RuleFor(x => x.NomeContato).NotEmpty().MaximumLength(100);
        RuleFor(x => x.TelefoneContato).NotEmpty();
        RuleFor(x => x.DataReserva).GreaterThan(DateTime.UtcNow).WithMessage("Data deve ser no futuro.");
        RuleFor(x => x.NumPessoas).GreaterThan(0);
    }
}

public class CriarReservaHandler(IUnitOfWork uow) : IRequestHandler<CriarReservaCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CriarReservaCommand req, CancellationToken ct)
    {
        if (req.MesaId.HasValue)
        {
            var mesa = await uow.Mesas.GetByIdAsync(req.MesaId.Value, ct)
                ?? throw new NotFoundException(nameof(Mesa), req.MesaId.Value);
            mesa.Reservar();
            uow.Mesas.Update(mesa);
        }
        var reserva = Reserva.Criar(req.FilialId, req.FuncionarioId, req.NomeContato,
            req.TelefoneContato, req.DataReserva, req.NumPessoas, req.ClienteId, req.MesaId);
        await uow.Reservas.AddAsync(reserva, ct);
        await uow.CommitAsync(ct);
        return Result<int>.Success(reserva.Id);
    }
}
