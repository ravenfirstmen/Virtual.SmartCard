using System;
using System.Text;

namespace Virtual.SmartCard.PDU
{
    public class APDUCommand : IPDU
    {
        private const int MAX_APDU_SIZE = 65544;

        protected APDUCommand()
        {
        }

        public APDUCommand(byte @class, byte instruction, byte p1, byte p2)
            : this(@class, instruction, p1, p2, null, 0)
        {
        }

        public APDUCommand(byte @class, byte instruction, byte p1, byte p2, int le)
            : this(@class, instruction, p1, p2, null, le)
        {
        }

        public APDUCommand(byte @class, byte instruction, byte p1, byte p2, byte[] data)
            : this(@class, instruction, p1, p2, data, 0)
        {
        }

        public APDUCommand(byte @class, byte instruction, byte p1, byte p2, byte[] data, int le)
        {
            Header = new PDUHeader(@class, instruction, p1, p2);
            Body = new PDUBody(data, le);
        }

        public PDUHeader Header { get; private set; }
        public PDUBody Body { get; private set; }

        public byte[] Serialize()
        {
            var header = Header.Serialize();
            var body = Body.Serialize();

            var fullApdu = new byte[header.Length + (body != null ? body.Length : 0)];
            Buffer.BlockCopy(header, 0, fullApdu, 0, header.Length);
            if (body != null)
            {
                Buffer.BlockCopy(body, 0, fullApdu, header.Length, body.Length);
            }

            return fullApdu;
        }

        public override string ToString()
        {
            var bytes = Serialize();

            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:X2} ", b);
            }

            return sb.ToString();
        }
    }

}
