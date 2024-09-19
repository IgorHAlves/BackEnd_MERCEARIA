using MERCEARIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MERCEARIA.Data.Mappings
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            //Tabela
            builder.ToTable("Pedido");
            //Chave primaria
            builder.HasKey(x => x.Id);
            //Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            //Propriedades chave estrangeira
            builder.HasOne(x => x.Cliente)
                .WithMany()
                .HasConstraintName("FK_Pedido_Cliente")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Itens)
              .WithOne(y => y.Pedido)
              .HasConstraintName("FK_Pedido_PedidoItem")
              .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Pago)
              .IsRequired()
              .HasColumnName("Ativo")
              .HasColumnType("boolean")
              .HasMaxLength(100);
        }
    }
}
