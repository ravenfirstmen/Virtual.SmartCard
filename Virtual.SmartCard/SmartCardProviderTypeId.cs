namespace Virtual.SmartCard
{
    public enum SmartCardProviderTypeId : uint
    {
        Primary = 1,
        CSP = 2,
        KSP = 3,
        Module = 0x80000001
    }
}