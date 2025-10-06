using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Pago
{
    public short Id { get; set; }

    public short PedidoId { get; set; }

    public string Metodo { get; set; } = null!;

    public string? EstadoPago { get; set; }

    public decimal Monto { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public sbyte? Estado { get; set; }

    public short? CreadoPor { get; set; }

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;
}
