using System;
using System.Text;

namespace Virtual.SmartCard.PDU
{
    public class APDUResponse : IPDU
    {
        public APDUResponse(byte[] data)
        {
            if (data == null || data.Length < 2)
            {
                throw new SmartCardException("Response from card is inválid!");
            }

            Trailer = new ResponsePDUTrailer(data);

            if (data.Length <= 2)
            {
                Data = null;
            }
            else
            {
                Data = new byte[data.Length - 2];
                Buffer.BlockCopy(data, 0, Data, 0, data.Length - 2);
            }
        }

        public byte[] Data { get; private set; } // Body
        public ResponsePDUTrailer Trailer { get; private set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Data != null)
            {
                foreach (byte b in Data)
                {
                    sb.AppendFormat("{0:X2} ", b);
                }
            }
            sb.AppendFormat("[SW1: {0:X2}] ", Trailer.Status);
            sb.AppendFormat("[SW2: {0:X2}]", Trailer.Qualification);
            sb.AppendFormat(PDUStatusCondition.GetStatusConditionMessage(Trailer.Status, Trailer.Qualification));

            return sb.ToString();
        }
    }

    public class ResponsePDUTrailer
    {
        public ResponsePDUTrailer(byte[] data)
        {
            Status = data[data.Length - 2];
            Qualification = data[data.Length - 1];
        }

        public byte Status { get; set; } // SW1
        public byte Qualification { get; set; } // SW2
    }
}