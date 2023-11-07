namespace Virtual.SmartCard
{
    public enum SmartCardReaderType : uint
    {
        Serial = 0x01,
        Parallel = 0x02,
        Keyboard = 0x04,
        SCSI = 0x08,
        IDE = 0x10,
        USB = 0x20,
        PCMCIA = 0x40,
        TPM = 0x80,
        VendorDefined = 0xf0
    }
}