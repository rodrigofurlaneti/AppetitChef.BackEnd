using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Application.Features.Pedidos.Dtos
{
    public record PedidoDto(int Id, int FilialId, int? MesaId, string? Cliente,
        string Funcionario, string Tipo, string Status,
        decimal Subtotal, decimal Desconto, decimal TaxaServico, decimal Total,
        string? Observacao, DateTime CriadoEm, IEnumerable<ItemPedidoDto> Itens);
}
