using System.ComponentModel.DataAnnotations;

namespace Reservas.BData.Data.Entity
{
    public class Persona
	{
		public int Id { get; set; }
		[Required]
        public int Dni { get; set; }
		[Required(ErrorMessage = "El Nombre es Obligatorio")]
		[MaxLength(55, ErrorMessage = "Solo se aceptan hasta 55 caracteres en el Nombre")]
		public string Nombres { get; set; }
		[Required(ErrorMessage = "El Apellido es Obligatorio")]
		[MaxLength(55, ErrorMessage = "Solo se aceptan hasta 55 caracteres en el Nombre")]
		public string Apellidos { get; set; }
		[Required(ErrorMessage = "El Correo es Obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Correo { get; set; }
		[Required(ErrorMessage = "El Telefono es Obligatorio")]
		[MaxLength(20, ErrorMessage = "Solo se aceptan hasta 20 caracteres en el Nombre")]
		public string Telefono { get; set; }
		[Required(ErrorMessage = "El Numero de tarjeta es Obligatorio")]
		[MaxLength(20, ErrorMessage = "Solo se aceptan hasta 20 caracteres en el Nombre")]
		public string NumTarjeta { get; set; }
		[Required]
        public int NumHab { get; set; }
		[Required]
        public bool EsHuespedyReservante { get; set; }
        public Huesped Huesped { get; set; }
    }
}