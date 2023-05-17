using System.ComponentModel.DataAnnotations;

namespace ClinicaBase.Validations
{
    public class RoleAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success!; //para eso esta [Required]
            }

            if (value.ToString() != "Admin" && value.ToString() != "Medico" 
                && value.ToString() != "Enfermeria" && value.ToString() != "Secretaria"
                && value.ToString() != "Recursos Humanos")
            {
                return new ValidationResult("Rol inválido");
            }

            return ValidationResult.Success!;
        }
    }
}
