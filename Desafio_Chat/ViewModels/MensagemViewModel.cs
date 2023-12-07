using System.ComponentModel.DataAnnotations;

namespace Desafio_Chat.ViewModels
{
    public class MensagemViewModel
    {
        public class MensagemCreateViewModel
        {
            [Required]
            public string Email_Usuario_R { get; set; }
            public string Texto { get; set; }
            public byte[]? Imagem { get; set; }



        }
        public class MensagemPutViewModel
        {
            public string Texto { get; set; }
            public byte[] Imagem { get; set; }



        }
    }
}
