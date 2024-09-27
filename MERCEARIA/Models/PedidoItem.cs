namespace MERCEARIA.Models
{
    public class PedidoItem
    {
        public int Id { get; set; }
        //  public int IdPedido { get; set; }
        public Pedido Pedido { get; set; }
        public Produto Produto{ get; set; }
        public int Quantidade{ get; set; }
    }
}
