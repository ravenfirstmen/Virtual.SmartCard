﻿namespace Virtual.SmartCard.TLV.Simple
{
    public enum BERType : byte
    {
        EndOfContent = 0x00,
        Boolean = 0x01,
        Integer = 0x02,
        BitString = 0x03,
        OctetString = 0x04,
        Null = 0x05,
        ObjectIdentifier = 0x06,
        External = 0x08,
        Enumerated = 0x0a,
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