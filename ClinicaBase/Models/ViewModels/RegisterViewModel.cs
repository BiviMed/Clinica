using ClinicaBase.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ClinicaBase.Models.ViewModels
{
    public class RegisterViewModel
    {
        private const string RequiredError = "Campo Obligatorio";

        [Required(ErrorMessage = RequiredError)]
        [Document]
        public int Documento { get; set; }

        [Required(ErrorMessage = RequiredError)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [EmailAddress]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [Phone]
        public string Telefono { get; set; } = null!;
               
        [Required(ErrorMessage = RequiredError)]
        [Role]
        public string Rol { get; set; } = null!;

        public string? Message { get; set; } = null;

        public int? Succeed { get; set; } = null;

        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "Admin",
                    Text = "Admin"
                },
                new SelectListItem
                {
                    Value = "Recursos Humanos",
                    Text = "Recursos Humanos"
                },
                new SelectListItem
                {
                    Value = "Medico",
                    Text = "Medico"
                },
                new SelectListItem
                {
                    Value = "Enfermeria",
                    Text = "Enfermeria"
                },
                new SelectListItem
                {
                    Value = "Secretaria",
                    Text = "Secretaria"
                }
            };        
        /*
            CambioContrasena debe ser 0 o 1;
                Tan pronto se registra un usuario la contraseña
                es la misma que el numero de documento pero debe
                cambiarla.

            Activo debe ser 0 o 1;
                Si sigue trabajando en la Clinica o no.
        */
    }
}
