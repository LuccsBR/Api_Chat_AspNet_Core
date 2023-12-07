namespace Desafio_Chat.Models
{
    public class Mensagens
    {
        public int Id { get; set; }
        public User Id_Usuario_E { get; set; }
        public User Id_Usuario_R { get; set; }
        public string? Texto { get; set; }
        public DateTime horario { get; set; }
        public byte[]? Image { get; set; }
        public bool New { get; set; }


    }
}
