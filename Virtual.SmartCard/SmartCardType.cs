namespace Virtual.SmartCard
{
    public enum SmartCardType : uint
    {
        Unknown = 0,
        ISO7816Asynchronous = 1,
        ISO7816Synchronous = 2,
        ISO7816SynchronousType1 = 3,
        ISO7816SynchronousType2 = 4,
        ISO14443TypeA = 5,
        ISO14443TypeB = 6,
        ISO15693 = 7
    }
}