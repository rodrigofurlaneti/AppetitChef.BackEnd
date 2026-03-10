using AppetitChef.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Cliente
    // ─────────────────────────────────────────────
    public class Cliente : BaseEntity
    {
        public string Nome { get; private set; } = null!;
        public string? Cpf { get; private set; }
        public DateOnly? DataNascimento { get; private set; }
        public string? Telefone { get; private set; }
        public string? Email { get; private set; }
        public int PontosFidelidade { get; private set; }
        public bool Ativo { get; private set; } = true;

        private readonly List<FidelidadeTransacao> _transacoes = [];
        public IReadOnlyCollection<FidelidadeTransacao> Transacoes => _transacoes.AsReadOnly();

        protected Cliente() { }

        public static Cliente Criar(string nome, string? cpf = null, string? email = null, string? telefone = null) =>
            new() { Nome = nome, Cpf = cpf, Email = email, Telefone = telefone };

        public void CreditarPontos(int pontos, int pedidoId)
        {
            if (pontos <= 0) return;
            PontosFidelidade += pontos;
            _transacoes.Add(FidelidadeTransacao.Criar(Id, pedidoId, "credito", pontos, $"Pedido #{pedidoId}"));
            SetUpdatedAt();
        }

        public void DebitarPontos(int pontos, string descricao)
        {
            if (pontos > PontosFidelidade)
                throw new InvalidOperationException("Pontos insuficientes para resgate.");
            PontosFidelidade -= pontos;
            _transacoes.Add(FidelidadeTransacao.Criar(Id, null, "debito", pontos, descricao));
            SetUpdatedAt();
        }
    }
}
