using System.ComponentModel.DataAnnotations;

namespace Spotify2.ViewModels
{
    public class ArtistaCreateViewModels
    {
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required]
        public int Idade { get; set; }
        [Required]
        public string Descricao { get; set; } = string.Empty;
        
        [Required]
        public int Ouvintes { get; set; }
        [Required]
        public string MusicaMaisFamosa { get; set; } = string.Empty;
        [Required]
        public int GeneroId { get; set; }
    }

    public class ArtistaUpdateViewModel
    {
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required]
        public string Descricao { get; set; } = string.Empty;
        [Required]
        public int Idade { get; set; }
        [Required]
        public int Ouvintes { get; set; }
        [Required]
        public string MusicaMaisFamosa { get; set; } = string.Empty;
        [Required]
        public int GeneroId { get; set; } 
    }
}

