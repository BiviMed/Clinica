using System.ComponentModel.DataAnnotations;

namespace ClinicaBase.Models.ViewModels
{
    public class NuevoControleViewModel
    {
        private const string RequiredError = "Campo obligatorio";

        public Guid Id { get; set; }

        [Required(ErrorMessage = RequiredError)]
        public int PatientId { get; set; }

        [Required(ErrorMessage = RequiredError)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        public string Motivo { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        public string Tratamiento { get; set; } = null!;

        public int UserId { get; set; }

        // 0 o 1
        public int? Succeeded { get; set; } = null;

        public string? Message { get; set; } = null;
    }
}
