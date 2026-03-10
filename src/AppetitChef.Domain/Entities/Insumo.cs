using AppetitChef.Domain.Common;
using AppetitChef.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Insumo
    // ─────────────────────────────────────────────
    public class Insumo : BaseEntity
    {
        public int? FornecedorId { get; private set; }
        public string Nome { get; private set; } = null!;
        public string UnidadeMedida { get; private set; } = null!;
        public decimal EstoqueAtual { get; private set; }
        public decimal EstoqueMinimo { get; private set; }
        public decimal CustoUnitario { get; private set; }

        protected Insumo() { }

        public static Insumo Criar(string nome, string unidade, decimal estoqueMinimo, decimal custo, int? fornecedorId = null) =>
            new() { Nome = nome, UnidadeMedida = unidade, EstoqueMinimo = estoqueMinimo, CustoUnitario = custo, FornecedorId = fornecedorId };

        public void Creditar(decimal quantidade)
        {
            if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser positiva.");
            EstoqueAtual += quantidade;
            SetUpdatedAt();
        }

        public void Debitar(decimal quantidade)
        {
            if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser positiva.");
            if (quantidade > EstoqueAtual)
                throw new EstoqueInsuficienteException(Nome, EstoqueAtual, quantidade);
            EstoqueAtual -= quantidade;
            SetUpdatedAt();
        }

        public bool EstaCritico() => EstoqueAtual <= EstoqueMinimo;
    }
}
