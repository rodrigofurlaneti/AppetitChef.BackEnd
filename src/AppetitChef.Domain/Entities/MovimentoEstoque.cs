using AppetitChef.Domain.Common;
using AppetitChef.Domain.Enums;

namespace AppetitChef.Domain.Entities;

public class MovimentoEstoque : BaseEntity
{
    public int InsumoId { get; private set; }
    public TipoMovimentoEstoque Tipo { get; private set; }
    public decimal Quantidade { get; private set; }
    public int? FuncionarioId { get; private set; }
    public string? Observacao { get; private set; }

    protected MovimentoEstoque() { }

    public static MovimentoEstoque Registrar(int insumoId, TipoMovimentoEstoque tipo, decimal quantidade, int? funcionarioId, string? observacao)
    {
        return new MovimentoEstoque
        {
            InsumoId = insumoId,
            Tipo = tipo,
            Quantidade = quantidade,
            FuncionarioId = funcionarioId,
            Observacao = observacao
        };
    }
}