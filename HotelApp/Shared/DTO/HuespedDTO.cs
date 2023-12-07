using System.ComponentModel.DataAnnotations;

namespace Reservas.BData.Data.Entity
{
    public class HuespedDTO
    {
        [Required(ErrorMessage = "El DNI es Obligatorio")]
        public int Dni { get; set; }

        [Required(ErrorMessage = "El Nombre es Obligatorio")]
        [MaxLength(50, ErrorMessage = "Solo se aceptan hasta 50 caracteres en el Nombre")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El Apellido es Obligatorio")]
        [MaxLength(50, ErrorMessage = "Solo se aceptan hasta 50 caracteres en el Apellido")]
        public string Apellidos { get; set; }

        public bool Checkin { get; set; }

        [Required(ErrorMessage = "El DniPersona es Obligatorio")]
        public int DniPersona { get; set; }
        [Required(ErrorMessage ="El número de habitación del huesped es obligatorio")]
        public int Habitacionnumero { get; set; }
    }
}
