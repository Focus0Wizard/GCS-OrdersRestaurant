using Restaurant.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetAllProductosAsync();
        Task<Producto?> GetProductoByIdAsync(short id);
        Task AddProductoAsync(Producto producto);
        Task UpdateProductoAsync(Producto producto);
        Task DeleteProductoAsync(short id);
    }
}