using System.Security.Cryptography;
using System.Text;

namespace ClinicaBase.Services.ServicioHash
{
    public class ServicioHash256 : IServicioHash
    {
        public void CreateHashPassword(string password, out string? hashPasswordSalt, out string? salt)
        {
            try
            {
                salt = Hash(DateTime.Now.ToString() + "estaesmisal");
                hashPasswordSalt = Hash($"{password}{salt}");
            }
            catch (Exception)
            {
                salt = null;
                hashPasswordSalt = null;
            }          

        }

        public bool VerifyHashPassword(string password, string salt, string hashPasswordSalt)
        {
            string hashLoginPassword = Hash($"{password}{salt}");

            if (hashLoginPassword == hashPasswordSalt)
            {
                return true;
            }
            return false;
        }


        private static string Hash(string toHash)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = Encoding.Default.GetBytes(toHash);
            byte[] hashed = sha256.ComputeHash(hashBytes);
            string hashedString = Convert.ToHexString(hashed);

            return hashedString;
        }
    }
}
