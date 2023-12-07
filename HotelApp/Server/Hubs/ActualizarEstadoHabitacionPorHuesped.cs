using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.SignalR;

namespace HotelApp.Server.Hubs
{
    public class ActualizarEstadoHabitacionPorHuesped : Hub
    {
        public async Task UpdateRoomStatus(HabitacionDTO HabitacionDTO)
        {
            await Clients.All.SendAsync("UpdateRoomStatus", HabitacionDTO);
        }
        public async Task PostRoomStatus(HabitacionDTO HabitacionDTO)
        {
            await Clients.All.SendAsync("PostRoomStatus", HabitacionDTO);
        }
    }
}