using Restaurant.Entities;
using Restaurant.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Restaurant.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IGenericRepository<Pedido> _pedidoRepository;
        private readonly IGenericRepository<DetallePedido> _detalleRepository;

        public PedidoService(IGenericRepository<Pedido> pedidoRepository,
                             IGenericRepository<DetallePedido> detalleRepository)
        {
            _pedidoRepository = pedidoRepository;
            _detalleRepository = detalleRepository;
        }

        public async Task<IEnumerable<Pedido>> GetAllPedidosAsync()
        {
            return await _pedidoRepository.GetAllAsync();
        }

        public async Task<Pedido?> GetPedidoByIdAsync(short id)
        {
            return await _pedidoRepository.GetByIdAsync(id);
        }

        public async Task<Pedido> AddPedidoAsync(Pedido pedido, List<DetallePedido> detalles)
        {
            if (detalles == null || detalles.Count == 0)
            {
                throw new ArgumentException("El pedido debe contener al menos un detalle");
            }

            pedido.FechaCreacion = DateTime.Now;
            pedido.UltimaActualizacion = DateTime.Now;
            pedido.Total = detalles.Sum(d => d.Subtotal);

            await _pedidoRepository.AddAsync(pedido);
            await _pedidoRepository.SaveChangesAsync();

            foreach (var detalle in detalles)
            {
                detalle.PedidoId = pedido.Id;
                await _detalleRepository.AddAsync(detalle);

                // 🔹 Actualizar stock
                if (detalle.Producto != null)
                {
                    detalle.Producto.Stock -= detalle.Cantidad;
                }
            }

            await _detalleRepository.SaveChangesAsync();
            return pedido;
        }


        public async Task UpdatePedidoAsync(Pedido pedido)
        {
            pedido.UltimaActualizacion = DateTime.Now;
            _pedidoRepository.Update(pedido);
            await _pedidoRepository.SaveChangesAsync();
        }

        public async Task DeletePedidoAsync(short id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido != null)
            {
                _pedidoRepository.Delete(pedido);
                await _pedidoRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateEstadoPedidoAsync(short id, sbyte nuevoEstado)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido != null)
            {
                pedido.EstadoPedido = nuevoEstado;
                pedido.UltimaActualizacion = DateTime.Now;
                _pedidoRepository.Update(pedido);
                await _pedidoRepository.SaveChangesAsync();
            }
        }
    }
}
