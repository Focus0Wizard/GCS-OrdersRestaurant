using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Entities;
using Restaurant.Repositories;

namespace Restaurant.Pages.Repartidores
{
    public class RepartidoresModel : PageModel
    {
        private readonly IGenericRepository<Repartidore> _repo;

        public RepartidoresModel(IGenericRepository<Repartidore> repo)
        {
            _repo = repo;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty]
        public Repartidore Repartidor { get; set; } = new();

        public IEnumerable<Repartidore> Repartidores { get; set; } = new List<Repartidore>();

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int page = 1)
        {
            PageNumber = page;
            var allRepartidores = string.IsNullOrEmpty(SearchTerm)
                ? (await _repo.GetAllAsync()).Where(r => r.Estado == 1)
                : (await _repo.FindAsync(r =>
                    (r.Nombre.Contains(SearchTerm!) ||
                     r.Apellido.Contains(SearchTerm!) ||
                     (r.Telefono ?? "").Contains(SearchTerm!)) &&
                    r.Estado == 1));

            var totalCount = allRepartidores.Count();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            Repartidores = allRepartidores
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
            return RedirectToPage("./Index");
        }
    }
}