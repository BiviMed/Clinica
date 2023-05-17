using System.ComponentModel.DataAnnotations;

namespace ClinicaBase.Models.ViewModels
{
    public class EditarPacieteViewModel
    {
        private const string RequiredError = "Campo obligatorio";

        [StringLength(255)]
        public string? Correo { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(150)]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(20)]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(100)]
        public string Profesion { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(100)]
        public string Ocupacion { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(20)]
        public string TelefonoFamiliar { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(100)]
        public string NombreFamiliar { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(30)]
        public string TipoEps { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(50)]
        public string NombreEps { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        public string ExamenFisico { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        public string Antecedentes { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        public string? AntecedentesFarmac { get; set; }
    }
}
