using Spotify2.Models;
using Microsoft.EntityFrameworkCore;

namespace Spotify2.Data
{
    //AppDbContext representaçao do bd
    public class AppDbContext : DbContext
    {
        //Db Set representação de uma tabela
        public DbSet<Usuario> Users { get; set; }
        public DbSet<GeneroMusical> Generos { get; set; }
        public DbSet<Artista> Artistas { get; set; }

        // string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("DataSource=app.db; Cache=Shared");
        }
    }
}