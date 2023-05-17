using System.ComponentModel.DataAnnotations;

namespace ClinicaBase.Models.ViewModels
{
    public class LoginViewModel
    {
        private const string RequiredError = "Campo Obligatorio";

        [Required(ErrorMessage = RequiredError)]
        public int Documento { get; set; }

        [Required(ErrorMessage = RequiredError)]
        public string Contrasena { get; set; } = null!;
    }
}
