using Restaurant.Entities;

namespace Restaurant.Services
{
    public interface IPedidoService
    {
        Task<IEnumerable<Pedido>> GetAllPedidosAsync();
        Task<Pedido?> GetPedidoByIdAsync(short id);
        Task<Pedido> AddPedidoAsync(Pedido pedido, List<DetallePedido> detalles);
        Task UpdatePedidoAsync(Pedido pedido);
        Task DeletePedidoAsync(short id);
        Task UpdateEstadoPedidoAsync(short id, sbyte nuevoEstado);
    }
}