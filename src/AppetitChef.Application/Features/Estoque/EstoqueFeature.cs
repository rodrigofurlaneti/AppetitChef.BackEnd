using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace AppetitChef.Application.Features.Estoque;

public record InsumoDto(int Id, string Nome, string UnidadeMedida, decimal EstoqueAtual,
    decimal EstoqueMinimo, decimal CustoUnitario, bool EstaCritico);

public record MovimentarEstoqueCommand(int InsumoId, string Tipo, decimal Quantidade,
    string? Observacao, int? FuncionarioId) : IRequest;

public class MovimentarEstoqueValidator : AbstractValidator<MovimentarEstoqueCommand>
{
    private static readonly string[] TiposValidos = ["entrada", "saida", "ajuste", "perda"];
    public MovimentarEstoqueValidator()
    {
        RuleFor(x => x.InsumoId).GreaterThan(0);
        RuleFor(x => x.Quantidade).GreaterThan(0);
        RuleFor(x => x.Tipo).Must(t => TiposValidos.Contains(t.ToLower()))
            .WithMessage("Tipo deve ser: entrada, saida, ajuste ou perda.");
    }
}

public class MovimentarEstoqueHandler(IUnitOfWork uow) : IRequestHandler<MovimentarEstoqueCommand>
{
    public async Task Handle(MovimentarEstoqueCommand req, CancellationToken ct)
    {
        var insumo = await uow.Insumos.GetByIdAsync(req.InsumoId, ct)
            ?? throw new NotFoundException(nameof(Insumo), req.InsumoId);

        switch (req.Tipo.ToLower())
        {
            case "entrada": insumo.Creditar(req.Quantidade); break;
            case "saida": case "perda": insumo.Debitar(req.Quantidade); break;
            case "ajuste":
                if (req.Quantidade > insumo.EstoqueAtual) insumo.Creditar(req.Quantidade - insumo.EstoqueAtual);
                else insumo.Debitar(insumo.EstoqueAtual - req.Quantidade);
                break;
        }

        uow.Insumos.Update(insumo);
        await uow.CommitAsync(ct);
    }
}

public record GetInsumoCriticosQuery : IRequest<IEnumerable<InsumoDto>>;

public class GetInsumosCriticosHandler(IUnitOfWork uow) : IRequestHandler<GetInsumoCriticosQuery, IEnumerable<InsumoDto>>
{
    public async Task<IEnumerable<InsumoDto>> Handle(GetInsumoCriticosQuery req, CancellationToken ct)
    {
        var insumos = await uow.Insumos.GetCriticosAsync(ct);
        return insumos.Select(i => new InsumoDto(i.Id, i.Nome, i.UnidadeMedida,
            i.EstoqueAtual, i.EstoqueMinimo, i.CustoUnitario, i.EstaCritico()));
    }
}

public record CriarInsumoCommand(string Nome, string UnidadeMedida, decimal EstoqueMinimo,
    decimal CustoUnitario, int? FornecedorId) : IRequest<int>;

public class CriarInsumoValidator : AbstractValidator<CriarInsumoCommand>
{
    public CriarInsumoValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
        RuleFor(x => x.UnidadeMedida).NotEmpty().MaximumLength(20);
        RuleFor(x => x.EstoqueMinimo).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CustoUnitario).GreaterThanOrEqualTo(0);
    }
}

public class CriarInsumoHandler(IUnitOfWork uow) : IRequestHandler<CriarInsumoCommand, int>
{
    public async Task<int> Handle(CriarInsumoCommand req, CancellationToken ct)
    {
        var insumo = Insumo.Criar(req.Nome, req.UnidadeMedida, req.EstoqueMinimo, req.CustoUnitario, req.FornecedorId);
        await uow.Insumos.AddAsync(insumo, ct);
        await uow.CommitAsync(ct);
        return insumo.Id;
    }
}
