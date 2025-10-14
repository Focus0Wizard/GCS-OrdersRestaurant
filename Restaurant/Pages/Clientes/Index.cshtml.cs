using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Entities;
using Restaurant.Repositories;

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

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task OnGetAsync(short? id, string? searchTerm, int? page)
        {
            var allClientes = string.IsNullOrEmpty(searchTerm)
                ? await _repo.GetAllAsync()
                : await _repo.FindAsync(c =>
                    c.Nombre.Contains(searchTerm) ||
                    c.Apellido.Contains(searchTerm) ||
                    c.Correo.Contains(searchTerm));

            var totalCount = allClientes.Count();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            PageNumber = page.HasValue && page.Value > 0 ? page.Value : 1;

            Clientes = allClientes
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            if (id.HasValue)
            {
                var cliente = await _repo.GetByIdAsync(id.Value);
                if (cliente != null)
                {
                    Cliente = cliente;
                }
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
    }
}
