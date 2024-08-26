using APISeguros.Context;
using APISeguros.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APISeguros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguroController : ControllerBase
    {
        private ApplicationDbContext _context;

        public SeguroController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los asegurados con paginación
        [HttpGet]
        public ActionResult<PagedResult<Seguro>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Obtener el total de registros disponibles
            var totalCount = _context.Seguros.Count();

            // Obtener los registros para la página actual
            var seguros = _context.Seguros
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            // Crear el resultado paginado
            var result = new PagedResult<Seguro>
            {
                Items = seguros,
                TotalCount = totalCount
            };

            return Ok(result);
        }

        // Clase para representar el resultado paginado
        public class PagedResult<T>
        {
            public List<T> Items { get; set; } = new List<T>();
            public int TotalCount { get; set; }
        }


        // GET: api/Seguro/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Seguro>> GetSeguro(int id)
        {
            // Busca el seguro por ID en la base de datos
            var seguro = await _context.Seguros.FindAsync(id);

            // Si no se encuentra el seguro, devuelve 404 Not Found
            if (seguro == null)
            {
                return NotFound();
            }

            // Devuelve el seguro encontrado
            return Ok(seguro);
        }

        // Obtener asegurados por número de identificación
        [HttpGet("filtrar")]
        public ActionResult<IEnumerable<Seguro>> GetByIdentification([FromQuery] string NumeroIdentificacion)
        {
            var seguros = _context.Seguros
                                  .Where(s => s.NumeroIdentificacion == NumeroIdentificacion)
                                  .ToList();
            return Ok(seguros);
        }

        [HttpPost]
        public async Task<ActionResult<Seguro>> Create([FromBody] Seguro seguro)
        {
            if (seguro == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Seguros.Add(seguro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("duplicate") ?? false)
                {
                    return Conflict(new { message = "El número de identificación ya existe." });
                }
                return StatusCode(500, "Error al guardar los datos.");
            }

            return CreatedAtAction(nameof(Get), new { id = seguro.id }, seguro);
        }

        // Actualizar un asegurado existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Seguro seguro)
        {
            if (id != seguro.id)
            {
                return BadRequest(new { message = "El ID del asegurado no coincide." });
            }

            _context.Entry(seguro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate"))
                {
                    // Verifica si el error es debido a un índice único duplicado
                    return Conflict(new { message = "El número de identificación ya existe." });
                }

                return StatusCode(500, "Error al actualizar los datos.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al actualizar los datos.");
            }

            return NoContent();
        }

        // Eliminar un asegurado
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var seguro = _context.Seguros.FirstOrDefault(s => s.id == id);
            if (seguro == null)
            {
                return NotFound();
            }

            _context.Seguros.Remove(seguro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
