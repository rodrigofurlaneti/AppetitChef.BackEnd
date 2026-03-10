using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace AppetitChef.Application.Features.Clientes.Commands
{
    public record CriarClienteCommand(string Nome, string? Cpf, string? Email, string? Telefone) : IRequest<int>;

    public class CriarClienteValidator : AbstractValidator<CriarClienteCommand>
    {
        public CriarClienteValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).EmailAddress().When(x => x.Email is not null);
            RuleFor(x => x.Cpf).Length(14).When(x => x.Cpf is not null)
                .WithMessage("CPF deve ter 14 caracteres no formato 000.000.000-00.");
        }
    }

    public class CriarClienteHandler(IUnitOfWork uow) : IRequestHandler<CriarClienteCommand, int>
    {
        public async Task<int> Handle(CriarClienteCommand req, CancellationToken ct)
        {
            if (req.Cpf is not null)
            {
                var existing = await uow.Clientes.GetByCpfAsync(req.Cpf, ct);
                if (existing is not null)
                    throw new InvalidOperationException("CPF já cadastrado.");
            }

            var cliente = Cliente.Criar(req.Nome, req.Cpf, req.Email, req.Telefone);
            await uow.Clientes.AddAsync(cliente, ct);
            await uow.CommitAsync(ct);
            return cliente.Id;
        }
    }
}