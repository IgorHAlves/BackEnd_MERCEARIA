namespace MERCEARIA.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public List<PedidoItem> Itens { get; set; } = new();
        public bool Pago { get; set; }  
        //public float ValorTotal()
    }
}
