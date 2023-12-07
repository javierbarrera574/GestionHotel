using BlazorCrud.Shared;
using HotelApp.Server.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Huesped")]
    public class HuespedController : ControllerBase
    {
        private readonly Context context;
        private readonly IHubContext<ActualizarEstadoHabitacionPorHuesped> hubContext1;

       
        public HuespedController(Context context,
            IHubContext<ActualizarEstadoHabitacionPorHuesped> hubContext)
        {
            this.context = context;
            this.hubContext1 = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Huesped>>> Get()
        {
            return await context.Huespedes.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Huesped>> Get(int id)
        {
            var buscar = await context.Huespedes.FirstOrDefaultAsync(c => c.Id == id);

            if (buscar == null)
            {
                return BadRequest($"No se encontro el huesped de dni numero: {id}");
            }

            return buscar;
        }

        [HttpGet("GetDni/{dni:int}")]
        public async Task<ActionResult<Huesped>> GetDni(int dni)
        {
            var buscar = await context.Huespedes.FirstOrDefaultAsync(c => c.Dni == dni);

            if (buscar == null)
            {
                return BadRequest($"No se encontro el huesped de dni numero: {dni}");
            }

            return buscar;
        
        }  

        [HttpDelete("{id:int}")]
        public async Task <IActionResult> Borrar(int Id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbHuesped = await context.Huespedes.FirstOrDefaultAsync(e => e.Id == Id);
                if (dbHuesped != null)
                {
                    context.Huespedes.Remove(dbHuesped);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                } else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Huesped no encontrado";
                }
            }catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
                return BadRequest("No se pudo eliminar la persona");
            }
            return Ok(responseApi);
        }
        [HttpPut("PutCheckIn/{numeroHabitacion:int}")]
        public async Task<IActionResult> PutCheckIn(bool checkIn, int numeroHabitacion)
        {
            try
            {
                // Obtener la habitación correspondiente al número de habitación
                var habitacion = await context.Habitaciones.FirstOrDefaultAsync(h => h.Nhab == numeroHabitacion);

                if (habitacion == null)
                {
                    return BadRequest($"No se encontró la habitación con el número: {numeroHabitacion}");
                }

                // Obtener el huésped asociado a la habitación
                var huesped = await context.Huespedes
                    .FirstOrDefaultAsync(h => h.HabitacionNumero == numeroHabitacion);

                if (huesped == null)
                {
                    return BadRequest($"No se encontró un huésped asociado a la habitación {numeroHabitacion}");
                }

                // Verificar el estado actual de la habitación y del check-in
                bool estadoActualHabitacionOcupada = habitacion.Estado == "ocupada";
                bool checkInActual = huesped.Checkin;

                // Verificar si hay cambios y actualizar en consecuencia
                if (checkInActual != checkIn)
                {
                    // Cambiar el estado de la habitación según el check-in
                    habitacion.Estado = checkIn ? "ocupada" : "disponible";

                    // Cambiar el estado de check-in del huésped
                    huesped.Checkin = checkIn;

                    // Guardar los cambios en la base de datos
                    await context.SaveChangesAsync();

                    // Notificar cambio de estado de la habitación a través de SignalR
                    await hubContext1.Clients.All.SendAsync("CambioEstadoHabitacionCheckIn", habitacion.Nhab, habitacion.Estado);
                }
                
                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest($"Error al aplicar el Check-In");
            }
        }


        [HttpPut("PutFuncional2/{id:int}")]
        public async Task<IActionResult> PutFuncional2(HuespedDTO HuespedDTO, int Id)
        {
            var responseApi = new ResponseAPI<int>();
            try
            {
                var dbHuesped = await context.Huespedes.FirstOrDefaultAsync(e => e.Id == Id);
                if (dbHuesped != null)
                {
                    // Guardar el número de habitación antes de la modificación
                    var numeroHabitacionAnterior = dbHuesped.HabitacionNumero;

                    dbHuesped.Nombres = HuespedDTO.Nombres;
                    dbHuesped.Apellidos = HuespedDTO.Apellidos;
                    dbHuesped.Checkin = HuespedDTO.Checkin;
                    dbHuesped.DniPersona = HuespedDTO.DniPersona;
                    dbHuesped.HabitacionNumero = HuespedDTO.Habitacionnumero;
                    context.Huespedes.Update(dbHuesped);
                    await context.SaveChangesAsync();

                    // Obtener la habitación correspondiente al número de habitación anterior
                    var habitacionAnterior = await context.Habitaciones.FirstOrDefaultAsync(h => h.Nhab == numeroHabitacionAnterior);

                    // Obtener la habitación correspondiente al nuevo número de habitación
                    var habitacionNueva = await context.Habitaciones.FirstOrDefaultAsync(h => h.Nhab == HuespedDTO.Habitacionnumero);

                    if (habitacionAnterior != null && habitacionNueva != null)
                    {
                        // Cambiar el estado de la habitación anterior y nueva según el checkin
                        if (HuespedDTO.Checkin.Equals(true))
                        {
                            habitacionAnterior.Estado = "disponible";
                            habitacionNueva.Estado = "ocupada";
                        }
                        else if(HuespedDTO.Checkin.Equals(false))
                        {
                            habitacionAnterior.Estado = "ocupada";
                            habitacionNueva.Estado = "disponible";
                        }

                        await context.SaveChangesAsync();

                        // Emitir eventos de actualización
                        await hubContext1.Clients.All.SendAsync("UpdateRoomStatus", habitacionAnterior);
                        await hubContext1.Clients.All.SendAsync("UpdateRoomStatus", habitacionNueva);
                    }

                    responseApi.EsCorrecto = true;
                    responseApi.Mensaje = "Se modificó el huésped de DNI: " + dbHuesped.Dni;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "DNI no encontrado";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }


        [HttpPost("PostFuncional2")]
        public async Task<IActionResult> PostFuncional2(HuespedDTO HuespedDTO)
        {

            var personaExistente = await 
                context.Personas
                .FirstOrDefaultAsync(p => p.Dni == HuespedDTO.Dni && p.EsHuespedyReservante);

            if (personaExistente != null)
            {
                return BadRequest("Esta persona ya ha sido cargada como huésped y reservante al mismo tiempo en la lista de Personas");
            }

            var entidad = await context.Huespedes.FirstOrDefaultAsync(x => x.Dni == HuespedDTO.Dni);

            if (entidad != null) // existe un Huesped con el mismo DNI
            {
                return BadRequest($"Ya existe un húesped con el DNI N°: {HuespedDTO.Dni}");
            }

            try
            {
                var mdHuesped = new Huesped
                {
                    Dni = HuespedDTO.Dni,
                    Nombres = HuespedDTO.Nombres,
                    Apellidos = HuespedDTO.Apellidos,
                    Checkin = HuespedDTO.Checkin,
                    DniPersona = HuespedDTO.DniPersona,
                    HabitacionNumero = HuespedDTO.Habitacionnumero
                };

                context.Huespedes.Add(mdHuesped);
                await context.SaveChangesAsync();

                // Obtener la habitación correspondiente al check-in
                var habitacion = await context.Habitaciones.FirstOrDefaultAsync(h => h.Nhab == HuespedDTO.Habitacionnumero);

                if (HuespedDTO.Checkin && habitacion != null && habitacion.Estado == "disponible")
                {
                    // Cambiar el estado de la habitación a "ocupada"
                    habitacion.Estado = "ocupada";

                    await context.SaveChangesAsync();

                    // Emitir evento de actualización
                    await hubContext1.Clients.All.SendAsync("PostRoomStatus", habitacion);
                }
                else if (!HuespedDTO.Checkin && habitacion == null && habitacion.Estado == "ocupada")
                {
                    // Cambiar el estado de la habitación a "disponible"
                    habitacion.Estado = "disponible";

                    await context.SaveChangesAsync();

                    // Emitir evento de actualización
                    await hubContext1.Clients.All.SendAsync("PostRoomStatus", habitacion);
                }            

                const string EstadoReparacion = "reparacion";

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var habitacionEnReparacion = await context.Habitaciones
                            .AnyAsync(h => h.Nhab == HuespedDTO.Habitacionnumero && h.Estado == EstadoReparacion);

                        if (habitacionEnReparacion)
                        {
                            return BadRequest("La habitación seleccionada está en reparación");
                        }

                        // Agregar la lógica para agregar la habitación aquí

                        // Commit de la transacción si todo está bien
                        transaction.Commit();

                        // Resto del código...
                    }
                    catch (Exception ex)
                    {
                        // Manejar excepciones aquí
                        transaction.Rollback();
                        return BadRequest($"Error: {ex.Message}");
                    }
                }


                return Ok();
            }
            catch (Exception ex)
            {               
                return BadRequest(ex.Message);
            }
        }


    }
}