using System.ComponentModel.DataAnnotations;

namespace ClinicaBase.Models.ViewModels
{
    public class PerfilViewModel
    {
        private const string RequiredError = "Campo obligatorio";
        
        
        public int Documento { get; set; }

        public string Nombres { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [Phone(ErrorMessage = "Teléfono inválido")]
        public string Telefono { get; set; } = null!;

        public string Rol { get; set; } = null!;

        public int? Succeeded { get; set; } = null;

        public string? Message { get; set; } = null;
    }
}
