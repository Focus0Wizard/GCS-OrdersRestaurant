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

        public async Task OnGetAsync(short? id, string? searchTerm)
        {
            var productos = await _repo.GetAllAsync();

            productos = productos.Where(p => p.Estado == 1);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                productos = productos.Where(p =>
                    p.Nombre.ToLower().Contains(searchTerm) ||
                    p.CategoriaId.ToString().Contains(searchTerm));
            }

            Productos = productos;
            if (id.HasValue)
            {
                var producto = await _repo.GetByIdAsync(id.Value);
                if (producto != null)
                {
                    Producto = producto;
                }
            }
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Producto == null)
                return Page();

            if (Producto.Id > 0)
            {
                var productoExistente = await _repo.GetByIdAsync(Producto.Id);
                if (productoExistente == null)
                    return NotFound();

                productoExistente.Nombre = Producto.Nombre;
                productoExistente.Precio = Producto.Precio;
                productoExistente.Stock = Producto.Stock;
                productoExistente.Descripcion = Producto.Descripcion;
                productoExistente.CategoriaId = Producto.CategoriaId;

                productoExistente.UltimaActualizacion = DateTime.Now;

                await _repo.SaveChangesAsync();
            }
            else
            {
                Producto.CreadoPor = 1;
                Producto.FechaCreacion = DateTime.Now;
                Producto.UltimaActualizacion = DateTime.Now;
                Producto.Estado = 1;

                await _repo.AddAsync(Producto);
                await _repo.SaveChangesAsync();
            }

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostEliminarAsync(short id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity != null)
            {
                await _repo.SoftDeleteAsync(entity);
            }
            return RedirectToPage();
        }
    }
}
