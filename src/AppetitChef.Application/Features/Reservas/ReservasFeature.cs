using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace AppetitChef.Application.Features.Reservas;

// ==============================================================================
// DTOS (DATA TRANSFER OBJECTS) - Usados pelas Queries
// ==============================================================================
public record ReservaDto(int Id, int FilialId, string NomeContato, string Telefone,
    DateTime DataReserva, int NumPessoas, string Status, string? Observacao);

// ==============================================================================
// COMMANDS (ESCRITA/AÇÃO)
// ==============================================================================

// 1. Criar Reserva
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

        return Result<int>.Success(reserva.Id); // Corrigido para retornar Result<int>
    }
}

// 2. Confirmar Reserva
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

// ==============================================================================
// QUERIES (LEITURA/CONSULTA)
// ==============================================================================

public record GetReservasByDataQuery(int FilialId, DateTime Data) : IRequest<Result<IEnumerable<ReservaDto>>>;

public class GetReservasByDataHandler(IUnitOfWork uow) : IRequestHandler<GetReservasByDataQuery, Result<IEnumerable<ReservaDto>>>
{
    public async Task<Result<IEnumerable<ReservaDto>>> Handle(GetReservasByDataQuery req, CancellationToken ct)
    {
        var reservas = await uow.Reservas.GetByDataAsync(req.Data, req.FilialId, ct);

        var dtos = reservas.Select(r => new ReservaDto(r.Id, r.FilialId, r.NomeContato,
            r.TelefoneContato, r.DataReserva, r.NumPessoas, r.Status.ToString(), r.Observacao));

        return Result<IEnumerable<ReservaDto>>.Success(dtos);
    }
}