using MERCEARIA.Data;
using MERCEARIA.Models;
using MERCEARIA.ViewModels;

namespace MERCEARIA.Service
{
    public class ClienteService
    {
        private static MerceariaDataContext _context;
        public ClienteService(MerceariaDataContext context)
        {
            _context = context;
        }
        public List<ClienteViewModel> VisualizarCLientes(int id)
        {
            try
            {
                var vm = new List<ClienteViewModel>();
                var cliente = _context.Clientes.ToList();
                foreach (var i in cliente)
                {
                    var item = new ClienteViewModel()
                    {
                        NomeCliente = i.NomeCliente,
                        Email = i.Email,
                        DataNascimento = i.DataNascimento,
                        Ativo = i.Ativo,
                    };
                    vm.Add(item);
                }
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ClienteViewModel VisualizarCLienteById(int id)
        {
            try
            {
               var cliente = _context.Clientes.FirstOrDefault(x => x.Id == id);
                var vm = new ClienteViewModel()
                {
                    NomeCliente = cliente.NomeCliente,
                    Email = cliente.Email,
                    DataNascimento = cliente.DataNascimento,
                    Ativo = cliente.Ativo,
                };
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int AdicionarCLiente(Cliente cliente)
        {
            try
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return cliente.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int EditarCLiente(Cliente clienteEditado, int id)
        {
            try
            {
                var cliente = _context.Clientes.FirstOrDefault(x => x.Id == id);
                if (cliente == null)
                    return 1;

                cliente.NomeCliente = clienteEditado.NomeCliente;
                cliente.Email = clienteEditado.Email;
                cliente.DataNascimento = clienteEditado.DataNascimento;
                cliente.Ativo = clienteEditado.Ativo;

                _context.Clientes.Update(cliente);
                _context.SaveChanges();
                return cliente.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Deletarcliente(int id)
        {
            try
            {
                var cliente = _context.Clientes.FirstOrDefault(x => x.Id == id);
                if (cliente == null)
                    return "Cliente não localizado";

                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
                return "Cliente deletado com sucesso";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
