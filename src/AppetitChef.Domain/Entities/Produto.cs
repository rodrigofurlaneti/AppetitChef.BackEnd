using AppetitChef.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Produto
    // ─────────────────────────────────────────────
    public class Produto : BaseEntity
    {
        public int CategoriaId { get; private set; }
        public CategoriaProduto Categoria { get; private set; } = null!;
        public string Codigo { get; private set; } = null!;
        public string Nome { get; private set; } = null!;
        public string? Descricao { get; private set; }
        public decimal PrecoVenda { get; private set; }
        public decimal CustoEstimado { get; private set; }
        public int TempoPreparo { get; private set; }
        public bool Disponivel { get; private set; } = true;
        public bool VendidoNoBar { get; private set; }
        public bool VendidoNoSalao { get; private set; } = true;
        public bool VendidoDelivery { get; private set; }
        public string? ImagemUrl { get; private set; }

        protected Produto() { }

        public static Produto Criar(int categoriaId, string codigo, string nome,
            decimal preco, decimal custo, int tempoPreparo, string? descricao = null)
        {
            if (preco < 0) throw new ArgumentException("Preço deve ser positivo.");
            return new Produto
            {
                CategoriaId = categoriaId,
                Codigo = codigo,
                Nome = nome,
                PrecoVenda = preco,
                CustoEstimado = custo,
                TempoPreparo = tempoPreparo,
                Descricao = descricao
            };
        }

        public void Ativar() { Disponivel = true; SetUpdatedAt(); }
        public void Desativar() { Disponivel = false; SetUpdatedAt(); }
        public void AtualizarPreco(decimal novoPreco)
        {
            if (novoPreco < 0) throw new ArgumentException("Preço deve ser positivo.");
            PrecoVenda = novoPreco;
            SetUpdatedAt();
        }
    }
}
