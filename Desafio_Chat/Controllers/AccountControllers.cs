using Desafio_Chat.Models;
using Desafio_Chat.Services;
using Desafio_Chat.ViewModels;
using Loja.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Desafio_Chat.Controllers
{
    [ApiController]

    public class AccountControllers : ControllerBase
    {
        [HttpPost("account/login")]
        public async Task<IActionResult> Login(
          [FromServices] AppDbContext context,
          [FromBody] AccountLoginViewModel model,
          [FromServices] TokenService tokenService)
        {
            try
            {
                var user = await context.Users
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user == null)
                    return StatusCode(401, new { message = "usuario ou senha invalido" });

                if (user.Password != Settings.GenerateHash(model.Password))
                    return StatusCode(401, new { message = "usuario ou senha invalido" });

                var token = tokenService.CreateToken(user);

                return Ok(new { token });
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [HttpPost("account/Signup")]
        public async Task<IActionResult> SignUp(
            [FromBody] AccountSignupViewModel model,
            [FromServices] AppDbContext context)
        {
            try
            {
                var user = await context.Users
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user != null)
                    return StatusCode(401, new { message = "Email já cadastrado!" });

                var newUser = new User
                {
                    Email = model.Email,
                    Password = Settings.GenerateHash(model.Password),
                    Name = model.Name,
                    Role = "cliente"
                };

                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm, cliente")]
        [HttpGet("account/{email}")]
        public async Task<IActionResult> GetEmail(
           [FromServices] AppDbContext context,
           [FromRoute] string email)
        {
            try
            {
                var user = await context.Users.Where(x => x.Email == email).Select(x => new User
                    {
                        Email = x.Email,
                        Name = x.Name,
                    }) .FirstOrDefaultAsync();
                if (user == null) return StatusCode(401, new { message = "usuario invalido" });

                return Ok(new {user});
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm")]
       [HttpGet("account")]
        public async Task<IActionResult> GetEmailAll(
          [FromServices] AppDbContext context)
        {
            try
            {
                var user =  context.Users.ToList();
                if (user == null) return StatusCode(401, new { message = "usuario invalido" });

                return Ok(new { user });
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
    }
}
