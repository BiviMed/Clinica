using System.ComponentModel.DataAnnotations;

namespace ClinicaBase.Validations
{
    public class DocumentAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                int numDoc = Convert.ToInt32(value);
                if (numDoc == 0)
                {
                    return new ValidationResult("Número de documento inválido");
                }
            }
            catch (Exception)
            {
                return new ValidationResult("Número de documento inválido");
            }
            return ValidationResult.Success!;

        }
    }
}
