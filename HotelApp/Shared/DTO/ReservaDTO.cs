using System.ComponentModel.DataAnnotations;

namespace HotelApp.Shared.DTO
{
    public class ReservaDTO
	{
		public int NroReserva { get; set; }
		[Required(ErrorMessage = "La Fecha de inicio es Obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_inicio { get; set; }
		[Required(ErrorMessage = "La Fecha de fin es Obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_fin { get; set; }
		[Required(ErrorMessage = "El Dni del dueño de la reserva es obligatorio")]
		public int Dni { get; set; }
		[Required(ErrorMessage = "El Dni de los huespedes es obligatorio")]
		public List<int> Dns { get; set; } = new List<int>();	
		[Required(ErrorMessage = "se requiere la lista de habitaciones")]
		public int Nhabs { get; set; }
        [Required(ErrorMessage = "Se requiere el estado de la habitacion reservada")]
        public bool EstaDisponibleuOCupada { get; set; }

    }
}
