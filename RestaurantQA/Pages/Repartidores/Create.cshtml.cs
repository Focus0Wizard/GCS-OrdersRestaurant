using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Entities;
using Restaurant.Repositories;

namespace Restaurant.Pages.Repartidores
{
    public class CreateModel : PageModel
    {
        private readonly IGenericRepository<Repartidore> _repo;

        public CreateModel(IGenericRepository<Repartidore> repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public Repartidore Repartidor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id.HasValue)
            {
                var repartidorExistente = await _repo.GetByIdAsync(id.Value);
                if (repartidorExistente == null)
                    return RedirectToPage("./Index");

                Repartidor = repartidorExistente;
                ViewData["Title"] = "Editar Repartidor";
            }
            else
            {
                ViewData["Title"] = "Registrar Repartidor";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Repartidor.Id > 0)
            {
                var repartidorExistente = await _repo.GetByIdAsync(Repartidor.Id);
                if (repartidorExistente == null)
                    return RedirectToPage("./Index");

                repartidorExistente.Nombre = Repartidor.Nombre;
                repartidorExistente.Apellido = Repartidor.Apellido;
                repartidorExistente.Telefono = Repartidor.Telefono;
                repartidorExistente.Tipo = Repartidor.Tipo;
                repartidorExistente.UltimaActualizacion = DateTime.Now;

                _repo.Update(repartidorExistente);
            }
            else
            {
                Repartidor.FechaCreacion = DateTime.Now;
                Repartidor.UltimaActualizacion = DateTime.Now;
                Repartidor.Estado = 1;
                Repartidor.CreadoPor = 1;

                await _repo.AddAsync(Repartidor);
            }

            await _repo.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
