using Desafio_Chat.Models;
using Loja.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static Desafio_Chat.ViewModels.MensagemViewModel;

namespace Desafio_Chat.Controllers
{
    public class MensagemControllers : ControllerBase
    {
        [Authorize(Roles = "adm, cliente")]
        [HttpPost("mensagens")]
        public   IActionResult PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] MensagemCreateViewModel viewModel)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var newMensagens = new Mensagens();
                var Email_Usuario_R = RemoveEspacos(viewModel.Email_Usuario_R);
                var Id_Usuario_R =  context.Users.FirstOrDefault(x => x.Email.Trim() ==Email_Usuario_R);
                if (Id_Usuario_R == null) return StatusCode(500, new { message = "Erro interno" });
                var Id_Usuario_E =  context.Users.FirstOrDefault(x => x.Email.Trim() == userEmail);
                if(Id_Usuario_E == null) return StatusCode(500, new { message = "Erro interno" });
             
                newMensagens = new Mensagens
                    {
                        Id_Usuario_E = Id_Usuario_E,
                        Id_Usuario_R = Id_Usuario_R,
                        horario = DateTime.Now,
                        Texto = viewModel.Texto,
                        New = true
                    };
                 context.Mensagens.Add(newMensagens);
                 context.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm, cliente")]
        [HttpPut("mensagens/{id:int}")]
        public async Task<IActionResult> Put(
            [FromServices] AppDbContext context,
            [FromBody] MensagemPutViewModel viewModel,
            [FromRoute] int id)
        {
            try
            {
                var mensagem = await context.Mensagens.FindAsync(id);
                if (mensagem == null)
                    return NotFound();
                mensagem.Texto = viewModel.Texto;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm, cliente")]
        [HttpDelete("mensagens/{id:int}")]
        public async Task<IActionResult> Delete([FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            try
            {
                var mensagem = await context.Mensagens.FindAsync(id);

                if (mensagem == null)
                    return NotFound();

                context.Mensagens.Remove(mensagem);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm, cliente")]
        [HttpGet("mensagens/email")]
       public async Task<IActionResult> GetRId(
           [FromServices] AppDbContext context)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var User_E =await context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
                var User_R = await context.Users.ToListAsync();
                var mensagem = context.Mensagens.Where(m => (m.Id_Usuario_R == User_E) || ( m.Id_Usuario_E == User_E)).Select(x => new Mensagens
                {
                    Id = x.Id,
                    Id_Usuario_E = new User
                    {
                        Email = x.Id_Usuario_E.Email,
                        Name = x.Id_Usuario_E.Name,
                    },
                    Id_Usuario_R = new User
                    {
                        Email = x.Id_Usuario_R.Email,
                        Name = x.Id_Usuario_R.Name,
                    },
                    Texto = x.Texto,
                    horario = x.horario,
                    Image = x.Image,
                    New = x.New
                }).ToList();

                return Ok(mensagem);
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm")]
        [HttpGet("mensagens")]
       public async Task<IActionResult> Get(
           [FromServices] AppDbContext context)
        {
            try
            {
                var user = await context.Users.ToListAsync();
                var mensagem = await context.Mensagens.Select(x => new Mensagens
                {
                    Id = x.Id,
                    Id_Usuario_E = new User
                    {
                        Email = x.Id_Usuario_E.Email,
                        Name = x.Id_Usuario_E.Name
                    },
                    Id_Usuario_R = new User
                    {
                        Email = x.Id_Usuario_R.Email,
                        Name = x.Id_Usuario_R.Name,
                    },
                    Texto = x.Texto,
                    Image = x.Image,
                    horario = x.horario,
                    New = x.New
                }).ToListAsync();
                return Ok(mensagem);
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm, cliente")]
        [HttpGet("mensagens/{email_r}")]
       public async Task<IActionResult> GetR(
           [FromServices] AppDbContext context,
           [FromRoute] string email_r)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var User_E = await context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
                var Email_Usuario_R = RemoveEspacos(email_r);
                var User_R = await context.Users.FirstOrDefaultAsync(i => i.Email == Email_Usuario_R);
                var mensagem = context.Mensagens.Where(m => (m.Id_Usuario_R == User_E && m.Id_Usuario_E == User_R) || (m.Id_Usuario_R == User_R && m.Id_Usuario_E == User_E)).Select(x => new Mensagens
                {
                    Id = x.Id,
                    Id_Usuario_E = new User
                    {
                        Email = x.Id_Usuario_E.Email,
                        Name = x.Id_Usuario_E.Name,
                    },
                    Id_Usuario_R = new User
                    {
                        Email = x.Id_Usuario_R.Email,
                        Name = x.Id_Usuario_R.Name,
                    },
                    Texto = x.Texto,
                    Image = x.Image,
                    horario = x.horario,
                    New = x.New
                }).ToList();

                return Ok(mensagem);
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm, cliente")]
        [HttpGet("mensagens/new/{email_r}")]
       public async Task<IActionResult> GetNew(
           [FromServices] AppDbContext context,
           [FromRoute] string email_r)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var User_E = await context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
                var Email_Usuario_R = RemoveEspacos(email_r);
                var User_R = await context.Users.FirstOrDefaultAsync(i => i.Email == Email_Usuario_R);
                var mensagem = context.Mensagens.Where(m => (m.Id_Usuario_R == User_E && m.Id_Usuario_E == User_R)).Select(x => new Mensagens
                {
                    Id = x.Id,
                    Id_Usuario_E = new User
                    {
                        Email = x.Id_Usuario_E.Email,
                        Name = x.Id_Usuario_E.Name,
                    },
                    Id_Usuario_R = new User
                    {
                        Email = x.Id_Usuario_R.Email,
                        Name = x.Id_Usuario_R.Name,
                    },
                    Texto = x.Texto,
                    Image = x.Image,
                    horario = x.horario,
                    New = x.New
                }).ToList();

                return Ok(mensagem);
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm, cliente")]
        [HttpPut("mensagens/new/{email_r}")]
       public async Task<IActionResult> PutNew(
           [FromServices] AppDbContext context,
           [FromRoute] string email_r)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var User_E = await context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
                var Email_Usuario_R = RemoveEspacos(email_r);
                var User_R = await context.Users.FirstOrDefaultAsync(i => i.Email == Email_Usuario_R);
                var mensagem = context.Mensagens.Where(m => (m.Id_Usuario_R == User_E && m.Id_Usuario_E == User_R)).ToList();
                for(int i =0; i<mensagem.Count;i++)
                {
                    mensagem[i].New = false;
                }
                await context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        [Authorize(Roles = "adm, cliente")]
        [HttpPost("mensagens/imagem")]
        public IActionResult PostImagemAsync(
            [FromServices] AppDbContext context,
            [FromBody] MensagemCreateViewModel viewModel)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var newMensagens = new Mensagens();
                var Email_Usuario_R = RemoveEspacos(viewModel.Email_Usuario_R);
                var Id_Usuario_R = context.Users.FirstOrDefault(x => x.Email.Trim() == Email_Usuario_R);
                if (Id_Usuario_R == null) return StatusCode(500, new { message = "Erro interno" });
                var Id_Usuario_E = context.Users.FirstOrDefault(x => x.Email.Trim() == userEmail);
                if (Id_Usuario_E == null) return StatusCode(500, new { message = "Erro interno" });

                newMensagens = new Mensagens
                {
                    Id_Usuario_E = Id_Usuario_E,
                    Id_Usuario_R = Id_Usuario_R,
                    horario = DateTime.Now,
                    Image = viewModel.Imagem,
                    New = true
                };
                context.Mensagens.Add(newMensagens);
                context.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
        static string RemoveEspacos(string input)
        {
            return Regex.Replace(input, @"\s", "");
        }
    }
   
}
