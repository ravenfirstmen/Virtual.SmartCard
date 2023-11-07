namespace Virtual.SmartCard
{
    public enum MechanicalCharacteristics : uint
    {
        NoSpecialCharacteristics = 0x00000000,
        CardSwallowingMechanism = 0x00000001,
        CardEjectionMechanism = 0x00000002,
        CardCaptureMechanism = 0x00000004,
        Contactless = 0x00000008,
        RFU = 0xFFFFFFFF
    }
}