using System;
using System.Collections.Generic;
using System.Linq;

namespace Virtual.SmartCard
{
    public class SmartCardATR
    {
        public SmartCardATR()
        {
            ATR = new byte[] {};
            DefaultProtocol = AsynchronousSmartCardProtocol.T0;

            _tai = new List<byte?>();
            _tbi = new List<byte?>();
            _tci = new List<byte?>();
            _tdi = new List<byte?>();
        }

        public byte[] ATR { get; private set; }
        public AsynchronousSmartCardProtocol DefaultProtocol { get; private set; }
        public CodeConvention Convention { get; private set; }

        public byte TS { get; private set; }
        public byte T0 { get; private set; }
        public byte? CheckSum { get; private set; }
        public AsynchronousSmartCardProtocol[] SupportedProtocols { get; private set; }

        public int NHistoricalBytes { get; private set; } // K
        public byte[] HistoricalBytes { get; private set; }

        public int NInterfaceBytes { get; private set; }
        public byte?[] TAi { get { return _tai.ToArray(); } }
        public byte?[] TBi { get { return _tbi.ToArray(); } }
        public byte?[] TCi { get { return _tci.ToArray(); } }
        public byte?[] TDi { get { return _tdi.ToArray(); } }
        public bool? CheckSumOk { get; private set; }

        public void Parse(byte[] atrBytes)
        {
            if (atrBytes == null || atrBytes.Length < 2) // 2 = TS + T0 (obrigatórios)
            {
                throw new SmartCardException("ATR inválido");
            }

            TS = atrBytes[0];
            T0 = atrBytes[1];
            
            CopyATR(atrBytes);
            Convention = ParseTS(TS);
            NHistoricalBytes = T0 & NHISTORICAL_BYTES_MASK;
            ParseTXi(atrBytes);
            ObtainHistoricalBytes(atrBytes);
            ObtainSupportedProtocols();
            VerifyCheckSum(atrBytes);
        }

        private void CopyATR(byte[] atrBytes)
        {
            ATR = new byte[atrBytes.Length];
            Buffer.BlockCopy(atrBytes, 0, ATR, 0, atrBytes.Length);
        }

        private CodeConvention ParseTS(byte ts) // sync byte
        {
            if (ts != INVERSE_CONVENTION_BYTE && ts != DIRECT_CONVENTION_BYTE)
            {
                throw new SmartCardException("Byte TS inválido!");
            }

            return ts == INVERSE_CONVENTION_BYTE ? CodeConvention.Inverse : CodeConvention.Direct;
        }

        private void ParseTXi(byte[] atrBytes)
        {
            NInterfaceBytes = 0;

            bool _hasTDi = true;

            int pointer = 1;// 0 = Ts
            for (int n = 0; _hasTDi; n++)
            {
                var td = atrBytes[pointer]; 
                var y = td >> 4 & N_NEXT_BYTES_MASK;
                int hasTAi = (y & BIT_TAi) != 0 ? 1 : 0;
                int hasTBi = (y & BIT_TBi) != 0 ? 1 : 0; // para ignorar conforme iso 7816
                int hasTCi = (y & BIT_TCi) != 0 ? 1 : 0;
                int hasTDi = (y & BIT_TDi) != 0 ? 1 : 0;

                _hasTDi = (y & BIT_TDi) != 0;

                _tai.Add(hasTAi != 0 ? atrBytes[pointer + hasTAi] : (byte?)null);
                _tbi.Add(hasTBi != 0 ? atrBytes[pointer + hasTAi + hasTBi] : (byte?)null);
                _tci.Add(hasTCi != 0 ? atrBytes[pointer + hasTAi + hasTBi + hasTCi] : (byte?)null);
                _tdi.Add(hasTDi != 0 ? atrBytes[pointer + hasTAi + hasTBi + hasTCi + hasTDi] : (byte?)null);

                NInterfaceBytes += (hasTAi + hasTBi + hasTCi + hasTDi);

                pointer += (hasTAi + hasTBi + hasTCi + hasTDi);
            }
        }

        private void ObtainHistoricalBytes(byte[] atrBytes)
        {
            if (NHistoricalBytes != 0)
            {
                HistoricalBytes = new byte[NHistoricalBytes];
                for (int i = 0; i < NHistoricalBytes; i++)
                {
                    HistoricalBytes[i] = atrBytes[1 /* TS */+ 1 /*T0*/ + NInterfaceBytes + i];
                }
            }
            else
            {
                HistoricalBytes = new byte[] {};
            }
        }

        private void VerifyCheckSum(byte[] atrBytes)
        {
            // Alterar para: se algum dos protocolos suportados for != T0, então é que tem checksum!!!!
            if ((NInterfaceBytes + NHistoricalBytes + 1 /*T0*/+ 1 /*TS*/) == (atrBytes.Length - 1)) // tem checksum byte
            {
                int checksum = 0;
                for (int i = 1; i < atrBytes.Length; i++)
                {
                    checksum = checksum ^ atrBytes[i];
                }

                CheckSum = atrBytes[atrBytes.Length - 1];
                CheckSumOk = checksum == 0;
            }
        }

        private void ObtainSupportedProtocols()
        {
            // NOTA: T15 não vai ser devolvido!
            var list = new List<AsynchronousSmartCardProtocol>();

            foreach (var b in _tdi)
            {
                if (b.HasValue)
                {
                    int protocol = b.Value & PROTOCOL_BYTE_MASK;
                    switch (protocol)
                    {
                        case 0x00:
                            if (!list.Exists(p => p == AsynchronousSmartCardProtocol.T0))
                            {
                                list.Add(AsynchronousSmartCardProtocol.T0);
                            }
                            break;
                        case 0x01:
                            if (!list.Exists(p => p == AsynchronousSmartCardProtocol.T1))
                            {
                                list.Add(AsynchronousSmartCardProtocol.T1);
                            }
                            break; 
                        default:
                            break; // nada...
                    }
                }
            }

            // segundo iso 7816 se não forem indicados protocolos é assumido T0
            if (list.Count == 0)
            {
                list.Add(AsynchronousSmartCardProtocol.T0);
            }

            SupportedProtocols = list.ToArray();
            DefaultProtocol = SupportedProtocols[0];
        }

        public enum CodeConvention
        {
            Direct,
            Inverse
        }

        public const int MAX_ATR_SIZE = 1 + 32; //byte TS + 32 Bytes no máximo. Iso 7816-3 ou http://www.cardwerk.com/smartcards/smartcard_standard_ISO7816-3.aspx
        public const byte INVERSE_CONVENTION_BYTE = 0x3F;
        public const byte DIRECT_CONVENTION_BYTE = 0x3B;

        // Masks
        public const byte NHISTORICAL_BYTES_MASK = 0x0F; // 00001111
        public const byte N_NEXT_BYTES_MASK = 0x0F; // 00001111
        public const byte PROTOCOL_BYTE_MASK = 0x0F; // 00001111
        public const byte BIT_TDi = 0x08; // 00001000
        public const byte BIT_TCi = 0x04; // 00000100
        public const byte BIT_TBi = 0x02; // 00000010
        public const byte BIT_TAi = 0x01; // 00000001
        public const byte MAX_TXi_BYTES = 8;

        // helper...
        private IList<byte?> _tai = null;
        private IList<byte?> _tbi = null;
        private IList<byte?> _tci = null;
        private IList<byte?> _tdi = null;

        private static IDictionary<byte, FrequencyClockAndRate> _frequencyClockAndRateTable = new Dictionary
            <byte, FrequencyClockAndRate>
            {
                {0x0, new FrequencyClockAndRate(372, 4, FrequencyClockAndRate.RFU)},
                {0x1, new FrequencyClockAndRate(372, 5, 1)},
                {0x2, new FrequencyClockAndRate(558, 6, 2)},
                {0x3, new FrequencyClockAndRate(744, 8, 4)},
                {0x4, new FrequencyClockAndRate(1116, 12, 8)},
                {0x5, new FrequencyClockAndRate(1488, 16, 16)},
                {0x6, new FrequencyClockAndRate(1860, 20, 32)},
                {
                    0x7,
                    new FrequencyClockAndRate(FrequencyClockAndRate.RFU, FrequencyClockAndRate.NOT_APPLICABLE,
                                              64)
                },
                {
                    0x8,
                    new FrequencyClockAndRate(FrequencyClockAndRate.RFU, FrequencyClockAndRate.NOT_APPLICABLE,
                                              12)
                },
                {0x9, new FrequencyClockAndRate(512, 5, 20)},
                {0xA, new FrequencyClockAndRate(768, 7.5f, FrequencyClockAndRate.RFU)},
                {0xB, new FrequencyClockAndRate(1024, 10, FrequencyClockAndRate.RFU)},
                {0xC, new FrequencyClockAndRate(1536, 15, FrequencyClockAndRate.RFU)},
                {0xD, new FrequencyClockAndRate(2048, 20, FrequencyClockAndRate.RFU)},
                {
                    0xE,
                    new FrequencyClockAndRate(FrequencyClockAndRate.RFU, FrequencyClockAndRate.NOT_APPLICABLE,
                                              FrequencyClockAndRate.RFU)
                },
                {
                    0xF,
                    new FrequencyClockAndRate(FrequencyClockAndRate.RFU, FrequencyClockAndRate.NOT_APPLICABLE,
                                              FrequencyClockAndRate.RFU)
                }
            };

        // convenção: 0 = RFU, -1: Not applicable
        internal class FrequencyClockAndRate
        {
            public FrequencyClockAndRate(int fi, float fmax, int di)
            {
                Fi = fi;
                Fmax = fmax;
                Di = di;
            }

            public int Fi { get; private set; }
            public float Fmax { get; private set; }
            public int Di { get; private set; }

            public const int RFU = 0;
            public const int NOT_APPLICABLE = -1;
        }
    }
}