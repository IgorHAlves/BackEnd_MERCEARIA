using MERCEARIA.Models;
using Microsoft.EntityFrameworkCore;

namespace MERCEARIA.Data
{
    public class MerceariaDataContext : DbContext
    {
        public DbSet<Cliente> Clientes{ get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidosItens { get; set; }

        public DbSet<Estoque> Estoque { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Mercearia;User ID=SA;Password=1q2w3e4r@#$;TrustServerCertificate=True");
            
        }
    }
}
    