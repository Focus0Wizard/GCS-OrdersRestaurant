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

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty]
        public Cliente Cliente { get; set; } = new();

        public IEnumerable<Cliente> Clientes { get; set; } = new List<Cliente>();

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int page = 1)
        {
            PageNumber = page;
            var allClientes = string.IsNullOrEmpty(SearchTerm)
                ? (await _repo.GetAllAsync()).Where(c => c.Estado == 1)
                : (await _repo.FindAsync(c =>
                    (c.Nombre.Contains(SearchTerm!) ||
                     c.Apellido.Contains(SearchTerm!) ||
                     c.Correo.Contains(SearchTerm!)) &&
                     c.Estado == 1));

            var totalCount = allClientes.Count();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            Clientes = allClientes
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
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
