using Restaurant.Entities;

namespace Restaurant.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientesAsync();
        Task<Cliente?> GetClienteByIdAsync(short id);
        Task AddClienteAsync(Cliente producto);
        Task UpdateClienteAsync(Cliente producto);
        Task DeleteClienteAsync(short id);
    }
}