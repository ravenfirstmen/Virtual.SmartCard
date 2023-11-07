using System;

namespace Virtual.SmartCard.TLV.Asn1.Utils
{
    public static class Asn1Utils
    {
        public static int MinBytesNeededForEncoding(uint value)
        {
            return MinBytesNeededForEncoding((ulong)value);
        }

        public static int MinBytesNeededForEncoding(ulong value)
        {
            int numAsn1Bytes = 1;
            while (value > (Math.Pow(2, (7 * numAsn1Bytes)) - 1))  // apenas 7 bits são usaveis para encoding da tagnumAsn1...
            {
                numAsn1Bytes++;
            }

            return numAsn1Bytes;
        }

        public static int MinBytesNeededForEncoding(long value)
        {
            // diferença ulong ... valores negativos
            int numAsn1Bytes = 1;
            while (value > (Math.Pow(2, (8 * numAsn1Bytes) - 1) - 1) || value < Math.Pow(-2, (8 * numAsn1Bytes) - 1))
            {
                numAsn1Bytes++;
            }

            return numAsn1Bytes;
        }
    }
}
