using AppetitChef.Domain.Common;

namespace AppetitChef.Domain.Events;

public sealed record PedidoAbertoDomainEvent(int PedidoId, int? MesaId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record PedidoFechadoDomainEvent(int PedidoId, int? ClienteId, decimal Total) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ItemEntregeDomainEvent(int ItemId, int PedidoId, int? ProdutoId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
