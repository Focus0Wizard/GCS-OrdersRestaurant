using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Categoria
{
    public short Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
