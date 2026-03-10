using AppetitChef.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Filial
    // ─────────────────────────────────────────────
    public class Filial : BaseEntity
    {
        public string Nome { get; private set; } = null!;
        public string Cnpj { get; private set; } = null!;
        public string? Telefone { get; private set; }
        public string? Email { get; private set; }
        public bool Ativa { get; private set; } = true;

        private readonly List<Area> _areas = [];
        public IReadOnlyCollection<Area> Areas => _areas.AsReadOnly();

        protected Filial() { }

        public static Filial Criar(string nome, string cnpj, string? telefone, string? email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nome);
            ArgumentException.ThrowIfNullOrWhiteSpace(cnpj);
            return new Filial { Nome = nome, Cnpj = cnpj, Telefone = telefone, Email = email };
        }
    }
}
