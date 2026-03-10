using AppetitChef.Application.Features.Pedidos.Dtos;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Pedidos.Queries;

public record GetPedidosAbertosQuery(int FilialId) : IRequest<IEnumerable<PedidoResumoDto>>;

public class GetPedidosAbertosHandler(IUnitOfWork uow) : IRequestHandler<GetPedidosAbertosQuery, IEnumerable<PedidoResumoDto>>
{
    public async Task<IEnumerable<PedidoResumoDto>> Handle(GetPedidosAbertosQuery req, CancellationToken ct)
    {
        var pedidos = await uow.Pedidos.GetAbertosAsync(req.FilialId, ct);
        return pedidos.Select(p => new PedidoResumoDto(
            p.Id,
            p.Mesa?.Numero,
            p.Cliente?.Nome,
            p.Status.ToString(),
            p.Total,
            p.CreatedAt));
    }
}
