namespace Virtual.SmartCard
{
    public enum SmartCardState : uint
    {
        NotPresent = 0,
        PresentNotSwallowed = 1,
        PresenSwallowed = 2,
        Confiscated = 4
    }
}