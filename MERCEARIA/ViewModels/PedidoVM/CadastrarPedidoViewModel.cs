using MERCEARIA.Models;

namespace MERCEARIA.ViewModels
{
    public class CadastrarPedidoViewModel
    {
        public long IdCliente{ get; set; }
        public List<CadastrarPedidoItemViewModel> Itens { get; set; }
        public bool Pago { get; set; }
    }
}
