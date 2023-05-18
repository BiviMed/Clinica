using ClinicaBase.Models.Entities;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;

namespace ClinicaBase.Services.ServicioPacientes
{
    public interface IServicioPaciente
    {
        public Task<GeneralResponse> ActializarInformacion(Patient editarPaciente);
        public Task<GeneralResponse> AgregarHistoriaClinica(PacienteViewModel request);
        public Task<GeneralResponse> BuscarPaciente(int documentoRequest);
        public Task<GeneralResponse> BuscarPacientes(BuscarPacienteViewModel pacienteViewModel);
        public void MapearEditarPacienteAPaciente(Patient paciente, EditarPacieteViewModel editarPaciente, out Patient pacienteActualizado);
        public void MapearPacienteAEditarPaciente(Patient paciente, out EditarPacieteViewModel editarPaciente);
    }
}
