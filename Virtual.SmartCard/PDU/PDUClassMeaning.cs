using System.ComponentModel;

namespace Virtual.SmartCard.PDU
{
    // ver iso 7816-4: secção 5.3.1 (class byte)
    public enum PDUClassMeaning : byte
    {
        [Description("Basic interindustry class/Iso 7816")]
        ISO_0 = 0x00,

        [Description("Reserved for future use")]
        RFU_1 = 0x10,

        [Description("Reserved for future use")]
        RFU_2 = 0x20,

        [Description("Reserved for future use")]
        RFU_3 = 0x30,

        [Description("Reserved for future use")]
        RFU_4 = 0x40,

        [Description("Reserved for future use")]
        RFU_5 = 0x50,

        [Description("Reserved for future use")]
        RFU_6 = 0x60,

        [Description("Reserved for future use")]
        RFU_7 = 0x70,

        [Description("Command response/Iso 7816")]
        ISO_8 = 0x80,

        [Description("Command response/Iso 7816")]
        ISO_9 = 0x90,

        [Description("Command response/Iso 7816")]
        ISO_A = 0xA0,

        [Description("Command response/Iso 7816")]
        ISO_B = 0xB0,

        [Description("Command response/Iso 7816")]
        ISO_C = 0xC0,

        [Description("Proprietary command response")]
        PROPRIETARY_D = 0xD0,

        [Description("Proprietary command response")]
        PROPRIETARY_E = 0xE0,

        [Description("Proprietary command response")]
        PROPRIETARY_F = 0xF0,

        [Description("Protocol and parameters selection")]
        PROTOCOL_PARAMS_SELECTION = 0xFF
    }
}