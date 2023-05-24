namespace ClinicaBase.Models.DTOs
{
    public class UserDTO
    {
        public int Documento { get; set; }

        public string Nombres { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Telefono { get; set; } = null!;
    }
}
