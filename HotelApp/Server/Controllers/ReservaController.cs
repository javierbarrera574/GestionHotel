using BlazorCrud.Shared;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Reservar")]
    //
    public class ReservaController : ControllerBase
    {
        private readonly Context context;

        public ReservaController(Context context)
        {
            this.context = context;
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
        public async Task<IActionResult> Post(ReservaDTO reservaDTO)
        {
            var responseApi = new ResponseAPI<int>(); 

            var entidad = await context.Reservas.FirstOrDefaultAsync(x => x.NroReserva == reservaDTO.NroReserva);

            if (entidad != null) // existe una hab con el num ingresado
            {
                return BadRequest("Ya existe una reserva");
            }
            try
            {
                var mdReserva = new Reserva
                {
                    NroReserva = reservaDTO.NroReserva,
                    Fecha_inicio = reservaDTO.Fecha_inicio,
                    Fecha_fin = reservaDTO.Fecha_fin,
                    Dni = reservaDTO.Dni,
                    DniHuesped = reservaDTO.Dns,
                    nhabs = reservaDTO.Nhabs
                };
                context.Reservas.Add(mdReserva);
                await context.SaveChangesAsync();
                return Ok(responseApi);
            }
            catch (Exception ex) { return BadRequest(ex.InnerException.Message); }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Editar(ReservaDTO reservaDTO, int id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbReserva = await context.Reservas.FirstOrDefaultAsync(e => e.Id == id);

                if (dbReserva != null)
                {
                    dbReserva.NroReserva = reservaDTO.NroReserva;
                    dbReserva.Fecha_inicio = reservaDTO.Fecha_inicio;
                    dbReserva.Fecha_fin = reservaDTO.Fecha_fin;
                    dbReserva.Dni = reservaDTO.Dni;
                    dbReserva.DniHuesped = reservaDTO.Dns;
                    dbReserva.nhabs = reservaDTO.Nhabs;
                    context.Reservas.Update(dbReserva);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbReserva.NroReserva;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "reserva no encontrada";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.InnerException.Message;
            }
            return Ok(responseApi);
        }
     
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Borrar(int id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                //var dbReserva = await context.Reservas.FirstOrDefaultAsync(e => e.NroReserva == nroRes);
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
    }
}