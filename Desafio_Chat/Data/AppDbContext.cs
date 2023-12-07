using Desafio_Chat.Models;
using Microsoft.EntityFrameworkCore;
namespace Loja.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Mensagens> Mensagens { get; set; }
        
    }
}
