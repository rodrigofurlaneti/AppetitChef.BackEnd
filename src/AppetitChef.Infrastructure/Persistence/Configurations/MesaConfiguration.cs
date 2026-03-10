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
    public class MesaConfiguration : IEntityTypeConfiguration<Mesa>
    {
        public void Configure(EntityTypeBuilder<Mesa> b)
        {
            b.ToTable("mesa");
            b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);

            // Verifique se Area e Mesas existem no Domain. Se der erro, comente esta linha:
            b.HasOne(x => x.Area).WithMany(a => a.Mesas).HasForeignKey(x => x.AreaId);

            b.HasIndex(x => new { x.AreaId, x.Numero }).IsUnique();
        }
    }
}
