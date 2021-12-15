using System.Text;

namespace Server.Misc
{
    public static class Utf8
    {
        private static readonly UTF8Encoding Encoding = new(false);

        public static byte[] GetBytes(string data)
        {
            return Encoding.GetBytes(data);
        }

        public static int GetBytes(string data, byte[] buffer)
        {
            return Encoding.GetBytes(data, 0, data.Length, buffer, 0);
        }

        public static int GetBytes(string data, byte[] buffer, int offset)
        {
            return Encoding.GetBytes(data, 0, data.Length, buffer, offset);
        }

        public static string GetString(byte[] data)
        {
            return Encoding.GetString(data);
        }

        public static string GetString(byte[] data, int offset, int count)
        {
            return Encoding.GetString(data, offset, count);
        }
    }
}