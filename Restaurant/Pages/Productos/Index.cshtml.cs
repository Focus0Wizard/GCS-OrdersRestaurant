using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Entities;
using Restaurant.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
        public async Task OnGetAsync(short? id)
        {
            Productos = await _repo.GetAllAsync();

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
                // Actualizar
                Producto.UltimaActualizacion = DateTime.Now;
                _repo.Update(Producto);
                await _repo.SaveChangesAsync();

                // Mantener el formulario cargado con datos actualizados
                return RedirectToPage();
            }
            else
            {
                // Crear
                Producto.CreadoPor = 1;
                Producto.FechaCreacion = DateTime.Now;
                Producto.UltimaActualizacion = DateTime.Now;
                Producto.Estado = 1;
                await _repo.AddAsync(Producto);
                await _repo.SaveChangesAsync();

                // Volver a formulario vacío después de guardar
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
