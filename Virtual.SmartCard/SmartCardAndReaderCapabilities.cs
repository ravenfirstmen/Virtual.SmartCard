using System;

namespace Virtual.SmartCard
{
    public class SmartCardAndReaderCapabilities
    {
        public class VendorInfo
        {
            public VendorInfo(string name, string ifdType, string serialNumber, ushort majorVersion, ushort minorVersion, ushort buildNumber)
                : this(name, ifdType, serialNumber, new Version(majorVersion, minorVersion, buildNumber))
            {
            }

            public VendorInfo(string name, string ifdType, string serialNumber, Version version)
            {
                Name = name;
                Model = ifdType;
                IFDVersion = version;
                SerialNumber = serialNumber;
            }

            public string Name { get; private set; }
            public string Model { get; private set; }
            public Version IFDVersion { get; private set; }
            public string SerialNumber { get; private set; }

            public class Version
            {
                public Version(ushort majorVersion, ushort minorVersion, ushort buildNumber)
                {
                    MajorVersion = majorVersion;
                    MinorVersion = minorVersion;
                    BuildNumber = buildNumber;
                }

                public ushort MajorVersion { get; private set; }
                public ushort MinorVersion { get; private set; }
                public ushort BuildNumber { get; private set; }
            }
        }

        public class System
        {
            public System(int unit, bool inUse, string friendlyName, string systemName, string unicodeFriendlyName, string unicodeSystemName, bool supressT1Request)
            {
                Unit = unit;
                InUse = inUse;
                FriendlyName = friendlyName;
                SystemName = systemName;
                UnicodeFriendlyName = unicodeFriendlyName;
                UnicodeSystemName = unicodeSystemName;
                SupressT1Request = supressT1Request;
            }

            public int Unit { get; private set; }
            public bool InUse { get; private set; }
            public string FriendlyName { get; private set; }
            public string SystemName { get; private set; }
            public string UnicodeFriendlyName { get; private set; }
            public string UnicodeSystemName { get; private set; }
            public bool SupressT1Request { get; private set; }
        }

        public class Communications
        {
            public Communications(int channelNumber, SmartCardReaderType readerType)
            {
                ChannelNumber = channelNumber;
                ReaderType = readerType;
            }

            public int ChannelNumber { get; private set; }
            public SmartCardReaderType ReaderType { get; private set; }
        }

        public class SmartCardProtocol
        {
            public SmartCardProtocol(AsynchronousSmartCardProtocol asynchronousSmartCardProtocol, uint defaultClockRate, uint maxClockRate, uint defaultDataRate, uint maxDataRate, uint readerInformationFieldSize, SynchronousSmartCardProtocol synchronousSmartCardProtocol)
            {
                AsynchronousSmartCardProtocol = asynchronousSmartCardProtocol;
                DefaultClockRate = defaultClockRate;
                MaxClockRate = maxClockRate;
                DefaultDataRate = defaultDataRate;
                MaxDataRate = maxDataRate;
                ReaderInformationFieldSize = readerInformationFieldSize;
                SynchronousSmartCardProtocol = synchronousSmartCardProtocol;
            }

            public AsynchronousSmartCardProtocol AsynchronousSmartCardProtocol { get; private set; }
            public UInt32 DefaultClockRate { get; private set; }
            public UInt32 MaxClockRate { get; private set; }
            public UInt32 DefaultDataRate { get; private set; }
            public UInt32 MaxDataRate { get; private set; }
            public UInt32 ReaderInformationFieldSize { get; private set; }
            public SynchronousSmartCardProtocol SynchronousSmartCardProtocol { get; private set; }
        }

        public class PowerManagement
        {
            public PowerManagement(bool supportPowerDown)
            {
                SupportPowerDown = supportPowerDown;
            }

            public bool SupportPowerDown { get; private set; }
        }

        public class Security
        {
            public Security(UserToCardAuthenticationDevice[] userToCardAuthenticationDevices, UserAuthenticationInputDevice[] userAuthenticationInputDevices)
            {
                UserToCardAuthenticationDevices = userToCardAuthenticationDevices;
                UserAuthenticationInputDevices = userAuthenticationInputDevices;
            }

            public UserToCardAuthenticationDevice[] UserToCardAuthenticationDevices { get; private set; }
            public UserAuthenticationInputDevice[] UserAuthenticationInputDevices { get; private set; }
        }

        public class Mechanical
        {
            public Mechanical(MechanicalCharacteristics[] mechanicalCharacteristics)
            {
                MechanicalCharacteristics = mechanicalCharacteristics;
            }

            public MechanicalCharacteristics[] MechanicalCharacteristics { get; private set; }
        }

        public class ICC
        {
            public ICC(SmartCardState smartCardState, SmartCardInterfaceStatus interfaceStatus, byte[] currentIoState, byte[] atr, SmartCardType smartCardType)
            {
                SmartCardState = smartCardState;
                InterfaceStatus = interfaceStatus;
                CurrentIOState = currentIoState;
                ATR = atr;
                SmartCardType = smartCardType;
            }

            public SmartCardState SmartCardState { get; private set; }
            public SmartCardInterfaceStatus InterfaceStatus { get; private set; }
            public byte[] CurrentIOState { get; private set; } // A VER ....
            public byte[] ATR { get; private set; }
            public SmartCardType SmartCardType { get; private set; }
        }

        public class VendorDefined
        {
            public VendorDefined(byte[] reset, byte[] cancel, byte[] authenticationResquest, byte[] maxInput)
            {
                Reset = reset;
                Cancel = cancel;
                AuthenticationResquest = authenticationResquest;
                MaxInput = maxInput;
            }

            public byte[] Reset { get; private set; }
            public byte[] Cancel { get; private set; }
            public byte[] AuthenticationResquest { get; private set; }
            public byte[] MaxInput { get; private set; }
        }

        public class Performance
        {
            public Performance(uint numberTransmitions, uint bytesTransmitted, uint transmitionTime)
            {
                NumberTransmitions = numberTransmitions;
                BytesTransmitted = bytesTransmitted;
                TransmitionTime = transmitionTime;
            }

            public uint NumberTransmitions { get; private set; }
            public uint BytesTransmitted { get; private set; }
            public uint TransmitionTime { get; private set; }
        }

        public class SmartCardReaderProtocol
        {
            public SmartCardReaderProtocol(SmartCardReaderType type, uint currentClockRate, uint clockConversionFactor, uint bitRateConversionFactor, uint currentGuardTime, uint currentWorkWaitingTime, uint cardByteSize, uint deviceByteSize, uint currentBlockWaitingTime, uint currentCharacterWaitingTime, uint extendedBlockWaitingTime, ErrorBlockControlEncoding errorBlockControlEncoding)
            {
                Type = type;
                CurrentClockRate = currentClockRate;
                ClockConversionFactor = clockConversionFactor;
                BitRateConversionFactor = bitRateConversionFactor;
                CurrentGuardTime = currentGuardTime;
                CurrentWorkWaitingTime = currentWorkWaitingTime;
                CardByteSize = cardByteSize;
                DeviceByteSize = deviceByteSize;
                CurrentBlockWaitingTime = currentBlockWaitingTime;
                CurrentCharacterWaitingTime = currentCharacterWaitingTime;
                ExtendedBlockWaitingTime = extendedBlockWaitingTime;
                ErrorBlockControlEncoding = errorBlockControlEncoding;
            }

            public SmartCardReaderType Type { get; private set; }
            public UInt32 CurrentClockRate { get; private set; }
            public UInt32 ClockConversionFactor { get; private set; }
            public UInt32 BitRateConversionFactor { get; private set; }
            public UInt32 CurrentGuardTime { get; private set; }
            public UInt32 CurrentWorkWaitingTime { get; private set; }
            public UInt32 CardByteSize { get; private set; }
            public UInt32 DeviceByteSize { get; private set; }
            public UInt32 CurrentBlockWaitingTime { get; private set; }
            public UInt32 CurrentCharacterWaitingTime { get; private set; }
            public UInt32 ExtendedBlockWaitingTime { get; private set; }
            public ErrorBlockControlEncoding ErrorBlockControlEncoding { get; private set; }
        }
    }
}