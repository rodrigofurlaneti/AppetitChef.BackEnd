using AppetitChef.Application.Features.Mesa.Dtos;
using AppetitChef.Domain.Enums;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Mesa.Queries;

public record ListarMesasQuery(int FilialId, bool? SomenteDisponiveis) : IRequest<IEnumerable<MesaQueryDto>>;

public class ListarMesasHandler(IUnitOfWork uow) : IRequestHandler<ListarMesasQuery, IEnumerable<MesaQueryDto>>
{
    public async Task<IEnumerable<MesaQueryDto>> Handle(ListarMesasQuery request, CancellationToken ct)
    {
        var mesas = await uow.Mesas.GetByFilialAsync(request.FilialId, ct);

        if (request.SomenteDisponiveis == true)
            mesas = mesas.Where(m => m.Status == StatusMesa.Livre);

        return mesas.Select(m => new MesaQueryDto(
            m.Id,
            m.Numero,
            m.Status.ToString(),
            m.Area?.Nome ?? "Geral"
        ));
    }
}
