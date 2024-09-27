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
            builder.ToTable("PedidoItem");
            //Chave primaria
            builder.HasKey(x => x.Id);
            //Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            //Propriedades chave estrangeira
            //builder.Property(x => x.IdPedido)
            //    .HasColumnName("IdPedido")
            //    .IsRequired();

            builder.HasOne(x => x.Pedido)
                .WithMany(x => x.Itens)
                .HasForeignKey(x => x.Pedido.Id)
                .HasConstraintName("FK_Pedido_Item_Pedido")
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
