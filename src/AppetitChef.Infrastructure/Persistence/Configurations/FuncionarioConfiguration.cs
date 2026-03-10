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
    public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> b)
        {
            b.ToTable("funcionario");
            b.HasIndex(x => x.Cpf).IsUnique();
            b.HasIndex(x => x.Login).IsUnique();

            // Comentados Perfil e Filial que causavam erro CS1061 no seu log
            // b.Property(x => x.Perfil).HasConversion<string>().HasMaxLength(20);
            // b.HasOne(x => x.Filial).WithMany(f => f.Funcionarios).HasForeignKey(x => x.FilialId);
        }
    }
}
