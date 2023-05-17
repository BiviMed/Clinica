using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;

namespace ClinicaBase.Services.ServicioUsuarios
{
    public interface IServicioUsuarios
    {
        public Task<GeneralResponse> AddUsuario(RegisterViewModel request);
        public Task<GeneralResponse> Auth(UsuarioAuthViewModel request);
    }
}
