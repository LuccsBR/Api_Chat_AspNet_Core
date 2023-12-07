using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Desafio_Chat.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
