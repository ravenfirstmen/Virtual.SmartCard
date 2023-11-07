namespace Virtual.SmartCard
{
    public enum UserToCardAuthenticationDevice : uint
    {
        NoDevice = 0x00000000,
        RFU = 0x00000001,
        NumericPad = 0x00000002,
        Keyboard = 0x00000004,
        FingerPrintScanner = 0x00000008,
        RetinalScanner = 0x00000010,
        ImageScanner = 0x00000020,
        VoicePrintScanner = 0x00000040,
        DisplayDevice = 0x00000080,
        VendorDefined = 0xFFF00FF
    }
}