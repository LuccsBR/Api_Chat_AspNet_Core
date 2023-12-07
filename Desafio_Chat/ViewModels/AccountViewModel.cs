using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Desafio_Chat.ViewModels
{
    public class AccountLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

    }

    public class AccountSignupViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;

    }
    public class AccountGetViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

    }
}
