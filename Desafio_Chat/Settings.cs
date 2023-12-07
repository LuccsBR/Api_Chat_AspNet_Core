using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Desafio_Chat
{
    public static class Settings
    {
        public static string Secret = "dfDF43$fdf_23FDfdf$D#d43";

        public static string ApiKeyName = "api_key";
        public static string ApiKey = "jdfjsd09sdds0sbd324sdf56";

        public static string GenerateHash(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes("4546546546545");

            string hashed = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 32));

            return hashed;
        }
    }
}
