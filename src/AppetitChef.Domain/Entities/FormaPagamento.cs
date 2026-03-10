using AppetitChef.Domain.Common;

namespace AppetitChef.Domain.Entities;

public class FormaPagamento : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public bool Ativo { get; private set; } = true;

    // Construtor para o EF
    protected FormaPagamento() { }

    public static FormaPagamento Criar(string nome) => new() { Nome = nome };
}