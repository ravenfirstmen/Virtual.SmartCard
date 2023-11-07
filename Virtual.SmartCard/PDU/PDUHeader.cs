namespace Virtual.SmartCard.PDU
{
    public class PDUHeader
    {
        public PDUHeader(byte @class, byte instruction, byte p1, byte p2)
        {
            Class = new PDUClass(@class);
            Instruction = instruction;
            P1 = p1;
            P2 = p2;
        }

        public PDUClass Class { get; private set; }
        public byte Instruction { get; private set; }
        public byte P1 { get; private set; } // Parameter 1
        public byte P2 { get; private set; } // Parameter 2

        public byte[] Serialize()
        {
            var apdu = new byte[4];

            apdu[0] = Class.ClassValue;
            apdu[1] = Instruction;
            apdu[2] = P1;
            apdu[3] = P2;

            return apdu;
        }
    }
}