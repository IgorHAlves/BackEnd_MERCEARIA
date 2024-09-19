using MERCEARIA.Models;

namespace MERCEARIA.ViewModels.PedidoVM
{
    public class VisualizarPedidoViewModel
    {
        public Cliente Cliente { get; set; }
        public List<PedidoItem> Itens { get; set; }
        public bool Pago { get; set; }
    }
}
