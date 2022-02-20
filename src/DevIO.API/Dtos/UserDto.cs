using System.ComponentModel.DataAnnotations;

namespace DevIO.API.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "o campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "o campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginUserDto
    {
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "o campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "o campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }
}