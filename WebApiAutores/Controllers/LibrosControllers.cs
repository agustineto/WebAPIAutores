using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/Libros")]
    public class LibrosControllers : ControllerBase
    {
        private readonly ApplicationDBContext context;
        public LibrosControllers(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Libro libro)
        {
            var existe = await context.Autores.AnyAsync( x => x.Id == libro.AutorId);

            if (!existe)
            {
                return BadRequest("No existe el autor para este libro");
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
