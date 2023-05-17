using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ClinicaBase.Validations;
using System.Runtime.CompilerServices;

namespace ClinicaBase.Models.ViewModels
{
    public class PacienteViewModel
    {
        private const string RequiredError = "Campo obligatorio";

        [Document]
        [Required(ErrorMessage = RequiredError)]
        public int Documento { get; set; }

        [Required(ErrorMessage = RequiredError)]
        [StringLength(50)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(50)]
        public string Apellidos { get; set; } = null!;
                
        [StringLength(255)]
        public string? Correo { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(150)]        
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(20)]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        public DateTime FechaNacimiento { get; set; }

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

        public int? UserId { get; set; }
                
        public DateTime? FechaCreacion { get; set; }

        public int? Succeeded { get; set; } = null;

        public string? Mensaje { get; set; } = null;
    }
}
