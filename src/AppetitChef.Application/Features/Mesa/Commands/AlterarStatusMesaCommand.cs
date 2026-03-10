using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Enums;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Mesa.Commands; 

public record AlterarStatusMesaCommand(int MesaId, StatusMesa NovoStatus) : IRequest<Result<bool>>;

public class AlterarStatusMesaHandler(IUnitOfWork uow) : IRequestHandler<AlterarStatusMesaCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AlterarStatusMesaCommand req, CancellationToken ct)
    {
        // O repositório uow.Mesas usa o padrão de nomenclatura correto (GetByIdAsync)
        var mesa = await uow.Mesas.GetByIdAsync(req.MesaId, ct);

        if (mesa == null)
            return Result<bool>.Failure("Mesa não encontrada.");

        switch (req.NovoStatus)
        {
            case StatusMesa.Livre:
                mesa.Liberar();
                break;
            case StatusMesa.Reservada:
                mesa.Reservar();
                break;
            case StatusMesa.Manutencao:
                mesa.ColocarEmManutencao();
                break;
            default:
                return Result<bool>.Failure("Status inválido para alteração manual.");
        }

        uow.Mesas.Update(mesa);
        await uow.CommitAsync(ct);

        return Result<bool>.Success(true);
    }
}