using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CityVisitTrackerAPI.Helpers
{
    public class PasswordHashGenerator
    {
        public static string ComputeHash(string input, HashAlgorithm algorithm, byte[] salt)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);

            // Combine salt and input bytes
            var saltedInput = new byte[salt.Length + inputBytes.Length];
            salt.CopyTo(saltedInput, 0);
            inputBytes.CopyTo(saltedInput, salt.Length);

            var hashedBytes = algorithm.ComputeHash(saltedInput);

            return BitConverter.ToString(hashedBytes);
        }

        public static byte[] GetSalt()
        {
            using (RandomNumberGenerator random = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[32];
                random.GetNonZeroBytes(salt);
                return salt;
            }

        }

        public static string GetSaltString(byte[] salt)
        {
            return salt.Aggregate("", (current, x) => current + String.Format("{0:x2}", x));
        }

        public static byte[] StringToByteArray(string hex)
        {
            var chars = hex.Length;
            byte[] bytes = new byte[chars / 2];
            for (var i = 0; i < chars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

    }
}
