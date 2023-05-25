using ClinicaBase.Models.DTOs;

namespace ClinicaBase.Models.ViewModels
{
    public class UsuariosViewModel
    {
        public List<UserDTO>? Usuarios { get; set; }

        public int? Exitoso { get; set; } = null;

        public string? Mensaje { get; set; } = null;
    }
}
