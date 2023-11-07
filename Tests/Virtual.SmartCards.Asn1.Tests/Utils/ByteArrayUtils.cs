namespace Virtual.SmartCards.Asn1.Tests.Utils
{
    public static class ByteArrayUtils
    {
        public static bool AreEqual(byte[] arrayA, byte[] arrayB)
        {
            if (arrayA == null || arrayB == null)
            {
                return false;
            }

            if (arrayA.Length != arrayB.Length)
            {
                return false;
            }

            for (int i = 0; i < arrayA.Length; i++)
            {
                if (arrayA[i] != arrayB[i])
                {
                    return false;
                }
            }

            return true;
        }


        public static bool AreEqual(uint[] arrayA, uint[] arrayB)
        {
            if (arrayA == null || arrayB == null)
            {
                return false;
            }

            if (arrayA.Length != arrayB.Length)
            {
                return false;
            }

            for (int i = 0; i < arrayA.Length; i++)
            {
                if (arrayA[i] != arrayB[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
