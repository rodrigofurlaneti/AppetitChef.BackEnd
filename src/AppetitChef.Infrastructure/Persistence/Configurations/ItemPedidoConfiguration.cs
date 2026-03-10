using AppetitChef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppetitChef.Infrastructure.Persistence.Configurations
{
    public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> b)
        {
            b.ToTable("itens_pedido");
            b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.Property(x => x.PrecoUnitario).HasPrecision(10, 2);
            b.Property(x => x.Desconto).HasPrecision(10, 2);
            b.Ignore(x => x.UpdatedAt);
            b.HasOne(x => x.Produto).WithMany().HasForeignKey(x => x.ProdutoId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
