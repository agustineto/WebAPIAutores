using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDBContext context, ILogger<AutoresController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        [HttpGet("list")] //api/autores/list
        [HttpGet("/list")] //list
        public async Task<ActionResult<List<Autor>>> Get()
        {
            logger.LogInformation("Obteniendo lista de autores");
            logger.LogWarning("Obteniendo lista de autores con logger logWarning");
            return await context.Autores.Include(x => x.Libro).ToListAsync();
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> GetAutor([FromRoute] int id)
        {
            var existe = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (existe == null)
            {
                return NotFound();
            }

            return existe;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Autor autor)
        {
            var existeAutor = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);

            if (existeAutor)
            {
                return BadRequest($"Ya existe un autor con el mismo nombre {autor.Nombre}");
            }


            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("No coincide el id de la peticion con el autor");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound("No existe el autor");
            }


            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound("No existe el autor");
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();

        }
    }
}
