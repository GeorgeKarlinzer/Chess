using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public static class Encryption
    {
        public static string GenerateHash(string value, string salt)
        {
            var valueBytes = Encoding.ASCII.GetBytes(value);
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            if (saltBytes.Length != 32)
                throw new ArgumentException("Salt must have 32 bytes", nameof(salt));

            var bytes = SHA256.HashData(valueBytes);
            for (int i = 0; i < 32; i++)
                bytes[i] ^= saltBytes[i];

            return Encoding.ASCII.GetString(bytes);
        }

        public static string GenerateSalt()
        {
            var bytes = new byte[32];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)new Random().Next(byte.MinValue, byte.MaxValue);

            var sha256 = SHA256.HashData(bytes);
            return Encoding.ASCII.GetString(sha256);
        }
    }
}