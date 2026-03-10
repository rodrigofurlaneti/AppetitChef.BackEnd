using AppetitChef.Application.Common.Models;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Enums;
using AppetitChef.Domain.Interfaces;
using MediatR;

namespace AppetitChef.Application.Features.Estoque.Commands;

public record RegistrarMovimentoCommand(int InsumoId, TipoMovimentoEstoque Tipo,
    decimal Quantidade, int? FuncionarioId, string? Observacao) : IRequest<Result<bool>>;

public class RegistrarMovimentoHandler(IInsumoRepository insumoRepo, IRepository<MovimentoEstoque> movRepo, IUnitOfWork uow)
    : IRequestHandler<RegistrarMovimentoCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(RegistrarMovimentoCommand cmd, CancellationToken ct)
    {
        // 1. Alterado de ObterPorIdAsync para GetByIdAsync
        var insumo = await insumoRepo.GetByIdAsync(cmd.InsumoId, ct)
            ?? throw new InvalidOperationException($"Insumo {cmd.InsumoId} não encontrado.");

        switch (cmd.Tipo)
        {
            case TipoMovimentoEstoque.Entrada:
                insumo.Creditar(cmd.Quantidade); break;
            case TipoMovimentoEstoque.Saida:
            case TipoMovimentoEstoque.Perda:
                insumo.Debitar(cmd.Quantidade); break;
        }

        var mov = MovimentoEstoque.Registrar(cmd.InsumoId, cmd.Tipo, cmd.Quantidade, cmd.FuncionarioId, cmd.Observacao);

        // 2. Alterado de AdicionarAsync para AddAsync
        await movRepo.AddAsync(mov, ct);

        // 3. Alterado de Atualizar para Update
        insumoRepo.Update(insumo);

        await uow.CommitAsync(ct);

        return Result<bool>.Success(true);
    }
}