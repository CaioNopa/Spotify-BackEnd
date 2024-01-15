using System.ComponentModel.DataAnnotations;

namespace Spotify2.ViewModels
{
    public class UsuarioLoginViewModel
    {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            public string Senha { get; set; } = string.Empty;
    }

        public class UsuarioSignupViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
            [Required] 
            public string Senha { get; set; } = string.Empty;
            [Required]
            public string Name { get; set; } = string.Empty;
            
           

        }
    }



