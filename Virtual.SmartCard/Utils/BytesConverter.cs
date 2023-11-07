using Virtual.SmartCard.Infrastructure;

namespace Virtual.SmartCard.Utils
{
    public static class BytesConverter
    {
        private const int BITS_PER_BYTE = 8;

        public static int BytesToInt32(byte[] bytes)
        {
            Guard.Against(bytes == null, "BytesToInt32, param bytes cannot be null");
            Guard.Against(bytes.Length > sizeof(int), "BytesToInt32, param bytes cannot fit in an integer");

            int w1Msb = bytes.Length > 3 ? bytes[3] : 0x00;
            int w1Lsb = bytes.Length > 2 ? bytes[2] : 0x00;
            int w0Msb = bytes.Length > 1 ? bytes[1] : 0x00;
            int w0Lsb = bytes[0];

            int result = w0Lsb;
            result |= (w0Msb << BITS_PER_BYTE);
            result |= (w1Lsb << BITS_PER_BYTE * 2);
            result |= (w1Msb << BITS_PER_BYTE * 3);

            return result;
        }

        public static long BytesToInt64(byte[] bytes)
        {
            Guard.Against(bytes == null, "BytesToInt64, param bytes cannot be null");
            Guard.Against(bytes.Length > sizeof(long), "BytesToInt64, param bytes cannot fit in an long");

            long w3Msb = bytes.Length > 7 ? bytes[7] : 0x00;
            long w3Lsb = bytes.Length > 6 ? bytes[6] : 0x00;
            long w2Msb = bytes.Length > 5 ? bytes[5] : 0x00;
            long w2Lsb = bytes.Length > 4 ? bytes[4] : 0x00;
            long w1Msb = bytes.Length > 3 ? bytes[3] : 0x00;
            long w1Lsb = bytes.Length > 2 ? bytes[2] : 0x00;
            long w0Msb = bytes.Length > 1 ? bytes[1] : 0x00;
            long w0Lsb = bytes[0];

            long result = w0Lsb;
            result |= (w0Msb << BITS_PER_BYTE);
            result |= (w1Lsb << BITS_PER_BYTE * 2);
            result |= (w1Msb << BITS_PER_BYTE * 3);
            result |= (w2Lsb << BITS_PER_BYTE * 4);
            result |= (w2Msb << BITS_PER_BYTE * 5);
            result |= (w3Lsb << BITS_PER_BYTE * 6);
            result |= (w3Msb << BITS_PER_BYTE * 7);

            return result;
        }

        public static uint BytesToUInt32(byte[] bytes)
        {
            Guard.Against(bytes == null, "BytesToUInt32, param bytes cannot be null");
            Guard.Against(bytes.Length > sizeof(uint), "BytesToUInt32, param bytes cannot fit in an integer");

            return (uint)(bytes[3] << 24 | bytes[2] << 16 | bytes[1] << 8 | bytes[0]);
        }

        public static uint BytesToUShort(byte[] bytes)
        {
            Guard.Against(bytes == null, "BytesToUShort, param bytes cannot be null");
            Guard.Against(bytes.Length > sizeof(uint), "BytesToUShort, param bytes cannot fit in an integer");

            return bytes[0];
        }

        public static bool BuildBoolFrom1Byte(byte[] bytes)
        {
            Guard.Against(bytes == null, "BuildBoolFrom1Byte, param bytes cannot be null");
            Guard.Against(bytes.Length > sizeof(uint), "BuildBoolFrom1Byte, param bytes cannot fit in an integer");

            return (bytes[0] != 0x00);
        }
    }
}
