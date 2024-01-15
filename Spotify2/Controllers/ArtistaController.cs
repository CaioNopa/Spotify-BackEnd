using Spotify2.Data;
using Spotify2.Models;
using Spotify2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Spotify2.Controllers
{

    [ApiController]
        public class ArtistaController : ControllerBase
        {
            [Authorize(Roles = "agente")]
            [HttpPost("artista")]
            public async Task<IActionResult> PostAsync(
                [FromServices] AppDbContext context,
                [FromBody] ArtistaCreateViewModels model)
            {
                try
                {
                    var genero = await context.Generos.FindAsync(model.GeneroId);
                if (genero == null)
                    return NotFound(new { message = "Genero não existe" });

                var artistas = await context.Artistas.FirstOrDefaultAsync(x => x.Nome == model.Nome);

                if (artistas != null)
                    return StatusCode(401, new
                    {
                        message = "Artista já cadastrado"
                    });

                var artistaNew = new Artista
                    {
                        Nome = model.Nome,
                        Idade = model.Idade,
                        Descricao = model.Descricao,
                        Ouvintes = model.Ouvintes,
                        MusicaMaisFamosa = model.MusicaMaisFamosa,
                        GeneroMusical = genero
                    };

                    await context.Artistas.AddAsync(artistaNew);
                    await context.SaveChangesAsync();

                    return Ok(artistaNew);
                }
                catch
                {
                    return StatusCode(500, new { message = "Erro no servidor" });
                }
            }

            [Authorize(Roles = "ouvinte, agente")]
            [HttpGet("artistas")]
            public async Task<IActionResult> GetAsync(
                [FromServices] AppDbContext context)
            {
                try
                {
                    var artistas = await context.Artistas
                        .Include(x => x.GeneroMusical)
                        .ToListAsync();

                    return Ok(artistas);
                }
                catch
                {
                    return StatusCode(500, new { message = "Erro no servidor" });
                }
        }

        ////GET POR ID
        //   [Authorize(Roles = "ouvinte, agente")]
        //   [HttpGet("artistas/{id:int}")]
        //   public async Task<IActionResult> GetIDAsync(
        //     [FromServices] AppDbContext context,
        //     [FromRoute] int id)
        //   {
        //    try
        //    {
        //        var artista = await context.Artistas
        //            .Include(x => x.GeneroMusical)
        //            .FirstOrDefaultAsync(x => x.Id == id);

        //        if (artista == null)
        //            return NotFound();

        //        return Ok(artista);
        //    }
        //    catch
        //    {
                
        //        return StatusCode(500, "Erro interno do servidor");
        //    }
        //   }



        //BUSCA TODOS OS ARTISTAS DE DETERMINADO GENERO
        [Authorize(Roles = "ouvinte, agente")]
        [HttpGet("artistass/{nomeGenero}")]
        public async Task<IActionResult> GetArtistasPorGeneroAsync(
           [FromServices] AppDbContext context,
           [FromRoute] string nomeGenero)
        {
            try
            {
                var artistas = await context.Artistas
                    .Include(x => x.GeneroMusical)
                    .Where(x => x.GeneroMusical.Estilo == nomeGenero)
                    .ToListAsync();

                if (artistas == null || !artistas.Any())
                    return NotFound();

                return Ok(artistas);
            }
            catch
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        //BUSCA pelo nome do artista
        [Authorize(Roles = "ouvinte, agente")]
        [HttpGet("artista/{nome}")]
        public async Task<IActionResult> GetArtistasPorNomeAsync(
        [FromServices] AppDbContext context,
        [FromRoute] string nome)
        {
            try
            {
                var artistas = await context.Artistas
                    .Include(x => x.GeneroMusical)
                    .Where(x => x.Nome == nome)
                    .ToListAsync();

                if (artistas == null || !artistas.Any())
                    return NotFound();

                return Ok(artistas);
            }
            catch
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }


            [Authorize(Roles = "agente")]
            [HttpPut("artista/{nomeArtista}")]
            public async Task<IActionResult> PutAsync(
                [FromServices] AppDbContext context,
                [FromBody] ArtistaUpdateViewModel model,
                [FromRoute] string nomeArtista)
            {
                try
                {
                    var artistas = await context.Artistas
                      .FirstOrDefaultAsync(x => x.Nome == nomeArtista);



                    if (artistas == null)
                        return NotFound(new { message = "Artista não existe" });


                    var genero = await context.Generos
                        .FindAsync(model.GeneroId);

                    if (genero == null)
                        return NotFound(new { message = "Genero não existe" });


                    artistas.Nome = model.Nome;
                    artistas.Idade = model.Idade;
                    artistas.Descricao = model.Descricao;
                    artistas.Ouvintes = model.Ouvintes;
                    artistas.MusicaMaisFamosa = model.MusicaMaisFamosa;
                    artistas.GeneroMusical = genero;


                    await context.SaveChangesAsync();

                    return Ok();
                }
                catch
                {
                    return StatusCode(500, new { message = "Erro no servidor" });
                }
            }

            [Authorize(Roles = "agente")]
            [HttpDelete("artista/{nomeArtista}")]
            public async Task<IActionResult> DeleteAsync(
                [FromServices] AppDbContext context,
                [FromRoute] string nomeArtista)
            {
                try
                {
                  var artistas = await context.Artistas
                  .FirstOrDefaultAsync(x => x.Nome == nomeArtista);

                  if (artistas == null)
                        return NotFound(new { message = "Artista não existe" });

                    context.Artistas.Remove(artistas);
                    await context.SaveChangesAsync();

                    return Ok();
                }
                catch
                {
                    return StatusCode(500, new { message = "Erro no servidor" });
                }
            }
        }
    }

