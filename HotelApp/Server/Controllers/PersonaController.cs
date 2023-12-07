using BlazorCrud.Shared;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Persona")]

    public class PersonaController : ControllerBase
    {
        private readonly Context context;

        public PersonaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Persona>>> Get()
        {
            return await context.Personas.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Persona>> Get(int id)
        {
            var persona = await context.Personas.FirstOrDefaultAsync(p => p.Id == id);

            if (persona == null)
            {
                return NotFound(); 
            }

            return persona;
        }

        [HttpGet("GetDni/{dniPersona:int}")]
        public async Task<ActionResult<Persona>> GetDniPersona(int dniPersona)
        {
            var buscar = await context.Personas.FirstOrDefaultAsync(c => c.Dni == dniPersona);

            if (buscar is null)
            {
                return BadRequest($"No se encontro la Persona de Dni numero: {dniPersona}");
            }

            return buscar;
        }
        [HttpGet("GetNumHab/{NumHab:int}")]
        public async Task<ActionResult<List<Persona>>> GetNumHab(int numhab)
        {
            var buscar = await context.Personas.Where(c => c.NumHab == numhab).ToListAsync();

            if (buscar.Count ==0)
            {
                return BadRequest($"No se encontro la Persona de numero de habitacion: {numhab}");
            }

            return buscar;
        }    

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Editar(PersonaDTO personaDTO, int id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbPersona = await context.Personas.FirstOrDefaultAsync(e => e.Id == id);
                if (dbPersona != null)
                {
                    dbPersona.Nombres = personaDTO.Nombres;
                    dbPersona.Apellidos = personaDTO.Apellidos;
                    dbPersona.Correo = personaDTO.Correo;
                    dbPersona.Telefono = personaDTO.Telefono;
                    dbPersona.NumTarjeta = personaDTO.NumTarjeta;
                    dbPersona.NumHab = personaDTO.NumHab;
                    dbPersona.EsHuespedyReservante = personaDTO.EshuespedYreservante;
                    context.Personas.Update(dbPersona);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbPersona.Dni;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Persona no encontrada";
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
        public async Task<IActionResult> Borrar(int id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbPersona = await context.Personas.FirstOrDefaultAsync(e => e.Id == id);
                if (dbPersona != null)
                {
                    context.Personas.Remove(dbPersona);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                }else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Persona no encontrada";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }


        //Metodo para implementar la función

        [HttpPost]
        public async Task<IActionResult> Post2(PersonaDTO personaDTO)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Validación en el servidor
                var entidadPersona = await context.Personas
                    .FirstOrDefaultAsync(x => x.Dni == personaDTO.Dni);

                if (entidadPersona != null)
                {
                    // La persona ya existe
                    return BadRequest($"Ya existe una persona con el DNI N°: {personaDTO.Dni}");
                }

                // Validación de EsHuespedYReservante
                if (personaDTO.EshuespedYreservante)
                {
                    // Verificar si la persona ya es huésped y reservante
                    var entidadHuespedReservante = await context.Personas
                        .FirstOrDefaultAsync(x => x.Dni == personaDTO.Dni && x.EsHuespedyReservante);

                    if (entidadHuespedReservante != null)
                    {
                        return BadRequest("La persona ya está registrada como huésped y reservante.");
                    }
                }

                // Agregar persona
                var mdPersona = new Persona
                {
                    Dni = (int)personaDTO.Dni,
                    Nombres = personaDTO.Nombres,
                    Apellidos = personaDTO.Apellidos,
                    Correo = personaDTO.Correo,
                    Telefono = personaDTO.Telefono,
                    NumTarjeta = personaDTO.NumTarjeta,
                    NumHab = personaDTO.NumHab,
                    EsHuespedyReservante = personaDTO.EshuespedYreservante
                };

                context.Personas.Add(mdPersona);
                await context.SaveChangesAsync();

                // Commit de la transacción si todo fue exitoso
                await transaction.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Rollback de la transacción en caso de error
                await transaction.RollbackAsync();

                return BadRequest(ex);
            }
        }
    }
}
