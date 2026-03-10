using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using AppetitChef.Application.Features.Clientes.Dto;
using MediatR;

namespace AppetitChef.Application.Features.Clientes.Queries
{
    public record GetClienteByCpfQuery(string Cpf) : IRequest<ClienteDto>;

    public class GetClienteByCpfHandler(IUnitOfWork uow) : IRequestHandler<GetClienteByCpfQuery, ClienteDto>
    {
        public async Task<ClienteDto> Handle(GetClienteByCpfQuery req, CancellationToken ct)
        {
            var c = await uow.Clientes.GetByCpfAsync(req.Cpf, ct)
                ?? throw new NotFoundException(nameof(Cliente), req.Cpf);

            return new ClienteDto(c.Id, c.Nome, c.Cpf, c.Email, c.Telefone, c.PontosFidelidade);
        }
    }
}
