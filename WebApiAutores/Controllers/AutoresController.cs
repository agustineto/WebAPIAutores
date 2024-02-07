using Microsoft.AspNetCore.Http.HttpResults;
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
        public AutoresController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await context.Autores.Include( x => x.Libro ).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Autor autor)
        {
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
            if(!existe)
            {
                return NotFound("No existe el autor");
            }

            context.Remove(new Autor() { Id = id});
            await context.SaveChangesAsync();
            return Ok();

        }
    }
}
