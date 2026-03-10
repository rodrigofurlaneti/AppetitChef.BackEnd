using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Application.Features.Reservas.Queries
{
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
}
