using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbbouClima.Models
{
    public class Cliente
    {
        [Key] // Llave primaria
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        [Display(Name = "Nombre Completo")]
        public string? Nombre { get; set; }

        [Display(Name = "NIF")]
        public string? NIF { get; set; }

		//[Required(ErrorMessage = "El email es obligatorio")]
		[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",ErrorMessage = "El formato del correo electrónico no es válido.")]
		[Display(Name = "Correo Electrónico")]
        public string? Correo { get; set; }

		[RegularExpression(@"^\+?[0-9]{7,15}$",ErrorMessage = "El número de teléfono no es válido.")]
		[Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        public bool Borrado { get; set; }
        [Display(Name = "Fecha de Registro")]
        public string? FechaRegistro { get; set; }
        public string? FechaModificacion { get; set; }
        public List<Presupuesto>? Presupuestos { get; set; }
    }
}
