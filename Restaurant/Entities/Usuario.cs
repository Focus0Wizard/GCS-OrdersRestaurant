using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Usuario
{
    public short Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Rol { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public sbyte? Estado { get; set; }

    public short? CreadoPor { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual ICollection<Usuario> InverseCreadoPorNavigation { get; set; } = new List<Usuario>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Pedido> PedidoCreadoPorNavigations { get; set; } = new List<Pedido>();

    public virtual ICollection<Pedido> PedidoUsuarios { get; set; } = new List<Pedido>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Repartidore> Repartidores { get; set; } = new List<Repartidore>();
}
