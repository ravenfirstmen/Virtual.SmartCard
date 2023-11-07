using System;
using Virtual.SmartCard.Infrastructure;

namespace Virtual.SmartCard.PDU
{
    public class PDUBody
    {
        public PDUBody(byte[] data, int le)
        {
            Guard.Against(Le < 0, "Ne deve ser >= 0");
            Guard.Against(Le > 65536, "Ne deve ser <= 65536");
            Guard.Against(data != null && data.Length > 65535, "Número de bytes para os dados devem <= 65535");

            Data = data;
            Le = le;
        }

        public int Lc
        {
            get { return Data != null ? Data.Length : 0; }
        }

        public byte[] Data { get; private set; }
        public int Le { get; private set; }

        public int ExpectecResponseLength // + sw1 e sw2
        {
            get
            {
                EncodingCase @case = GetEncodingCase();

                switch (@case)
                {
                    case EncodingCase.Case2s:
                    case EncodingCase.Case4s:
                        return Le == 0 ? 256 + 2 : Le + 2;

                    case EncodingCase.Case2e:
                    case EncodingCase.Case4e:
                        return Le == 0 ? 65536 + 2 : Le + 2;

                    default:
                        return 0 + 2;
                }
            }

        } // data length + SW1 + SW2

        public byte[] Serialize()
        {
            byte[] apdu = null;

            if (Lc == 0)
            {
                if (Le == 0)
                {
                    // Caso 1
                    // Nada, apenas o header é que é codificado. Não existe Body
                }
                else
                {
                    // Caso 2s ou 2e
                    if (Le <= 256)
                    {
                        // Caso 2s
                        // 256 codificado como 0x00
                        byte le = (Le != 256) ? (byte)Le : (byte)0;
                        apdu = new byte[1];
                        apdu[0] = le;
                    }
                    else
                    {
                        // Caso 2e
                        byte le1, le2;
                        // 65536 codificado como 0x00 0x00
                        if (Le == 65536)
                        {
                            le1 = 0;
                            le2 = 0;
                        }
                        else
                        {
                            le1 = (byte)(Le >> 8 & 0xFF);
                            le2 = (byte)(Le & 0xFF);
                        }
                        apdu = new byte[3];
                        apdu[0] = 0x00;
                        apdu[1] = le1;
                        apdu[2] = le2;
                    }
                }
            }
            else
            {
                if (Le == 0)
                {
                    // Caso 3s ou 3e
                    if (Lc <= 255)
                    {
                        // Caso 3s
                        apdu = new byte[1 + Lc];
                        apdu[0] = (byte)Lc;
                        Buffer.BlockCopy(Data, 0, apdu, 1, Lc);
                    }
                    else
                    {
                        // Caso 3e
                        var lc1 = (byte)(Lc >> 8 & 0xFF);
                        var lc2 = (byte)(Lc & 0xFF);

                        apdu = new byte[3 + Lc];
                        apdu[0] = 0x00;
                        apdu[1] = lc1;
                        apdu[2] = lc2;
                        Buffer.BlockCopy(Data, 0, apdu, 3, Lc);
                    }
                }
                else
                {
                    // Caso 4s ou 4e
                    if ((Lc <= 255) && (Le <= 256))
                    {
                        // Caso 4s
                        var lc = (byte)(Lc & 0xFF);
                        var le = (byte)((Le != 256) ? (byte)Le : 0);

                        apdu = new byte[2 + Lc];
                        apdu[0] = lc;
                        Buffer.BlockCopy(Data, 0, apdu, 1, Lc);
                        apdu[apdu.Length - 1] = le;
                    }
                    else
                    {
                        // Caso 4e
                        var lc1 = (byte)(Lc >> 8 & 0xFF);
                        var lc2 = (byte)(Lc & 0xFF);
                        var le1 = (Le != 65536)
                                      ? (byte)(Le >> 8 & 0xFF)
                                      : (byte)0x00;
                        var le2 = (Le != 65536) ? (byte)(Le & 0xFF) : (byte)0x00;

                        apdu = new byte[5 + Lc];
                        apdu[0] = 0x00;
                        apdu[1] = lc1;
                        apdu[2] = lc2;
                        Buffer.BlockCopy(Data, 0, apdu, 3, Lc);
                        apdu[apdu.Length - 2] = le1;
                        apdu[apdu.Length - 1] = le2;
                    }
                }
            }

            return apdu;
        }

        public EncodingCase GetEncodingCase()
        {
            if (Lc == 0 && Le == 0)
            {
                return EncodingCase.Case1;
            }

            if (Lc == 0 && Le > 0)
            {
                return Le <= 256 ? EncodingCase.Case2s : EncodingCase.Case2e;
            }

            if (Lc > 0 && Le == 0)
            {
                return Lc <= 255 ? EncodingCase.Case3s : EncodingCase.Case3e;
            }

            return Lc <= 255 ? EncodingCase.Case4s : EncodingCase.Case4e;
        }

        //ver iso 7816-3: 12.1
        /**************************************************************
         * Caso     * Command data (Lc)  * Expected response data (Le)*
         **************************************************************
         * 1        * No data            * No data                    *
         * 2        * No data            * Data                       *
         * 3        * Data               * No data                    *
         * 4        * Data               * Data                       *
         **************************************************************
         **
         * Caso 1:  |CLA|INS|P1 |P2 |                                 len = 4
         * Caso 2s: |CLA|INS|P1 |P2 |LE |                             len = 5
         * Caso 3s: |CLA|INS|P1 |P2 |LC |...BODY...|                  len = 6..260
         * Caso 4s: |CLA|INS|P1 |P2 |LC |...BODY...|LE |              len = 7..261
         * Caso 2e: |CLA|INS|P1 |P2 |00 |LE1|LE2|                     len = 7
         * Caso 3e: |CLA|INS|P1 |P2 |00 |LC1|LC2|...BODY...|          len = 8..65542
         * Caso 4e: |CLA|INS|P1 |P2 |00 |LC1|LC2|...BODY...|LE1|LE2|  len =10..65544
         *
         * LE, LE1, LE2 pode ser 0x00.
         * LC não deve ser 0x00 e LC1|LC2 não deve ser 0x00|0x00
         */

        public enum EncodingCase
        {
            Case1,
            Case2s,
            Case3s,
            Case4s,
            Case2e,
            Case3e,
            Case4e
        }

    }
}