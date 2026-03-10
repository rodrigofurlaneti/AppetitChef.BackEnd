using AppetitChef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppetitChef.Infrastructure.Persistence.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> b)
        {
            b.ToTable("clientes");
            b.HasIndex(x => x.Cpf).IsUnique();
            b.HasIndex(x => x.Email).IsUnique();
            b.Ignore(x => x.UpdatedAt);
        }
    }
}
