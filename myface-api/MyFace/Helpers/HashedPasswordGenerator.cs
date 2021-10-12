using System;
using System.Security.Cryptography;
using System.Text;

namespace MyFace.Helpers
{
    public static class HashedPassword
    {
        private const int SALT_SIZE = 32;

        public static string CreateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[SALT_SIZE];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            var sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}