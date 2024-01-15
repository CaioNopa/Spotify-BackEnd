using Spotify2.Data;
using Spotify2.Models;
using Spotify2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Spotify2.Controllers
{
    [ApiController]
    public class GeneroController : ControllerBase
    {
        [Authorize(Roles = "agente")]
        [HttpPost("genero")]
        public async Task<IActionResult> PostGeneroAsync(
            [FromServices] AppDbContext context, [FromBody] GeneroCreateViewModel model)
        {
            try
            {
                var generos = await context.Generos.FirstOrDefaultAsync(x => x.Estilo == model.Estilo);

                if (generos != null)
                    return StatusCode(401, new
                    {
                        message = "Gênero já cadastrado"
                    });
                var newGenero = new GeneroMusical
                {
                    Estilo = model.Estilo
                };
                await context.Generos.AddAsync(newGenero);
                await context.SaveChangesAsync();

                return Created($"{newGenero.Estilo}", newGenero);
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }


        [Authorize(Roles = "agente")]
        [HttpDelete("genero/{id:int}")]
        public async Task<IActionResult> DeleteGeneroAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var genero = await context.Generos.FindAsync(id);

                if (genero == null)
                    return NotFound(new { message = "Genero não existe" });

                context.Generos.Remove(genero);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Erro no servidor" });
            }
        }


        [Authorize(Roles = "ouvinte, agente")]
        [HttpGet("generos")]
        public async Task<IActionResult> GetGenerosAsync(
            [FromServices] AppDbContext context)
        {
            try
            {
                var users = await context.Generos.ToListAsync();

                return Ok(users);
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno no servidor" });
            }
        }



    }
}
