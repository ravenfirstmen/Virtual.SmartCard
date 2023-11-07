namespace Virtual.SmartCard.Utils
{
    // Big endian para Litlle endian
    public static class EndianNormalizer
    {
        public static short Normalize(short host)
        {
            return (short)(((host & 0xff) << 8) | ((host >> 8) & 0xff));
        }

        public static int Normalize(int host)
        {
            return (((Normalize((short)host) & 0xffff) << 0x10) | (Normalize((short)(host >> 0x10)) & 0xffff));
        }

        public static long Normalize(long host)
        {
            return
                (((Normalize((int)host) & 0xffffffffL) << 0x20) | (Normalize((int)(host >> 0x20)) & 0xffffffffL));
        }

    }
}