using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Producto
{
    public short Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int? Stock { get; set; }

    public short CategoriaId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public sbyte? Estado { get; set; }

    public short? CreadoPor { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
