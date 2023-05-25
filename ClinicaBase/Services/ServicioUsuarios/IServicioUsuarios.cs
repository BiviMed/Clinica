using ClinicaBase.Models.Entities;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;

namespace ClinicaBase.Services.ServicioUsuarios
{
    public interface IServicioUsuarios
    {
        public Task<GeneralResponse> AddUsuario(RegisterViewModel request);
        public Task<GeneralResponse> Auth(UsuarioAuthViewModel request);
        public Task<GeneralResponse> DeleteUser(EliminarUsuarioViewModel usuarioEliminar);
        public Task<GeneralResponse> FindAllUsers();
        public Task<User?> FindById(int documento);
    }
}
