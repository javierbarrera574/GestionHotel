using BlazorCrud.Shared;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Habitacion")]
    
    //

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

        [HttpPost] 
        public async Task<IActionResult> Post(HabitacionDTO habitacionDTO)
        {

            var entidad = await context.Habitaciones.FirstOrDefaultAsync(x => x.Nhab == habitacionDTO.Nhab);

            if(entidad != null) // existe una hab con el num ingresado
            {
                return BadRequest("Ya existe una Habitacion con ese número");
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
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
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

        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Put(Habitacion entidad, int id)
        //{
        //    if (id != entidad.Id)
        //    {
        //        return BadRequest("El id de la Persona no corresponde.");
        //    }

        //    var existe = await context.Habitaciones.AnyAsync(x => x.Id == id);
        //    if (!existe)
        //    {
        //        return NotFound($"La Personas de id={id} no existe");
        //    }

        //    context.Update(entidad);
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}


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

        [HttpPost("AgregarMuchasHabitaciones")]

        public async Task<ActionResult> PostHabitaciones(List<Habitacion> habitaciones)
        {
            context.AddRange(habitaciones);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}