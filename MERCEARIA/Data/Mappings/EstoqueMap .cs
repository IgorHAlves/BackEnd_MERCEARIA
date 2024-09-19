using MERCEARIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MERCEARIA.Data.Mappings
{
    public class EstoqueMap : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            //Tabela
            builder.ToTable("Estoque");
            //Chave primaria
            builder.HasKey(x => x.Id);
            //Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            //Propriedades e chave estrangeira
            builder.HasOne(x => x.Produto)
                .WithMany()
                .HasConstraintName("FK_Estoque_Produto")
                .OnDelete(DeleteBehavior.Cascade);

            //Garantindo que o produto será unico no estoque
            builder.HasIndex(x => x.Produto)
                .IsUnique();

            builder.Property(x => x.Quantidade)
                .IsRequired()
                .HasColumnType("integer");
        }
    }
}
