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
    // CategoriaProducto
    // ─────────────────────────────────────────────
    public class CategoriaProduto : BaseEntity
    {
        public string Nome { get; private set; } = null!;
        public TipoCategoria Tipo { get; private set; }
        public string? Descricao { get; private set; }

        protected CategoriaProduto() { }

        public static CategoriaProduto Criar(string nome, TipoCategoria tipo, string? descricao = null) =>
            new() { Nome = nome, Tipo = tipo, Descricao = descricao };
    }
}
