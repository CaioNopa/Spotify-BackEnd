namespace Spotify2.Models
{
    public class Artista
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Ouvintes { get; set; }
        public string MusicaMaisFamosa { get; set; } = string.Empty;
        public GeneroMusical? GeneroMusical { get; set; }
    }
}
