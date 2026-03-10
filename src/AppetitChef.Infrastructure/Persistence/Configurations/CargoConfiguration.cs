using AppetitChef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppetitChef.Infrastructure.Persistence.Configurations
{
    public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> b)
        {
            b.ToTable("cargos");
            // cargos tem updated_at — não ignorar
        }
    }
}
