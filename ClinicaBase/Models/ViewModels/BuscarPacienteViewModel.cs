using ClinicaBase.Models.Entities;

namespace ClinicaBase.Models.ViewModels
{
    public class BuscarPacienteViewModel
    {
        public int? Documento { get; set; } = null;

        public string? Nombres { get; set; } = null;

        public string? Apellidos { get; set; } = null;

        public int? Succeeded { get; set; } = null;

        public string? Message { get; set; } = null;

        public List<Patient>? Pacientes { get; set; } = null;
    }
}
