using BlazorCrud.Shared;
using HotelApp.Server.Hubs;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Reservar")]
    public class ReservaController : ControllerBase
    {
        private readonly Context context;
        private readonly IHubContext<ReservaHub> hubContext;
        public ReservaController(Context context, IHubContext<ReservaHub> HubContext)
        {
            this.context = context;
            this.hubContext = HubContext;
        }
       
        [HttpGet]
        public async Task<ActionResult<List<Reserva>>> Get()
        {
            return await context.Reservas.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Reserva>> Get(int id)
        {
            var buscar = await context.Reservas.FirstOrDefaultAsync(c => c.Id == id);

            if (buscar is null)
            {
                return BadRequest($"No se encontro la reserva de id: {id}");
            }

            return buscar;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ReservaDTO reservadto)
        {
            var responseapi = new ResponseAPI<int>();

            if (reservadto.Fecha_fin < reservadto.Fecha_inicio)
            {
                return BadRequest("Error de ingreso. La fecha de egreso no puede ser menor que la fecha de ingreso");
            }

            var entidad = await context.Reservas.FirstOrDefaultAsync(x => x.NroReserva == reservadto.NroReserva);

            if (entidad != null) // existe una hab con el num ingresado
            {
                return BadRequest($"Ya existe una reserva con el N°: {reservadto.NroReserva}");
            }

            try
            {
                var mdreserva = new Reserva
                {
                    NroReserva = reservadto.NroReserva,
                    Fecha_inicio = reservadto.Fecha_inicio,
                    Fecha_fin = reservadto.Fecha_fin,
                    Dni = reservadto.Dni,
                    DniHuesped = reservadto.Dns,
                    nhabs = reservadto.Nhabs,
                    EstadisponibleUoCupada = reservadto.EstaDisponibleuOCupada
                };

                context.Reservas.Add(mdreserva);
                await context.SaveChangesAsync();

                // Obtener la habitación correspondiente a la reserva
                var habitacion = await context.Habitaciones.FirstOrDefaultAsync(h => h.Nhab == reservadto.Nhabs);

                if (habitacion != null && reservadto.EstaDisponibleuOCupada)
                {
                    // Cambiar el estado de la habitación a "ocupada"
                    habitacion.Estado = "ocupada";

                    // Notificar cambio de estado a través de SignalR
                    await hubContext.Clients.All.SendAsync("CambioEstadoHabitacionPost", reservadto.Nhabs, "ocupada");
                }

                await context.SaveChangesAsync();

                return Ok(responseapi);
            }
            catch (Exception)
            {
                return BadRequest("No se pudo registrar la reserva");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Editar(ReservaDTO reservaDTO, int id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbReserva = await context.Reservas.FirstOrDefaultAsync(e => e.Id == id);

                if (dbReserva == null)
                {
                    return BadRequest($"No se encontró la reserva de N°: {reservaDTO.NroReserva} para ser actualizada");
                }

                // Guardar el estado anterior de la habitación
                bool estadoHabitacionAnterior = dbReserva.EstadisponibleUoCupada;

                dbReserva.NroReserva = reservaDTO.NroReserva;
                dbReserva.Fecha_inicio = reservaDTO.Fecha_inicio;
                dbReserva.Fecha_fin = reservaDTO.Fecha_fin;
                dbReserva.Dni = reservaDTO.Dni;
                dbReserva.DniHuesped = reservaDTO.Dns;
                dbReserva.nhabs = reservaDTO.Nhabs;

                // Actualizar el estado de la habitación en la reserva
                dbReserva.EstadisponibleUoCupada = reservaDTO.EstaDisponibleuOCupada;

                // Si el estado de la habitación cambió, notificar a través de SignalR
                if (estadoHabitacionAnterior != dbReserva.EstadisponibleUoCupada)
                {
                    // Cambiar el estado de la habitación en la base de datos
                    var habitacion = await context.Habitaciones.FirstOrDefaultAsync(h => h.Nhab == dbReserva.nhabs);

                    if (habitacion != null)
                    {
                        // Invertir el estado de la habitación
                        habitacion.Estado = dbReserva.EstadisponibleUoCupada ? "ocupada" : "disponible";

                        // Notificar cambio de estado a través de SignalR
                        await this.hubContext.Clients.All.SendAsync("CambioEstadoHabitacionPut", habitacion.Nhab, habitacion.Estado);
                    }
                }

                context.Reservas.Update(dbReserva);
                await context.SaveChangesAsync();

                responseApi.EsCorrecto = true;
                responseApi.Valor = dbReserva.NroReserva;
                return Ok(responseApi);
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(responseApi.Mensaje);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Borrar(int id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbReserva = await context.Reservas.FirstOrDefaultAsync(e => e.Id == id);

                if (dbReserva != null)
                {
                    context.Reservas.Remove(dbReserva);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Reserva no encontrada";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.InnerException.Message;
            }
            return Ok(responseApi);
        }

        [HttpGet("FechasYEstadoSeleccionados")]
        public IQueryable<Reserva> ObtenerFechasYEstadoSeleccionados(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio == DateTime.MinValue || fechaFin == DateTime.MinValue)
            {
                BadRequest("Las fechas de inicio y fin son obligatorias");
                return Enumerable.Empty<Reserva>().AsQueryable();
            }

            var reservasFiltradas = context.Habitaciones
                .Where(h => h.Estado == "disponible" || h.Estado == "ocupada")
                .Join(context.Reservas,
                      habitacion => habitacion.Nhab,
                      reserva => reserva.nhabs,
                      (habitacion, reserva) =>
                      new { Habitacion = habitacion, Reserva = reserva })
                .Where(r => r.Reserva.Fecha_inicio.Date >= fechaInicio.Date && r.Reserva.Fecha_fin.Date <= fechaFin.Date)
                .Select(r => r.Reserva);

            if (reservasFiltradas is null)
            {
                 BadRequest("No se filtrando una mierda");
            }

            return reservasFiltradas;
        }
    }
}