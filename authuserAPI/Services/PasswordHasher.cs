using System.Security.Cryptography;

namespace authuserAPI.Services
{
    public class PasswordHasher
    {
        public (string Hash, string Salt) HashPassword(string password)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            var hmac = new HMACSHA256(saltBytes);
            var hashBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            string hash = Convert.ToBase64String(hashBytes);
            string salt = Convert.ToBase64String(saltBytes);

            return (hash, salt);
        }
    }
}
