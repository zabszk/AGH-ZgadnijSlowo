using System;
using System.Security.Cryptography;

namespace Server.Misc
{
    public static class SecureRandomGenerator
    {
        private static readonly RNGCryptoServiceProvider Csp = new();

        public static int RandomInt(int min, int max) => min + (int)(GetRandomUInt() % (max - min));

        public static string GeneratePassword(int length = 16)
        {
            const string allowableChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}|/\,.<>?:;`";

            // Generate random data
            var rnd = new byte[length];
            Csp.GetBytes(rnd);

            // Generate the output string
            var allowable = allowableChars.ToCharArray();
            var l = allowable.Length;
            var chars = new char[length];
            for (var i = 0; i < length; i++)
                chars[i] = allowable[rnd[i] % l];

            return new string(chars);
        }

        private static uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        private static byte[] GenerateRandomBytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            Csp.GetBytes(buffer);
            return buffer;
        }
    }
}