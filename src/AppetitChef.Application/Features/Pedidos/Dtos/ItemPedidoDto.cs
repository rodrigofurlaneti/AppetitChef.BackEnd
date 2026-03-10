namespace AppetitChef.Application.Features.Pedidos.Dtos
{
    public record ItemPedidoDto(int Id, string? Produto, string? Combo, int Quantidade,
        decimal PrecoUnitario, decimal Subtotal, string Status, string? Observacao);
}
