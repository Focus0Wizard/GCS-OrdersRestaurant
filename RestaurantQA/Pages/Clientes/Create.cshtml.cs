using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Entities;
using Restaurant.Repositories;

namespace Restaurant.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        private readonly IGenericRepository<Cliente> _repo;

        public CreateModel(IGenericRepository<Cliente> repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public Cliente Cliente { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id.HasValue)
            {
                var clienteExistente = await _repo.GetByIdAsync(id.Value);
                if (clienteExistente == null)
                    return RedirectToPage("./Index");

                Cliente = clienteExistente;
                if (ViewData != null)
                {
                    ViewData["Title"] = "Editar Cliente";
                }
            }
            else
            {
                if (ViewData != null)
                {
                    ViewData["Title"] = "Registrar Cliente";
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Cliente.Id > 0)
            {
                var clienteExistente = await _repo.GetByIdAsync(Cliente.Id);
                if (clienteExistente == null)
                    return RedirectToPage("./Index");

                clienteExistente.Nombre = Cliente.Nombre;
                clienteExistente.Apellido = Cliente.Apellido;
                clienteExistente.Telefono = Cliente.Telefono;
                clienteExistente.Correo = Cliente.Correo;

                clienteExistente.UltimaActualizacion = DateTime.Now;

                _repo.Update(clienteExistente);
            }
            else
            {
                Cliente.FechaCreacion = DateTime.Now;
                Cliente.UltimaActualizacion = DateTime.Now;
                Cliente.Estado = 1;
                Cliente.CreadoPor = 1;

                await _repo.AddAsync(Cliente);
            }

            await _repo.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
