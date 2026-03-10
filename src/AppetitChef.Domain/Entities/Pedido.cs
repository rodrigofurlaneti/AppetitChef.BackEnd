using AppetitChef.Domain.Common;
using AppetitChef.Domain.Enums;
using AppetitChef.Domain.Events;
using AppetitChef.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Pedido (aggregate root)
    // ─────────────────────────────────────────────
    public class Pedido : BaseEntity
    {
        public int FilialId { get; private set; }
        public int? MesaId { get; private set; }
        public Mesa? Mesa { get; private set; }
        public int? ClienteId { get; private set; }
        public Cliente? Cliente { get; private set; }
        public int FuncionarioId { get; private set; }
        public TipoPedido Tipo { get; private set; }
        public StatusPedido Status { get; private set; } = StatusPedido.Aberto;
        public decimal Subtotal { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal TaxaServico { get; private set; }
        public decimal Total => Subtotal - Desconto + TaxaServico;
        public string? Observacao { get; private set; }

        private readonly List<ItemPedido> _itens = [];
        public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

        protected Pedido() { }

        public static Pedido Criar(int filialId, int funcionarioId, TipoPedido tipo,
            int? mesaId = null, int? clienteId = null, string? observacao = null)
        {
            var pedido = new Pedido
            {
                FilialId = filialId,
                FuncionarioId = funcionarioId,
                Tipo = tipo,
                MesaId = mesaId,
                ClienteId = clienteId,
                Observacao = observacao
            };
            pedido.AddDomainEvent(new PedidoAbertoDomainEvent(pedido.Id, mesaId));
            return pedido;
        }

        public ItemPedido AdicionarItem(int? produtoId, int? comboId, int quantidade, decimal precoUnitario)
        {
            if (Status != StatusPedido.Aberto)
                throw new PedidoJaFechadoException(Id);

            var item = ItemPedido.Criar(Id, produtoId, comboId, quantidade, precoUnitario);
            _itens.Add(item);
            RecalcularSubtotal();
            SetUpdatedAt();
            return item;
        }

        public void CancelarItem(int itemId)
        {
            var item = _itens.FirstOrDefault(i => i.Id == itemId)
                ?? throw new KeyNotFoundException($"Item {itemId} não encontrado.");
            item.Cancelar();
            RecalcularSubtotal();
            SetUpdatedAt();
        }

        public void Fechar(bool aplicarTaxaServico = true)
        {
            if (Status == StatusPedido.Fechado) throw new PedidoJaFechadoException(Id);
            TaxaServico = aplicarTaxaServico ? Math.Round(Subtotal * 0.10m, 2) : 0;
            Status = StatusPedido.Fechado;
            AddDomainEvent(new PedidoFechadoDomainEvent(Id, ClienteId, Total));
            SetUpdatedAt();
        }

        public void Cancelar()
        {
            if (Status == StatusPedido.Fechado) throw new PedidoJaFechadoException(Id);
            Status = StatusPedido.Cancelado;
            SetUpdatedAt();
        }

        public void AplicarDesconto(decimal desconto)
        {
            if (desconto < 0) throw new ArgumentException("Desconto não pode ser negativo.");
            Desconto = desconto;
            SetUpdatedAt();
        }

        private void RecalcularSubtotal() =>
            Subtotal = _itens.Where(i => i.Status != StatusItemPedido.Cancelado).Sum(i => i.Subtotal);
    }
}
