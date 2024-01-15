using System.ComponentModel.DataAnnotations;

namespace Spotify2.ViewModels
{
    public class GeneroCreateViewModel
    {
        [Required]
        public string Estilo { get; set; } = string.Empty;
    }
}
