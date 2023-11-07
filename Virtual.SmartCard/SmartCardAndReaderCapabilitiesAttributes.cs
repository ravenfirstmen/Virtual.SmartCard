using System;

namespace Virtual.SmartCard
{
    // Definições na PCSC specification
    public class SmartCardAndReaderCapabilitiesAttributes
    {

        public static UInt32 Define(UInt32 @class, UInt32 tag)
        {
            return ((@class << 16) | 0x0100);
        }

        public enum Classes : uint
        {
            VendorInfo = 1, // Vendor information definitions
            Communications = 2, // Communication definitions
            SmartCardProtocol = 3, // SmartCardProtocol definitions
            PowerManagement = 4, // Power Management definitions
            Security = 5, // Security Assurance definitions
            Mechanical = 6, // Mechanical characteristic definitions
            VendorDefined = 7, // Vendor specific definitions
            SmartCardReaderProtocol = 8, // Interface Device Protocol options,
            ICCState = 9, // ICC State specific definitions
            Performance = 0x7ffe, // performace counters
            System = 0x7fff // System-specific definitions
        }

        // os atributos são definidos "winsmcrd.h" e da forma ((((ULONG)(Class)) << 16) | ((ULONG)(Tag)))

        public enum VendorInfo : uint
        {
            SmartCardReaderName = ((Classes.VendorInfo << 16) | 0x0100),
            SmartCardReaderType = ((Classes.VendorInfo << 16) | 0x0101),
            SmartCardReaderVersion = ((Classes.VendorInfo << 16) | 0x0102),
            SmartCardReaderSerialNumber = ((Classes.VendorInfo << 16) | 0x0103)
        }

        public enum Channel : uint
        {
            Id = ((Classes.Communications << 16) | 0x0110)
        }

        public enum SmartCardProtocol : uint
        {
            AsynchronousProtocolSupport = ((Classes.SmartCardProtocol << 16) | 0x0120),
            DefaultClockRate = ((Classes.SmartCardProtocol << 16) | 0x0121),
            MaxClockRate = ((Classes.SmartCardProtocol << 16) | 0x0122),
            DefaultDataRate = ((Classes.SmartCardProtocol << 16) | 0x0123),
            MaxDataRate = ((Classes.SmartCardProtocol << 16) | 0x0124),
            ReaderInformationFieldSize = ((Classes.SmartCardProtocol << 16) | 0x0125),
            SynchronousProtocolSupport = ((Classes.SmartCardProtocol << 16) | 0x0126)
        }

        public enum PowerManagement : uint
        {
            Support = ((Classes.PowerManagement << 16) | 0x0131)
        }

        public enum Security : uint
        {
            UserToCardAuthenticationDevice = ((Classes.Security << 16) | 0x0140),
            UserAuthenticationInputDevice = ((Classes.Security << 16) | 0x0142)
        }

        public enum Mechanical : uint
        {
            Characteristics = ((Classes.Mechanical << 16) | 0x0150)
        }

        public enum SmartCardReaderProtocol : uint
        {
            Type = ((Classes.SmartCardReaderProtocol << 16) | 0x0201),

            CurrentClockRate = ((Classes.SmartCardReaderProtocol << 16) | 0x0202),
            ClockConversionFactor = ((Classes.SmartCardReaderProtocol << 16) | 0x0203),
            BitRateConversionFactor = ((Classes.SmartCardReaderProtocol << 16) | 0x0204),
            CurrentGuardTime = ((Classes.SmartCardReaderProtocol << 16) | 0x0205),
            CurrentWorkWaitingTime = ((Classes.SmartCardReaderProtocol << 16) | 0x0206),
            CardByteSize = ((Classes.SmartCardReaderProtocol << 16) | 0x0207),
            DeviceByteSize = ((Classes.SmartCardReaderProtocol << 16) | 0x0208),
            CurrentBlockWaitingTime = ((Classes.SmartCardReaderProtocol << 16) | 0x0209),
            CurrentCharacterWaitingTime = ((Classes.SmartCardReaderProtocol << 16) | 0x020a),
            CurrentErrorBlockControlEncoding = ((Classes.SmartCardReaderProtocol << 16) | 0x020b),
            ExtendedBlockWaitingTime = ((Classes.SmartCardReaderProtocol << 16) | 0x020c)
        }

        public enum ICCState : uint
        {
            Presence = ((Classes.ICCState << 16) | 0x0300),
            InterfaceStatus = ((Classes.ICCState << 16) | 0x0301),
            CurrentIOState = ((Classes.ICCState << 16) | 0x0302),
            ATR = ((Classes.ICCState << 16) | 0x0303),
            SmartCardType = ((Classes.ICCState << 16) | 0x0304)
        }

        public enum VendorDefined : uint
        {
            Reset = ((Classes.VendorDefined << 16) | 0xA000),
            Cancel = ((Classes.VendorDefined << 16) | 0xA003),
            AuthenticationRequest = ((Classes.VendorDefined << 16) | 0xA005),
            MaxInput = ((Classes.VendorDefined << 16) | 0xA007)
        }

        public enum System : uint
        {
            Unit = ((Classes.System << 16) | 0x0001),
            InUse = ((Classes.System << 16) | 0x0002),
            FriendlyName = ((Classes.System << 16) | 0x0003),
            SystemName = ((Classes.System << 16) | 0x0004),
            UnicodeFriendlyName = ((Classes.System << 16) | 0x0005),
            UnicodeSystemName = ((Classes.System << 16) | 0x0006),
            SupressT1Request = ((Classes.System << 16) | 0x0007)
        }

        public enum Performance : uint
        {
            NumberTransmitions = ((Classes.Performance << 16) | 0x0001),
            BytesTransmitted = ((Classes.Performance << 16) | 0x0002),
            TransmitionTime = ((Classes.Performance << 16) | 0x0003)
        }

    }
}

