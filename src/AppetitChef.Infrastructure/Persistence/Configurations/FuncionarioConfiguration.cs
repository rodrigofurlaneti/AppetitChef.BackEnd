using AppetitChef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppetitChef.Infrastructure.Persistence.Configurations
{
    public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> b)
        {
            b.ToTable("funcionarios");
            b.HasIndex(x => x.Cpf).IsUnique();
            b.HasIndex(x => x.Login).IsUnique();
            // funcionarios TEM updated_at — nao ignorar
            b.HasOne(x => x.Cargo)
             .WithMany()
             .HasForeignKey(x => x.CargoId);
        }
    }
}
