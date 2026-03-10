using AppetitChef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppetitChef.Infrastructure.Persistence.Configurations
{
    public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> b)
        {
            b.ToTable("reservas");
            b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            // Tabela reserva nÃ£o tem coluna updated_at
            b.Ignore(x => x.UpdatedAt);
        }
    }
}
