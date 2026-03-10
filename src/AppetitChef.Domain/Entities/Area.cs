using AppetitChef.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Area
    // ─────────────────────────────────────────────
    public class Area : BaseEntity
    {
        public int FilialId { get; private set; }
        public Filial Filial { get; private set; } = null!;
        public string Nome { get; private set; } = null!;
        public string? Descricao { get; private set; }
        public bool Ativa { get; private set; } = true;

        private readonly List<Mesa> _mesas = [];
        public IReadOnlyCollection<Mesa> Mesas => _mesas.AsReadOnly();

        protected Area() { }

        public static Area Criar(int filialId, string nome, string? descricao = null) =>
            new() { FilialId = filialId, Nome = nome, Descricao = descricao };
    }
}
