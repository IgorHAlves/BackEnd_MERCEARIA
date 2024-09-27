using MERCEARIA.Models;

namespace MERCEARIA.ViewModels.PedidoVM
{
    public class VisualizarPedidoViewModel
    {
        public Cliente Cliente { get; set; }
        public List<VisualizarPedidoItemViewModel> Itens { get; set; } = new();
        public bool Pago { get; set; }
    }
}
