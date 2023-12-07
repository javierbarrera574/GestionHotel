using Microsoft.AspNetCore.SignalR;

namespace HotelApp.Server.Hubs
{
    public class ReservaHub : Hub
    {
       
        public async Task ReservaPostCambioEstado(string numeroHabitacion, string nuevoEstado)
        {
            await Clients.All.SendAsync("CambioEstadoHabitacion", numeroHabitacion, nuevoEstado);
        }
        public async Task ReservaPutCambioEstado(string numeroHabitacion, string nuevoEstado)
        {
            await Clients.All.SendAsync("CambioEstadoHabitacionPut", numeroHabitacion, nuevoEstado);
        }
    }
}