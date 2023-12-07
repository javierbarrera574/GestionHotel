using Microsoft.AspNetCore.SignalR;

namespace HotelApp.Server.Hubs
{
    public class HabitacionHub : Hub
    {
        // Puedes agregar métodos adicionales según tus necesidades
        public async Task NotificarCambioEstadoHabitacion(int numeroHabitacion, string nuevoEstado)
        {
            await Clients.All.SendAsync("CambioEstadoHabitacion", numeroHabitacion, nuevoEstado);
        }

        public async Task NotificarCambioEstadoHabitacionCheckIn(int numeroHabitacion, string nuevoEstado)
        {
            await Clients.All.SendAsync("CambioEstadoHabitacionCheckIn", numeroHabitacion, nuevoEstado);
        }
    }
}
