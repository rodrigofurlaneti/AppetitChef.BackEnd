using AppetitChef.Application.Common.Models;
using AppetitChef.Application.Features.Mesa.Dtos;
using AppetitChef.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Application.Features.Mesa.Queries
{
    public record GetMesasByFilialQuery(int FilialId) : IRequest<Result<IEnumerable<MesaDto>>>;

    public class GetMesasByFilialHandler(IUnitOfWork uow) : IRequestHandler<GetMesasByFilialQuery, Result<IEnumerable<MesaDto>>>
    {
        public async Task<Result<IEnumerable<MesaDto>>> Handle(GetMesasByFilialQuery req, CancellationToken ct)
        {
            var mesas = await uow.Mesas.GetByFilialAsync(req.FilialId, ct);
            var dtos = mesas.Select(m => new MesaDto(m.Id, m.AreaId, m.Area?.Nome ?? "", m.Numero, m.Capacidade, m.Status.ToString()));
            return Result<IEnumerable<MesaDto>>.Success(dtos);
        }
    }
}
