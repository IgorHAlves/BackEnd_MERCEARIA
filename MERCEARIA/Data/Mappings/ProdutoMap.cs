using MERCEARIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MERCEARIA.Data.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            //Tabela
            builder.ToTable("Produto");
            //Chave primaria
            builder.HasKey(x => x.Id);
            //Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            //Propriedades
            builder.Property(x => x.NomeProduto)
                .IsRequired()
                .HasColumnName("NomeProduto")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(60);

            builder.Property(x => x.PrecoUnit)
                .IsRequired()
                .HasColumnName("Preco")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);
        }
    }
}
