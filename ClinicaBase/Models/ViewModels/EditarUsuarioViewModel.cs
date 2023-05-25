using ClinicaBase.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicaBase.Models.ViewModels
{
    public class EditarUsuarioViewModel
    {
        private const string RequiredError = "Campo obligatorio";

        public int Documento { get; set; }

        [Required(ErrorMessage = RequiredError)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        public string Apellidos { get; set; } = null!;

        public int? Succeeded { get; set; } = null;

        public string? Message { get; set; } = null;

        [Required(ErrorMessage = RequiredError)]
        [Role]
        public string Rol { get; set; } = null!;

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
    }
}
