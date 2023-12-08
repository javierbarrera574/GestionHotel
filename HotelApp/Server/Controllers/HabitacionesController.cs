using BlazorCrud.Shared;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Reservas.BData;
using Reservas.BData.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Habitacion")]
    
    public class HabitacionesController : ControllerBase
    {
        private readonly Context context;

        public HabitacionesController(Context dbcontext)
        {
            context = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Habitacion>>> Get()
        {  
            var habitaciones = await context.Habitaciones.ToListAsync();       
            return habitaciones;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Habitacion>> GetById(int id)
        {
            var habitacion = await context.Habitaciones.FirstOrDefaultAsync(c => c.Id == id);

            if (habitacion == null)
            {
                return NotFound($"No se encontró una habitación con el ID: {id}");
            }

            return habitacion;
        }

        [HttpGet("GetNhab/{Nhab:int}")]
        public async Task<ActionResult<Habitacion>> Get(int Nhab)
        {
            var buscar = await context.Habitaciones.FirstOrDefaultAsync(c => c.Nhab == Nhab);

            if (buscar == null)
            {
                return BadRequest($"No se encontro la habitacion de numero: {Nhab}");
            }

            return buscar;
        }

        [HttpGet("Filtro")]
        public async Task<ActionResult<Habitacion>> GetFilter([Required] int NroHab, string Estado)
        {
            var BuscarEso = await context.Habitaciones.FirstOrDefaultAsync(c=>c.Nhab==NroHab && c.Estado==Estado);
            if (BuscarEso is null )
            {
                return NotFound("No se encontro un carajo");
            }
            return BuscarEso;
        }


        [HttpPost] 
        public async Task<IActionResult> Post(HabitacionDTO habitacionDTO)
        {

            var entidad = await context.Habitaciones.FirstOrDefaultAsync(x => x.Nhab == habitacionDTO.Nhab);

            if (habitacionDTO.Nhab <= 0)
            {
                return BadRequest("El número de la habitacion no puede ser menor o igual a 0");
            }

            if (habitacionDTO.Camas <= 0)
            {
                return BadRequest("La cantidad de camas a ingresar no debe ser menor o igual a 0.");
            }

            if(entidad != null) // existe una hab con el num ingresado
            {
                return BadRequest($"Ya existe una habitacion con el número: {habitacionDTO.Nhab}");
            }

            try {
                var mdHabitacion = new Habitacion
                {
                    Nhab = habitacionDTO.Nhab,
                    Camas = habitacionDTO.Camas,
                    Estado = habitacionDTO.Estado,                  
                };
                context.Habitaciones.Add(mdHabitacion);
                await context.SaveChangesAsync();
                return Ok("Cambios guardados correctamente");
            }
            catch (Exception) 
            {
                return BadRequest("No se pudieron guardar los cambios");
            }
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> Editar(HabitacionDTO habitacionDTO, int id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbHabitacion = await context.Habitaciones.FirstOrDefaultAsync(e => e.Id == id);
                if (dbHabitacion != null)
                {
                    dbHabitacion.Nhab = habitacionDTO.Nhab;
                    dbHabitacion.Camas = (int)habitacionDTO.Camas;
                    dbHabitacion.Estado = habitacionDTO.Estado;
                    context.Habitaciones.Update(dbHabitacion);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                    responseApi.Mensaje = "se cargo la hab" + dbHabitacion.Id;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "habitacion no encontrada";
                }
            }
            catch (Exception ex)
            {

                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var responseApi = new ResponseAPI<int>();

           try
            {
                var dbHabitacion = await context.Habitaciones.FirstOrDefaultAsync(e => e.Id == Id);
                if (dbHabitacion != null) 
                {
                    context.Habitaciones.Remove(dbHabitacion);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                }
                else { responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "habitacion no encontrada";
                }
            } catch (Exception ex) {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje =ex.Message;
            }
            return Ok(responseApi); 
        }

        [HttpGet("FiltroFechas/{fechaInicio:DateTime}/{fechaFin:DateTime}")]
        public ActionResult<IEnumerable<List<Habitacion>>> ObtenerFechasYEstadoSeleccionados([Required]DateTime fechaInicio, [Required]DateTime fechaFin)
        {
            if (fechaInicio == DateTime.MinValue || fechaFin == DateTime.MinValue)
            {
                return BadRequest("Las fechas de inicio y fin son obligatorias");
            }

            if (fechaFin.Date < fechaInicio.Date)
            {
                return BadRequest("La fecha de egreso no puede ser anterior a la fecha de ingreso");
            }

            var habitacionesFiltradas = context.Habitaciones
                .Where(h => h.Estado == "ocupada")
                .Join(context.Reservas,
                    habitacion => habitacion.Nhab,
                    reserva => reserva.nhabs,
                    (habitacion, reserva) =>
                    new { Habitacion = habitacion, Reserva = reserva })
                .Where(r => r.Reserva.Fecha_inicio.Date >= fechaInicio.Date && r.Reserva.Fecha_fin.Date <= fechaFin.Date)
                .Select(r => r.Habitacion)
                .ToList();  // Ejecuta la consulta para obtener resultados concretos

            if (!habitacionesFiltradas.Any())
            {
                return NotFound("No se encontraron habitaciones para filtrar");
            }

            return Ok(habitacionesFiltradas);
        }

    }
}