using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;

namespace ClinicaBase.Services.ServicioPacientes
{
    public interface IServicioPaciente
    {
        public Task<GeneralResponse> AgregarHistoriaClinica(PacienteViewModel request);
    }
}
