using System;
using System.Security.Cryptography;

namespace Server.Misc
{
    public static class SecureRandomGenerator
    {
        private static readonly RNGCryptoServiceProvider _csp;
        private static byte[] uintBuffer = new byte[sizeof(uint)];

        static SecureRandomGenerator()
        {
            _csp = new RNGCryptoServiceProvider();
        }

        public static int RandomInt(int minValue, int maxExclusiveValue)
        {
            if (minValue >= maxExclusiveValue)
                throw new ArgumentOutOfRangeException("minValue must be lower than maxExclusiveValue");

            long diff = (long)maxExclusiveValue - minValue;
            long upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                ui = GetRandomUInt();
            } while (ui >= upperBound);
            return (int)(minValue + (ui % diff));
        }

        public static string GeneratePassword(int length = 16)
        {
            const string allowableChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}|/\,.<>?:;`";

            // Generate random data
            var rnd = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(rnd);

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
            //var randomBytes = GenerateRandomBytes(sizeof(uint));
            _csp.GetBytes(uintBuffer);
            return BitConverter.ToUInt32(uintBuffer, 0);
            //return BitConverter.ToUInt32(randomBytes, 0);
        }

        /*private static byte[] GenerateRandomBytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            _csp.GetBytes(buffer);
            return buffer;
        }*/
    }
}