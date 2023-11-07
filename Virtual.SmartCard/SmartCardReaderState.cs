namespace Virtual.SmartCard
{
    public enum SmartCardReaderState : uint
    {
        Unknown = 0,
        Absent = 1,
        Present = 2,
        Allowed = 3,
        Powered = 4,
        Negotiable = 5,
        Specific = 6
    }
}