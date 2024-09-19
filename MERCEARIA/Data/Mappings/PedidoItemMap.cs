using MERCEARIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MERCEARIA.Data.Mappings
{
    public class PedidoItemMap : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
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
            builder.HasOne(x => x.Pedido)   
                .WithMany()
                .HasConstraintName("FK_PedidoItem_Cliente")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Produto)
                .WithMany()
                .HasConstraintName("FK_PedidoItem_Produto")
                .OnDelete(DeleteBehavior.Cascade);


            builder.Property(x => x.Quantidade)
              .IsRequired()
              .HasColumnName("Quantidade")
              .HasColumnType("integer");
        }
    }
}
