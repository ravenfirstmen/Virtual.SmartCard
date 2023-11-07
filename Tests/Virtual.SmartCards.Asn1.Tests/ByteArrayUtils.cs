using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual.SmartCards.Asn1.Tests
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
    }
}
