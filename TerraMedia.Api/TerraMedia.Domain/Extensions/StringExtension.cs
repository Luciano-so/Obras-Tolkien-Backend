using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Reflection;
using System.Text;

namespace TerraMedia.Domain.Extensions
{
    public static class StringExtension
    {
        public static string Encrypt(this string texto)
        {
            Guid guid = new Guid("b873d610-8b86-42c0-9731-3f93e7abc11d");
            string assyGuid = Assembly.GetExecutingAssembly().GetName().Name ?? "TerraMedia";

            var valueBytes = KeyDerivation.Pbkdf2(
                            password: texto,
                            salt: Encoding.UTF8.GetBytes($"{guid}{assyGuid}"),
                            prf: KeyDerivationPrf.HMACSHA512,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }
    }
}
