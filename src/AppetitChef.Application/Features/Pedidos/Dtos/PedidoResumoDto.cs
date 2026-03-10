namespace AppetitChef.Application.Features.Pedidos.Dtos
{
    public record PedidoResumoDto(int Id, int? MesaNumero, string? Cliente,
        string Status, decimal Total, DateTime CriadoEm);
}
