using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Mesa.Commands;

public record CriarMesaCommand(int AreaId, int Numero, int Capacidade) : IRequest<Result<int>>;

public class CriarMesaHandler(IUnitOfWork uow) : IRequestHandler<CriarMesaCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CriarMesaCommand req, CancellationToken ct)
    {
        var mesa = AppetitChef.Domain.Entities.Mesa.Criar(req.AreaId, req.Numero, req.Capacidade);
        await uow.Mesas.AddAsync(mesa, ct);
        await uow.CommitAsync(ct);

        return Result<int>.Success(mesa.Id);
    }
}