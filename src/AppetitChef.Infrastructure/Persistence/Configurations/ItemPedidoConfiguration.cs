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
    public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> b)
        {
            b.ToTable("item_pedido");
            b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.Property(x => x.PrecoUnitario).HasPrecision(10, 2);
            b.Property(x => x.Desconto).HasPrecision(10, 2);

            // Removemos ou comentamos o Ignore de propriedades inexistentes
            // b.Ignore(x => x.Subtotal); 

            b.HasOne(x => x.Produto).WithMany().HasForeignKey(x => x.ProdutoId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
