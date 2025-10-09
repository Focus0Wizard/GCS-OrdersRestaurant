using Microsoft.AspNetCore.Mvc;
using Restaurant.Entities;
using Restaurant.Services;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _productoService.GetAllProductosAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            await _productoService.AddProductoAsync(producto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(short id, Producto producto)
        {
            if (id != producto.Id) return BadRequest();
            await _productoService.UpdateProductoAsync(producto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            await _productoService.DeleteProductoAsync(id);
            return NoContent();
        }
    }
}