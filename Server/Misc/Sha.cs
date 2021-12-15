using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace Server.Misc
{
    public static class Sha
    {
        public static byte[] Sha512(string message)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(message.Length));
            int length = Utf8.GetBytes(message, buffer);
            byte[] result = Sha512(buffer, 0, length);
            ArrayPool<byte>.Shared.Return(buffer);
            return result;
        }

        private static byte[] Sha512(byte[] message, int offset, int length)
        {
            using var sha512 = SHA512.Create();
            return sha512.ComputeHash(message, offset, length);
        }
        
        public static string HashToString(byte[] hash)
        {
            var result = new StringBuilder(128);
            foreach (var t in hash)
                result.Append(t.ToString("X2"));
            return result.ToString();
        }
    }
}
