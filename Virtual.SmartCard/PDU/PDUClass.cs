namespace Virtual.SmartCard.PDU
{
    public class PDUClass
    {
        private const byte CLASS_MASK = 0xF0; // 11110000
        private const byte SECURE_MASK = 0xC; // 00001100
        private const byte CHAIN_MASK = 0x10; // 00010000

        public PDUClass(byte value)
        {
            ClassValue = value;

            ClassMeaning = (PDUClassMeaning)(ClassValue & CLASS_MASK);
            MessageFormat = (PDUMessagingFormat)(ClassValue & SECURE_MASK);
            IsLastOrOnlyInChain = (ClassValue & CHAIN_MASK) == 0;

            switch (ClassMeaning)
            {
                case PDUClassMeaning.ISO_0:
                    ChannelNumber = (ClassValue & 0x20 /* 00100000 */>> 3) | (ClassValue & 0x3 /* 0000011 */);
                    break;
                case PDUClassMeaning.ISO_8:
                case PDUClassMeaning.ISO_9:
                case PDUClassMeaning.ISO_A:
                    ChannelNumber = ClassValue & 0x3 /* 0000011 */;
                    break;
                default:
                    ChannelNumber = 0;
                    break;
            }
        }

        public byte ClassValue { get; private set; }

        public PDUClassMeaning ClassMeaning { get; private set; }
        public PDUMessagingFormat MessageFormat { get; private set; }
        public bool IsLastOrOnlyInChain { get; private set; }
        public int ChannelNumber { get; private set; }

        public static byte CreateBasicInterindustryClass()
        {
            return DEFAULT_BASIC_INTERINDUSTRY_CLASS;
        }

        public static byte CreateBasicChainedInterindustryClass()
        {
            return DEFAULT_BASIC_CHAINED_INTERINDUSTRY_CLASS;
        }

        public static byte CreateSecureHeaderNotAuthenticate()
        {
            return DEFAULT_SECURE_HEADER_NOT_AUTHENTICATE_CLASS;
        }

        public static byte CreateSecureHeaderAuthenticate()
        {
            return DEFAULT_SECURE_HEADER_AUTHENTICATE_CLASS;
        }

        private const byte DEFAULT_BASIC_INTERINDUSTRY_CLASS = 0x00; // 00000000 - no chaining, no secure, chanel = 0
        private const byte DEFAULT_BASIC_CHAINED_INTERINDUSTRY_CLASS = 0x10; // 00010000 - chaining, no secure, chanel = 0
        private const byte DEFAULT_SECURE_HEADER_NOT_AUTHENTICATE_CLASS = 0x08; // 00001000 - no chaining, secure, header not authenticate chanel = 0
        private const byte DEFAULT_SECURE_HEADER_AUTHENTICATE_CLASS = 0x0C; // 00001100 - no chaining, secure, header not authenticate chanel = 0
    }
}