namespace ClinicaBase.Services.ServicioHash
{
    public interface IServicioHash
    {
        public void CreateHashPassword(string password, out string? hashPasswordSalt, out string? salt);
        public bool VerifyHashPassword(string password, string salt, string hashPasswordSalt);
    }
}
