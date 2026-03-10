using AppetitChef.Domain.Common;
using AppetitChef.Domain.Enums;
using AppetitChef.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // ItemPedido
    // ─────────────────────────────────────────────
    public class ItemPedido : BaseEntity
    {
        public int PedidoId { get; private set; }
        public int? ProdutoId { get; private set; }
        public Produto? Produto { get; private set; }
        public int? ComboId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal Subtotal => Quantidade * PrecoUnitario - Desconto;
        public StatusItemPedido Status { get; private set; } = StatusItemPedido.Pendente;
        public string? Observacao { get; private set; }

        protected ItemPedido() { }

        internal static ItemPedido Criar(int pedidoId, int? produtoId, int? comboId, int quantidade, decimal preco)
        {
            if ((produtoId is null && comboId is null) || (produtoId is not null && comboId is not null))
                throw new OrigemItemPedidoInvalidaException();
            if (quantidade < 1) throw new ArgumentException("Quantidade deve ser pelo menos 1.");
            return new ItemPedido
            {
                PedidoId = pedidoId,
                ProdutoId = produtoId,
                ComboId = comboId,
                Quantidade = quantidade,
                PrecoUnitario = preco
            };
        }

        public void Cancelar() { Status = StatusItemPedido.Cancelado; SetUpdatedAt(); }
        public void MarcarEmPreparo() { Status = StatusItemPedido.EmPreparo; SetUpdatedAt(); }
        public void MarcarPronto() { Status = StatusItemPedido.Pronto; SetUpdatedAt(); }
        public void MarcarEntregue() { Status = StatusItemPedido.Entregue; SetUpdatedAt(); }
    }
}
