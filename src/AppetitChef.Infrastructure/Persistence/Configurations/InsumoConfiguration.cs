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
    public class InsumoConfiguration : IEntityTypeConfiguration<Insumo>
    {
        public void Configure(EntityTypeBuilder<Insumo> b)
        {
            b.ToTable("insumo");
            b.Property(x => x.EstoqueAtual).HasPrecision(12, 3);
            b.Property(x => x.EstoqueMinimo).HasPrecision(12, 3);
            b.Property(x => x.CustoUnitario).HasPrecision(10, 4);

            // Removido EstoqueAbaixoMinimo pois causava erro CS1061
        }
    }
}
