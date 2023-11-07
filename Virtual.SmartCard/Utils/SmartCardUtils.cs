using System.Text;

namespace Virtual.SmartCard.Utils
{
    public static class SmartCardUtils
    {
        public static string GetAnsiStringAttribute(byte[] data, string @default)
        {
            return GetAttribute(data, Encoding.ASCII, @default);
        }

        public static string GetUnicodeStringAttribute(byte[] data, string @default)
        {
            return GetAttribute(data, Encoding.Unicode, @default);
        }

        public static string GetAttribute(byte[] data, Encoding encoding, string @default)
        {
            return data == null ? @default : encoding.GetString(data);
        }
    }
}