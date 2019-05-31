using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WTCPortal.ExtensionMethods
{
    public static class Crypto
    {
        public static string Salt(int length)
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[length];
                provider.GetBytes(buffer);
                string salt = BitConverter.ToString(buffer);
                return salt;
            }
        }

        public static string Hash(string value, string salt)
        {
            string password = String.Concat(value, salt);

            return Convert.ToBase64String(SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}