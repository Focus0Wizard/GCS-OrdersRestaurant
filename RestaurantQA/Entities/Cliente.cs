using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Entities;

public partial class Cliente
{
    public short Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ]+(?: [A-ZÁÉÍÓÚÑ][a-záéíóúñ]+)*$", 
        ErrorMessage = "El nombre solo puede contener letras y debe iniciar con mayúscula.")]
    [StringLength(30, MinimumLength = 3,  ErrorMessage = "El nombre no puede exceder 10 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ]+(?: [A-ZÁÉÍÓÚÑ][a-záéíóúñ]+)*$", 
        ErrorMessage = "El apellido solo puede contener letras y debe iniciar con mayúscula.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "El apellido no puede exceder 10 caracteres.")]
    public string Apellido { get; set; } = null!;

    [Phone(ErrorMessage = "El número de teléfono no es válido.")]
    [RegularExpression(@"^[67][0-9]{5,7}$", ErrorMessage = "El número de teléfono debe comenzar con 6 o 7 y tener un total de 7 a 8 dígitos.")]
    [StringLength(8, MinimumLength = 7, ErrorMessage = "El número de teléfono debe tener entre 7 y 8 dígitos.")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    public string? Correo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public sbyte? Estado { get; set; }

    public short? CreadoPor { get; set; }

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Ubicacione> Ubicaciones { get; set; } = new List<Ubicacione>();
}
