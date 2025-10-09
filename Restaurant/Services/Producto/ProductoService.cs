using Restaurant.Entities;
using Restaurant.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepository;

        public ProductoService(IGenericRepository<Producto> productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            return await _productoRepository.GetAllAsync();
        }

        public async Task<Producto?> GetProductoByIdAsync(short id)
        {
            return await _productoRepository.GetByIdAsync(id);
        }

        public async Task AddProductoAsync(Producto producto)
        {
            await _productoRepository.AddAsync(producto);
            await _productoRepository.SaveChangesAsync();
        }

        public async Task UpdateProductoAsync(Producto producto)
        {
            _productoRepository.Update(producto);
            await _productoRepository.SaveChangesAsync();
        }

        public async Task DeleteProductoAsync(short id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto != null)
            {
                _productoRepository.Delete(producto);
                await _productoRepository.SaveChangesAsync();
            }
        }
    }
}