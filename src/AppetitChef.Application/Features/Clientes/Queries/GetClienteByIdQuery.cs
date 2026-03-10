using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using AppetitChef.Application.Features.Clientes.Dto;
using MediatR;

namespace AppetitChef.Application.Features.Clientes.Queries
{
    public record GetClienteByIdQuery(int ClienteId) : IRequest<ClienteDto>;

    public class GetClienteByIdHandler(IUnitOfWork uow) : IRequestHandler<GetClienteByIdQuery, ClienteDto>
    {
        public async Task<ClienteDto> Handle(GetClienteByIdQuery req, CancellationToken ct)
        {
            var c = await uow.Clientes.GetByIdAsync(req.ClienteId, ct)
                ?? throw new NotFoundException(nameof(Cliente), req.ClienteId);

            return new ClienteDto(c.Id, c.Nome, c.Cpf, c.Email, c.Telefone, c.PontosFidelidade);
        }
    }
}