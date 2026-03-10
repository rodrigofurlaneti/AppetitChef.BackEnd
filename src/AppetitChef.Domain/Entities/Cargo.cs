using AppetitChef.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Cargo
    // ─────────────────────────────────────────────
    public class Cargo : BaseEntity
    {
        public string Nome { get; private set; } = null!;
        public string? Descricao { get; private set; }
        public decimal SalarioBase { get; private set; }

        protected Cargo() { }

        public static Cargo Criar(string nome, decimal salarioBase, string? descricao = null) =>
            new() { Nome = nome, SalarioBase = salarioBase, Descricao = descricao };
    }
}
