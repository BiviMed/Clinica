namespace ClinicaBase.Models.DTOs
{
    public class UserClaimsDTO
    {
        public int Documento { get; set; }

        public string Nombres { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public string Rol { get; set; } = null!;
    }
}
