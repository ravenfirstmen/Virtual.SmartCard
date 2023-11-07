namespace Virtual.SmartCard
{
    public enum AsynchronousSmartCardProtocol : uint
    {
        Undefined = 0x00000000,
        T0  = 0x00000001,
        T1  = 0x00000002,
        RAW = 0x00010000,
        Tx = (T0 | T1)
    }
}

