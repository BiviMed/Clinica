using ClinicaBase.Models.Entities;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;

namespace ClinicaBase.Services.ServicioUsuarios
{
    public interface IServicioUsuarios
    {
        public Task<GeneralResponse> AddUsuario(RegisterViewModel request);
        public Task<GeneralResponse> Auth(UsuarioAuthViewModel request);
        public Task<GeneralResponse> DeleteUser(EliminarUsuarioViewModel usuarioEliminar, string rolUsuarioAutenticado);
        public Task<GeneralResponse> EditarUsuario(EditarUsuarioViewModel request, string rolUsuarioAutenticado);
        public Task<GeneralResponse> FindAllUsers();
        public Task<User?> FindById(int documento);
    }
}
