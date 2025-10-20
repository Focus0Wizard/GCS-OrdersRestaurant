using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Pedido
{
    public short Id { get; set; }

    public short ClienteId { get; set; }

    public short UsuarioId { get; set; }

    public short? RiderId { get; set; }

    public sbyte? EstadoPedido { get; set; }

    public string? NombreCliente { get; set; }

    public string? ApellidoCliente { get; set; }

    public decimal Total { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public short? CreadoPor { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual ICollection<DetallePedido>? DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Repartidore? Rider { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
