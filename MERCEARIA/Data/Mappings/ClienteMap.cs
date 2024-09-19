using MERCEARIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MERCEARIA.Data.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            //Tabela
            builder.ToTable("Cliente");
            //Chave primaria
            builder.HasKey(x => x.Id);
            //Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            //Propriedades
            builder.Property(x => x.NomeCliente)
                .IsRequired()
                .HasColumnName("NomeCliente")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnName("NomeCliente")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.DataNascimento)
                 .IsRequired()
                 .HasColumnName("DataNascimento")
                 .HasColumnType("DATE")
                 .HasMaxLength(100);

            builder.Property(x => x.Ativo)
              .IsRequired()
              .HasColumnName("Ativo")
              .HasColumnType("boolean")
              .HasMaxLength(100);
        }
    }
}
