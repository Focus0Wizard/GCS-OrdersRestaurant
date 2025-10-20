using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Entities;

public partial class Producto
{
    public short Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "El nombre del producto no puede pasar de los 20 caracteres y no ser menos a 4.")]
    public string Nombre { get; set; } = null!;

    [Range(0.01, 1000, ErrorMessage = "El precio debe ser mayor a 0 y menor a 1000.")]
    public decimal Precio { get; set; }

    [Range(0, 150, ErrorMessage = "El stock no puede ser negativo y no mayor a 200.")]
    public int? Stock { get; set; }
    
    [Required(ErrorMessage = "La descripcion del producto es obligatoria.")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "La descripcion del producto no puede pasar de los 20 caracteres y no ser menos a 4.")]
    public string Descripcion { get; set; }
    [Range(1, short.MaxValue, ErrorMessage = "La Categoría debe ser un número positivo mayor a cero.")]
    public short CategoriaId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public sbyte? Estado { get; set; }

    public short? CreadoPor { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
