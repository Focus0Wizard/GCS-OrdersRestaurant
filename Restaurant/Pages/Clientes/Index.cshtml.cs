using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Entities;
using Restaurant.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Pages.Clientes
{
    public class ClientesModel : PageModel
    {
        private readonly IGenericRepository<Cliente> _repo;

        public ClientesModel(IGenericRepository<Cliente> repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public Cliente Cliente { get; set; } = new();

        public IEnumerable<Cliente> Clientes { get; set; } = new List<Cliente>();

        public async Task<IActionResult> OnPostAsync()
        {
            if (Cliente == null)
                return Page();

            if (Cliente.Id > 0)
            {
                var dbCliente = await _repo.GetByIdAsync(Cliente.Id);
                if (dbCliente != null)
                {
                    dbCliente.Nombre = Cliente.Nombre;
                    dbCliente.Apellido = Cliente.Apellido;
                    dbCliente.Telefono = Cliente.Telefono;
                    dbCliente.Correo = Cliente.Correo;
                    dbCliente.UltimaActualizacion = DateTime.Now;

                    _repo.Update(dbCliente);
                    await _repo.SaveChangesAsync();
                }

                return RedirectToPage();
            }

            else
            {
                Cliente.FechaCreacion = DateTime.Now;
                Cliente.UltimaActualizacion = DateTime.Now;
                Cliente.Estado = 1;
                Cliente.CreadoPor = 1; 

                await _repo.AddAsync(Cliente);
                await _repo.SaveChangesAsync();

                return RedirectToPage();
            }
        }

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
        
        public async Task OnGetAsync(short? id, string? searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                Clientes = await _repo.FindAsync(c =>
                    c.Nombre.Contains(searchTerm) ||
                    c.Apellido.Contains(searchTerm) ||
                    c.Correo.Contains(searchTerm));
            }
            else
            {
                Clientes = await _repo.GetAllAsync();
            }
            
            if (id.HasValue)
            {
                var cliente = await _repo.GetByIdAsync(id.Value);
                if (cliente != null)
                {
                    Cliente = cliente;
                }
            }
        }

        
    }
}
