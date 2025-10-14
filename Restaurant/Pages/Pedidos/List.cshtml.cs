using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Entities;
using Restaurant.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Pages.Pedidos
{
    public class ListModel : PageModel
    {
        private readonly IPedidoService _pedidoService;

        public ListModel(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        public IEnumerable<Pedido> Pedidos { get; set; } = new List<Pedido>();

        public async Task OnGetAsync()
        {
            Pedidos = await _pedidoService.GetAllPedidosAsync();
        }

        public async Task<IActionResult> OnPostRecepcionadoAsync(short id)
        {
            await _pedidoService.UpdateEstadoPedidoAsync(id, 0);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCancelarAsync(short id)
        {
            await _pedidoService.UpdateEstadoPedidoAsync(id, -1);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEnviadoAsync(short id)
        {
            await _pedidoService.UpdateEstadoPedidoAsync(id, 1);
            return RedirectToPage();
        }
    }
}