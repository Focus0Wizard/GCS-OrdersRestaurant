using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Cliente
{
    public short Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public sbyte? Estado { get; set; }

    public short? CreadoPor { get; set; }

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Ubicacione> Ubicaciones { get; set; } = new List<Ubicacione>();
}
