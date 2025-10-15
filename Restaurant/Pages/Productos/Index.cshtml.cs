using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Entities;
using Restaurant.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Restaurant.Pages.Productos
{
    public class ProductosModel : PageModel
    {
        private readonly IGenericRepository<Producto> _repo;

        public ProductosModel(IGenericRepository<Producto> repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new();

        public IEnumerable<Producto> Productos { get; set; } = new List<Producto>();

        // GET: Listar productos y cargar producto a editar si viene id
        public async Task OnGetAsync(short? id, string? searchTerm)
        {
            var productos = await _repo.GetAllAsync();

            // 🔍 Si hay texto de búsqueda, filtra
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                productos = productos.Where(p =>
                    p.Nombre.ToLower().Contains(searchTerm) ||
                    p.CategoriaId.ToString().Contains(searchTerm));
            }

            Productos = productos;

            // Si viene un id (editar)
            if (id.HasValue)
            {
                var producto = await _repo.GetByIdAsync(id.Value);
                if (producto != null)
                {
                    Producto = producto;
                }
            }
        }

        // POST: Crear o actualizar producto
        public async Task<IActionResult> OnPostAsync()
        {
            if (Producto == null)
                return Page();

            if (Producto.Id > 0)
            {
                Producto.UltimaActualizacion = DateTime.Now;
                _repo.Update(Producto);
                await _repo.SaveChangesAsync();
                return RedirectToPage();
            }
            else
            {
                Producto.CreadoPor = 1;
                Producto.FechaCreacion = DateTime.Now;
                Producto.UltimaActualizacion = DateTime.Now;
                Producto.Estado = 1;
                await _repo.AddAsync(Producto);
                await _repo.SaveChangesAsync();
                return RedirectToPage();
            }
        }

        // POST: Eliminar producto
        public async Task<IActionResult> OnPostEliminarAsync(short id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity != null)
            {
                _repo.Delete(entity);
                await _repo.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
