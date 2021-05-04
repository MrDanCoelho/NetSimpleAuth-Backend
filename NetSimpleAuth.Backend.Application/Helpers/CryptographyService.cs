using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NetSimpleAuth.Backend.Application.Helpers
{
    public static class CryptographyService
    {
        public static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }
        
        public static string HashPassword(string password)
        {
            var bytes = Encoding.Unicode.GetBytes(password);
            var hashString = new SHA256Managed();
            var hash = hashString.ComputeHash(bytes);
            return hash.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
        }
    }
}