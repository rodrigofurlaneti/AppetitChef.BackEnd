namespace AppetitChef.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public sealed class MesaIndisponivelException : DomainException
{
    public MesaIndisponivelException(int mesaId)
        : base($"Mesa {mesaId} não está disponível para abertura de pedido.") { }
}

public sealed class PedidoJaFechadoException : DomainException
{
    public PedidoJaFechadoException(int pedidoId)
        : base($"Pedido {pedidoId} já foi fechado e não pode ser alterado.") { }
}

public sealed class EstoqueInsuficienteException : DomainException
{
    public EstoqueInsuficienteException(string insumo, decimal disponivel, decimal necessario)
        : base($"Estoque insuficiente para '{insumo}'. Disponível: {disponivel}, Necessário: {necessario}.") { }
}

public sealed class ProdutoIndisponivelException : DomainException
{
    public ProdutoIndisponivelException(string produto)
        : base($"O produto '{produto}' não está disponível no momento.") { }
}

public sealed class OrigemItemPedidoInvalidaException : DomainException
{
    public OrigemItemPedidoInvalidaException()
        : base("Um item de pedido deve referenciar um Produto ou um Combo, nunca os dois ou nenhum.") { }
}
