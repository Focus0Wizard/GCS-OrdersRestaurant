using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Ubicacione
{
    public short Id { get; set; }

    public short ClienteId { get; set; }

    public string? Direccion { get; set; }

    public string? Referencia { get; set; }

    public string? Ciudad { get; set; }

    public double? Latitud { get; set; }

    public double? Longitud { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;
}
