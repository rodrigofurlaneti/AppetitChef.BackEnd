using AppetitChef.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // FidelidadeTransacao
    // ─────────────────────────────────────────────
    public class FidelidadeTransacao : BaseEntity
    {
        public int ClienteId { get; private set; }
        public int? PedidoId { get; private set; }
        public string Tipo { get; private set; } = null!; // credito | debito | expiracao
        public int Pontos { get; private set; }
        public string? Descricao { get; private set; }

        protected FidelidadeTransacao() { }

        internal static FidelidadeTransacao Criar(int clienteId, int? pedidoId, string tipo, int pontos, string? descricao) =>
            new() { ClienteId = clienteId, PedidoId = pedidoId, Tipo = tipo, Pontos = pontos, Descricao = descricao };
    }
}
