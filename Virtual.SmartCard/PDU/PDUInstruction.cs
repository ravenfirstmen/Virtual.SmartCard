using System.ComponentModel;

namespace Virtual.SmartCard.PDU
{
    // Iso 7816-4 - 5.3.2 - Instruction Byte
    // os impares são BER-TLV

    public enum PDUInstruction : byte
    {
        [Description("ERASE BINARY")]
        EraseBinary = 0x0E,

        [Description("PERFORM SCQL OPERATION")]
        PerformSCSQLOperation = 0x10,

        [Description("PERFORM TRANSACTION OPERATION")]
        PerformTransactionOperation = 0x12,

        [Description("PERFORM USER OPERATION")]
        PerformUserOperation = 0x14,

        [Description("VERIFY")]
        Verify = 0x20,

        [Description("MANAGE SECURITY ENVIRONMENT")]
        ManageSecurityEnvironment = 0x22,

        [Description("CHANGE REFERENCE DATA")]
        ChangeReferenceData = 0x24,

        [Description("DISABLE VERIFICATION REQUIREMENT")]
        DisableVerificationRequirement = 0x26,

        [Description("ENABLE VERIFICATION REQUIREMENT")]
        EnableVerificationRequirement = 0x28,

        [Description("RESET RETRY COUNTER")]
        ResetRetryCounter = 0x2A,

        [Description("MANAGE CHANNEL")]
        ManageChannel = 0x70,

        [Description("EXTERNAL OU MUTUAL AUTHENTICATE")]
        ExternalOrMutualAuthenticate = 0x82,

        [Description("GET CHALLENGE")]
        GetChallenge = 0x84,

        [Description("GENERAL AUTHENTICATE")]
        GeneralAuthenticate = 0x86,

        [Description("INTERNAL AUTHENTICATE")]
        InternalAuthenticate = 0x88,

        [Description("SEARCH BINARY")]
        SearchBinary = 0xA0,

        [Description("SEARCH RECORD")]
        SearchRecord = 0xA2,

        [Description("SELECT")]
        Select = 0xA4,

        [Description("READ BINARY")]
        ReadBinary = 0xB0,

        [Description("READ RECORD")]
        ReadRecord = 0xB2,

        [Description("GET RESPONSE")]
        GetResponse = 0xC0,

        [Description("ENVELOPE")]
        Envelope = 0xC2,

        [Description("GET DATA")]
        GetData = 0xC4,

        [Description("WRITE BINARY")]
        WriteBinary = 0xD0,

        [Description("WRITE RECORD")]
        WriteRecord = 0xD2,

        [Description("UPDATE BINARY")]
        UpdateBinary = 0xD6,

        [Description("PUT DATA")]
        PutData = 0xDA,

        [Description("UPDATE RECORD")]
        UpdateRecord = 0xDC,

        [Description("APPEND RECORD")]
        AppendRecord = 0xE2,

        [Description("STATUS")]
        Status = 0xF2,


        // Iso 7816 - 8 - Cryptography
        [Description("GENERATE PUBLIC-KEY PAIR")]
        GeneratePublicKeyPair = 0x46,

        [Description("PERFORM SECURITY OPERATION")]
        PerformSecurityOperation = 0x2A,
        // ver as opções que existem: checksum, digital signature, etc... (pontos 5.3 em diante...)


        // Iso 7816 - 9 - Card file management
        [Description("CREATE FILE")]
        CreateFile = 0xE0,

        [Description("DELETE FILE")]
        DeleteFile = 0xE4,

        [Description("DEACTIVATE FILE")]
        DeactivateFile = 0x04,

        [Description("ACTIVATE FILE")]
        ActivateFile = 0x44,

        [Description("TERMINATE DF")]
        TerminateDF = 0xE6,

        [Description("TERMINATE EF")]
        TerminateEF = 0xE8,

        [Description("TERMINATE CARD USAGE")]
        TerminateCardUsage = 0xFE,

    }
}
