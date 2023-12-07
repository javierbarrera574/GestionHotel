using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Reservas.BData.Data.Entity
{
    public class Huesped
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El DNI es Obligatorio")]
        public int Dni { get; set; }
		[Required(ErrorMessage = "El Nombre es Obligatorio")]
		[MaxLength(50, ErrorMessage = "Solo se aceptan hasta 50 caracteres en el Nombre")]
		public string Nombres { get; set; }
		[Required(ErrorMessage = "El Apellido es Obligatorio")]
		[MaxLength(50, ErrorMessage = "Solo se aceptan hasta 50 caracteres en el Apellido")]
		public string Apellidos { get; set; }
		public bool Checkin { get; set; }
		[Required]
        public int DniPersona { get; set; }
		[Required]
        public int HabitacionNumero { get; set; }
		[JsonIgnore]
        public int? HabitacionId { get; set; }
        [JsonIgnore]
        public Habitacion Habitacion { get; set; }
		[JsonIgnore]
        public int? PersonaId { get; set; }
		[JsonIgnore]
        public Persona Persona { get; set; }
    }
}