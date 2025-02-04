using AbbouClima.Models;

namespace AbbouClima.Interfaces
{
    public interface IClienteService
    {
        Task<List<Cliente>> GetAll(string busqueda);
        Task<Cliente> DetailsById(int? id);
        Task CreateCliente(Cliente cliente);
        Task EditClient(Cliente cliente);
        Task DeleteClient(Cliente cliente);
        bool ClienteExists(int id);
    }
}
