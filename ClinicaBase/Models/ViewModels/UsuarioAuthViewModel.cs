using ClinicaBase.Validations;
using System.ComponentModel.DataAnnotations;

namespace ClinicaBase.Models.ViewModels
{
    public class UsuarioAuthViewModel
    {
        private const string RequiredError = "Campo Obligatorio"; 

        [Required(ErrorMessage = RequiredError)]
        [Document]
        public int Documento { get; set; }

        [Required(ErrorMessage = RequiredError)]
        public string Contrasena { get; set; } = null!;

        public int? Succeed { get; set; } = null;

        public string? Message { get; set; } = null;
    }
}
