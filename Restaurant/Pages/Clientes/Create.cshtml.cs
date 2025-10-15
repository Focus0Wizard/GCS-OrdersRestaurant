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

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Cliente.FechaCreacion = DateTime.Now;
            Cliente.UltimaActualizacion = DateTime.Now;
            Cliente.Estado = 1;
            Cliente.CreadoPor = 1;

            await _repo.AddAsync(Cliente);
            await _repo.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}