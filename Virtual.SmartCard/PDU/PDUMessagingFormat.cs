using System.ComponentModel;

namespace Virtual.SmartCard.PDU
{
    public enum PDUMessagingFormat : byte
    {
        [Description("Plain/no secure messaging")]
        Plain = 0x0,

        [Description("Proprietary messaging")]
        Proprietary = 0x4,

        [Description("Command header not authenticated")]
        CommandHeaderNotAuthenticated = 0x8,

        [Description("Command header authenticated")]
        CommandHeaderAuthenticated = 0xC
    }
}