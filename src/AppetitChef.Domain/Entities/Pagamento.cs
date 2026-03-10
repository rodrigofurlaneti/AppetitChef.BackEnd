using AppetitChef.Domain.Common;
using AppetitChef.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Pagamento
    // ─────────────────────────────────────────────
    public class Pagamento : BaseEntity
    {
        public int PedidoId { get; private set; }
        public int FormaPagamentoId { get; private set; }
        public decimal Valor { get; private set; }
        public decimal Troco { get; private set; }
        public string? Nsu { get; private set; }
        public StatusPagamento Status { get; private set; } = StatusPagamento.Pendente;

        protected Pagamento() { }

        public static Pagamento Criar(int pedidoId, int formaPagamentoId, decimal valor, decimal troco = 0) =>
            new() { PedidoId = pedidoId, FormaPagamentoId = formaPagamentoId, Valor = valor, Troco = troco };

        public void Aprovar(string? nsu = null) { Status = StatusPagamento.Aprovado; Nsu = nsu; SetUpdatedAt(); }
        public void Recusar() { Status = StatusPagamento.Recusado; SetUpdatedAt(); }
        public void Estornar() { Status = StatusPagamento.Estornado; SetUpdatedAt(); }
    }
}
