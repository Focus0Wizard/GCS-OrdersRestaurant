using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Entities;
using Restaurant.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Restaurant.Pages.Pedidos
{
    public class PedidosModel : PageModel
    {
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly IPedidoService _pedidoService;

        public PedidosModel(IClienteService clienteService,
                            IProductoService productoService,
                            IPedidoService pedidoService)
        {
            _clienteService = clienteService;
            _productoService = productoService;
            _pedidoService = pedidoService;
        }

        [BindProperty]
        public Pedido Pedido { get; set; } = new();

        [BindProperty]
        public List<int> Cantidades { get; set; } = new();

        [BindProperty]
        public List<short> ProductoIds { get; set; } = new();

        public IEnumerable<Cliente> Clientes { get; set; } = new List<Cliente>();
        public IEnumerable<Producto> Productos { get; set; } = new List<Producto>();
        public IEnumerable<Pedido> Pedidos { get; set; } = new List<Pedido>();

        public async Task OnGetAsync()
        {
            Clientes = await _clienteService.GetAllClientesAsync();
            Productos = await _productoService.GetAllProductosAsync();
            Pedidos = await _pedidoService.GetAllPedidosAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Clientes = await _clienteService.GetAllClientesAsync();
            Productos = await _productoService.GetAllProductosAsync();
            Pedidos = await _pedidoService.GetAllPedidosAsync();

            if (Pedido.ClienteId <= 0)
            {
                ModelState.AddModelError("", "Debe seleccionar un cliente.");
                return Page();
            }

            var detalles = new List<DetallePedido>();
            for (int i = 0; i < ProductoIds.Count; i++)
            {
                if (Cantidades[i] > 0)
                {
                    var producto = Productos.FirstOrDefault(p => p.Id == ProductoIds[i]);
                    if (producto != null)
                    {
                        detalles.Add(new DetallePedido
                        {
                            ProductoId = producto.Id,
                            Cantidad = Cantidades[i],
                            PrecioUnitario = producto.Precio,
                            Subtotal = producto.Precio * Cantidades[i]
                        });
                    }
                }
            }

            if (!detalles.Any())
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un producto.");
                return Page();
            }

            var cliente = await _clienteService.GetClienteByIdAsync(Pedido.ClienteId);
            if (cliente == null)
            {
                ModelState.AddModelError("", "Cliente no encontrado.");
                return Page();
            }

            Pedido.NombreCliente = cliente.Nombre;
            Pedido.ApellidoCliente = cliente.Apellido;
            Pedido.UsuarioId = 1; // TODO: asignar usuario logueado
            Pedido.EstadoPedido = 1; // Ejemplo: 1 = Creado

            await _pedidoService.AddPedidoAsync(Pedido, detalles);

            return RedirectToPage();
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

        public async Task<IActionResult> OnPostEliminarAsync(short id)
        {
            await _pedidoService.DeletePedidoAsync(id);
            return RedirectToPage();
        }

    }
}
