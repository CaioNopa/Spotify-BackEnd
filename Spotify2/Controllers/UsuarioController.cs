using Spotify2.Models;
using Spotify2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Spotify2.Data;
using Spotify2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Spotify2.Controllers
{
    [ApiController]
        public class UsuarioController : ControllerBase
        {
        [HttpPost("account/login")]
            public IActionResult Login(
                [FromBody] UsuarioLoginViewModel model,
                [FromServices] AppDbContext context,
                [FromServices] TokenService tokenService)
            {
                var user = context.Users.FirstOrDefault(
                    x => x.Email == model.Email);

                if (user == null)
                    return StatusCode(401, new { message = "Usuário ou senha inválidos" });

                if (user.Senha != Settings.GenerateHash(model.Senha))
                    return StatusCode(401, new { message = "Usuário ou senha inválidos" });

                try
                {
                    var token = tokenService.CreateToken(user);

                    return Ok(new { token = token, user });
                }
                catch
                {
                    return StatusCode(500, new
                    {
                        message = "Erro interno no servidor"
                    });
                }
            }


            //Registro Ouvinte
        [HttpPost("account/signup")]
            public IActionResult Signup(
                [FromBody] UsuarioSignupViewModel model,
                [FromServices] AppDbContext context)
            {
                var user = context.Users.FirstOrDefault(
                    x => x.Email == model.Email);

                if (user != null)
                    return StatusCode(401, new
                    {
                        message = "Email já cadastrado"
                    });         

                try
                {
                var newUser = new Usuario
                {
                    Email = model.Email,
                    Senha = Settings.GenerateHash(model.Senha),
                    Name = model.Name,
                    Role = "agente"
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();

                    return Ok(new
                    {
                        message = "Registro realizado com sucesso"
                    });
                }
                catch
                {
                    return StatusCode(500, new
                    {
                        message = "Erro interno no servidor"
                    });
                }
            }


            //Registro Agente
        //[HttpPost("account/signup/agente")]
        //    public IActionResult SignupOuvinte(
        //    [FromBody] UsuarioSignupViewModel model,
        //    [FromServices] AppDbContext context)
        //{
        //    var user = context.Users.FirstOrDefault(
        //        x => x.Email == model.Email);

        //    if (user != null)
        //        return StatusCode(401, new
        //        {
        //            message = "Email já cadastrado"
        //        });

        //    try
        //    {
        //        var newUser = new Usuario
        //        {
        //            Email = model.Email,
        //            Senha = Settings.GenerateHash(model.Senha),
        //            Name = model.Name,
        //            Role = "agente"
        //        };

        //        context.Users.Add(newUser);
        //        context.SaveChanges();

        //        return Ok(new
        //        {
        //            message = "Registro realizado com sucesso"
        //        });
        //    }
        //    catch
        //    {
        //        return StatusCode(500, new
        //        {
        //            message = "Erro interno no servidor"
        //        });
        //    }
        //}


        [Authorize(Roles = "agente")]
        [HttpGet("usuarios")]
            public async Task<IActionResult> GetUsuariosAsync(
               [FromServices] AppDbContext context)
        {
            try
            {
                var users = await context.Users.ToListAsync();

                return Ok(users);
            }
            catch
            {
                return StatusCode(500, new { message = "Erro no servidor" });
            }
        }


        [Authorize(Roles = "agente")]
        [HttpDelete("usuario/{id:int}")]
            public async Task<IActionResult> DeletaUsuarioAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var users = await context.Users.FindAsync(id);

                if (users == null)
                    return NotFound(new { message = "Usuario não existe" });

                context.Users.Remove(users);
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
