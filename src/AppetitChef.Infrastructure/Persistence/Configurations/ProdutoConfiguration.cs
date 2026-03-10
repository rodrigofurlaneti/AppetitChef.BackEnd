using AppetitChef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Infrastructure.Persistence.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> b)
        {
            b.ToTable("produto");
            b.HasIndex(x => x.Codigo).IsUnique();
            b.Property(x => x.PrecoVenda).HasPrecision(10, 2);
            b.Property(x => x.CustoEstimado).HasPrecision(10, 2);

            // Removido MargemLucro pois causava erro CS1061
            b.HasOne(x => x.Categoria).WithMany().HasForeignKey(x => x.CategoriaId);
        }
    }
}
