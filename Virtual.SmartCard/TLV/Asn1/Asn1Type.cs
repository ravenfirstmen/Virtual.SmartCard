namespace Virtual.SmartCard.TLV.Asn1
{
    public enum Asn1Type : byte
    {
        EndOfContent = 0x00,
        Boolean = 0x01,
        Integer = 0x02,
        BitString = 0x03,
        OctetString = 0x04,
        Null = 0x05,
        ObjectIdentifier = 0x06,
        ObjectDescriptor = 0x07,
        External = 0x08,
        Real = 0x09,
        Enumerated = 0x0a,
        EmbeddedPDV = 0x0b,
        UTF8String = 0x0c,
        RelativeOID = 0x0d,
        RFU_14 = 0x0e,
        RFU_15 = 0x0f,
        Sequence = 0x10,
        SequenceOf = 0x10, // for completeness
        Set = 0x11,
        SetOf = 0x11, // for completeness
        NumericString = 0x12,
        PrintableString = 0x13,
        T61String = 0x14,
        VideotexString = 0x15,
        IA5String = 0x16,
        UtcTime = 0x17,
        GeneralizedTime = 0x18,
        GraphicString = 0x19,
        VisibleString = 0x1a,
        GeneralString = 0x1b,
        UniversalString = 0x1c,
        CharacterString = 0x1d,
        BmpString = 0x1e,
        LongForm = 0x1f
    }
}