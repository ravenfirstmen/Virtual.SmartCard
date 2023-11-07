using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Virtual.SmartCard.PDU;
using Virtual.SmartCard.Serializers;
using Virtual.SmartCard.Utils;
using Virtual.SmartCard.Winscard;

namespace Virtual.SmartCard
{
    public class SmartCardReader : SmartCardResourceAware
    {
  
        private static readonly ILogger Log = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("");

        private readonly IntPtr _smartCardHandle = IntPtr.Zero;
        private readonly string _readerName;

        protected SmartCardReader(IntPtr handle, string readerName)
        {
            _smartCardHandle = handle;
            _readerName = readerName;

            Log.LogInformation("SmartCardReader contructor for reader '{0}'", _readerName);
        }

        public static SmartCardReader Create(SmartCardContext context, string readerName)
        {
            var handle = ConnectToReader(context, readerName);

            Log.LogInformation("Trying to instanciate SmartCardReader for reader '{0}'", readerName);

            return new SmartCardReader(handle, readerName);
        }

        public IntPtr GetCard()
        {
            return _smartCardHandle;
        }


        public SmartCardStatus GetStatus()
        {
            UInt32 smartReaderNameLength = 1024;
            var readerName = new string(' ', (int)smartReaderNameLength);

            UInt32 atrLength = Constants.MAX_ATR_LENGTH;
            var atrBuffer = new byte[Constants.MAX_ATR_LENGTH];
            UInt32 state = (UInt32)SmartCardReaderState.Unknown, protocol = (UInt32)AsynchronousSmartCardProtocol.Tx;

            Log.LogInformation("Trying to get status of smart card in reader '{0}'", _readerName);

            var or = NativeAPI.SCardStatus(GetCard(),
                                           readerName,
                                           ref smartReaderNameLength,
                                           ref state,
                                           ref protocol,
                                           atrBuffer,
                                           ref atrLength);
            if (or != NativeAPI.OperationResult.SUCCESS)
            {
                Log.LogError("SmartCardReader: GetStatus: '{0}'", SmartCardErrors.GetMessageFrom(or));
                throw new SmartCardException("SmartCardReader: GetStatus", or);
            }

            var atrBytes = new byte[atrLength];
            Buffer.BlockCopy(atrBuffer, 0, atrBytes, 0, (int)atrLength);

            return new SmartCardStatus(
                readerName,
                (SmartCardReaderState)state,
                (AsynchronousSmartCardProtocol)protocol,
                atrBytes
                );
        }


        public void Reconnect()
        {
            Log.LogInformation("Trying to reconnect to reader '{0}'", _readerName);
            DoReconnect();
        }

        public APDUResponse Transmit(APDUCommand apdu)
        {
            Log.LogInformation("Trying to transmit APDU '{0}'", apdu.ToString());

            var apduBuffer = apdu.Serialize();

            var responseBuffer = new byte[apdu.Body.ExpectecResponseLength];
            int receiveLength = responseBuffer.Length;

            int returnStatus = NativeAPI.OperationResult.SUCCESS;
            bool cmdSent = false;
            do
            {
                returnStatus = NativeAPI.SCardTransmit(
                    GetCard(),
                    NativeAPI.GetPci(AsynchronousSmartCardProtocol.T1),
                    apduBuffer,
                    apduBuffer.Length,
                    IntPtr.Zero,
                    responseBuffer,
                    ref receiveLength);

                if (SmartCardErrors.GetCodeFromErrorCode(returnStatus) == SmartCardErrors.Codes.InsuficientBuffer &&
                    responseBuffer.Length < receiveLength)
                {
                    Log.LogInformation("Buffer too small: Retrying transmitting. New buffer size {0} bytes", receiveLength);
                    responseBuffer = new byte[receiveLength];
                }
                else
                {
                    cmdSent = true;
                }
            } while (cmdSent == false);

            if (returnStatus != NativeAPI.OperationResult.SUCCESS)
            {
                Log.LogInformation("SmartCardReader: Transmit: '{0}'", SmartCardErrors.GetMessageFrom(returnStatus));
                throw new SmartCardException("SmartCardReader: Transmit", returnStatus);
            }

            Log.LogInformation("Transmited {0} bytes, received {1} bytes, expected {2} bytes", apduBuffer.Length, receiveLength, responseBuffer.Length);

            var responseApduBytes = new byte[receiveLength];
            Buffer.BlockCopy(responseBuffer, 0, responseApduBytes, 0, receiveLength);

            return new APDUResponse(responseApduBytes);
        }

        #region Attributes

        #region Get

        public SmartCardAndReaderCapabilities.VendorInfo GetVersionInfoCapabilities()
        {
            Log.LogInformation("Trying to obtain version info capabilities...");

            var name = SmartCardUtils.GetAnsiStringAttribute(GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.VendorInfo.SmartCardReaderName), "N/A");
            var type = SmartCardUtils.GetAnsiStringAttribute(GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.VendorInfo.SmartCardReaderType), "N/A");
            var version = GetIDFVersion(GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.VendorInfo.SmartCardReaderVersion));
            var sn = SmartCardUtils.GetAnsiStringAttribute(GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.VendorInfo.SmartCardReaderSerialNumber), "N/A");

            return new SmartCardAndReaderCapabilities.VendorInfo(name, type, sn, version);
        }

        public SmartCardAndReaderCapabilities.Communications GetCommunicationCapabilities()
        {
            Log.LogInformation("Trying to obtain communication capabilities...");

            var data = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.Channel.Id);
            if (data != null)
            {
                var rType = (uint)(data[3] << 8 | data[2]);
                var channelNumber = (data[1] << 8 | data[0]);

                return new SmartCardAndReaderCapabilities.Communications(channelNumber,
                                                                        rType < (uint)SmartCardReaderType.VendorDefined
                                                                            ? (SmartCardReaderType)rType
                                                                            : SmartCardReaderType.VendorDefined);
            }

            return null;
        }

        public SmartCardAndReaderCapabilities.SmartCardProtocol GetSmartCardProtocolCapabilities()
        {
            Log.LogInformation("Trying to obtain protocol capabilities...");

            var asyncProtocol = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardProtocol.AsynchronousProtocolSupport);
            var defaultClockRate = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardProtocol.DefaultClockRate);
            var maxClockRate = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardProtocol.MaxClockRate);
            var defaultDataRate = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardProtocol.DefaultDataRate);
            var maxDataRate = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardProtocol.MaxDataRate);
            var asyncbyteSize = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardProtocol.ReaderInformationFieldSize);
            var synchronousProtocol = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardProtocol.SynchronousProtocolSupport);

            var ap = AsynchronousSmartCardProtocol.Undefined;
            if (asyncProtocol != null)
            {
                switch ((uint)(asyncProtocol[1] << 8 | asyncProtocol[0]))
                {
                    case (uint)AsynchronousSmartCardProtocol.RAW:
                        ap = AsynchronousSmartCardProtocol.RAW;
                        break;
                    case (uint)AsynchronousSmartCardProtocol.T0:
                        ap = AsynchronousSmartCardProtocol.T0;
                        break;
                    case (uint)AsynchronousSmartCardProtocol.T1:
                        ap = AsynchronousSmartCardProtocol.T1;
                        break;
                    case (uint)AsynchronousSmartCardProtocol.Tx:
                        ap = AsynchronousSmartCardProtocol.Tx;
                        break;
                    default:
                        ap = AsynchronousSmartCardProtocol.Undefined;
                        break;
                }
            }
            var sp = SynchronousSmartCardProtocol.Undefined;
            if (synchronousProtocol != null)
            {
                switch ((uint)(synchronousProtocol[1] << 8 | synchronousProtocol[0]))
                {
                    case (uint)SynchronousSmartCardProtocol.Wire2:
                        sp = SynchronousSmartCardProtocol.Wire2;
                        break;
                    case (uint)SynchronousSmartCardProtocol.Wire3:
                        sp = SynchronousSmartCardProtocol.Wire3;
                        break;
                    case (uint)SynchronousSmartCardProtocol.I2CBus:
                        sp = SynchronousSmartCardProtocol.I2CBus;
                        break;
                    default:
                        sp = SynchronousSmartCardProtocol.Undefined;
                        break;
                }
            }
            return new SmartCardAndReaderCapabilities.SmartCardProtocol(
                ap,
                BytesConverter.BytesToUInt32(defaultClockRate),
                BytesConverter.BytesToUInt32(maxClockRate),
                BytesConverter.BytesToUInt32(defaultDataRate),
                BytesConverter.BytesToUInt32(maxDataRate),
                BytesConverter.BytesToUInt32(asyncbyteSize),
                sp
                );
        }

        public SmartCardAndReaderCapabilities.PowerManagement GetPowermanagementCapabilities()
        {
            Log.LogInformation("Trying to obtain power management capabilities...");

            var pm = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.PowerManagement.Support);
            var val = BytesConverter.BytesToUInt32(pm);

            return new SmartCardAndReaderCapabilities.PowerManagement(val != 0);
        }

        public SmartCardAndReaderCapabilities.Security GetSecurityCapabilities()
        {
            Log.LogInformation("Trying to obtain security capabilities...");

            var suc = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.Security.UserToCardAuthenticationDevice);
            var sucVal = BytesConverter.BytesToUInt32(suc);

            var sucDevices = new List<UserToCardAuthenticationDevice>();
            if (sucVal == (uint)UserToCardAuthenticationDevice.NoDevice)
            {
                sucDevices.Add(UserToCardAuthenticationDevice.NoDevice);
            }
            if ((sucVal & (uint)UserToCardAuthenticationDevice.NumericPad) == (uint)UserToCardAuthenticationDevice.NumericPad)
            {
                sucDevices.Add(UserToCardAuthenticationDevice.NumericPad);
            }
            if ((sucVal & (uint)UserToCardAuthenticationDevice.Keyboard) == (uint)UserToCardAuthenticationDevice.Keyboard)
            {
                sucDevices.Add(UserToCardAuthenticationDevice.Keyboard);
            }
            if ((sucVal & (uint)UserToCardAuthenticationDevice.FingerPrintScanner) == (uint)UserToCardAuthenticationDevice.FingerPrintScanner)
            {
                sucDevices.Add(UserToCardAuthenticationDevice.FingerPrintScanner);
            }
            if ((sucVal & (uint)UserToCardAuthenticationDevice.RetinalScanner) == (uint)UserToCardAuthenticationDevice.RetinalScanner)
            {
                sucDevices.Add(UserToCardAuthenticationDevice.RetinalScanner);
            }
            if ((sucVal & (uint)UserToCardAuthenticationDevice.ImageScanner) == (uint)UserToCardAuthenticationDevice.ImageScanner)
            {
                sucDevices.Add(UserToCardAuthenticationDevice.ImageScanner);
            }
            if ((sucVal & (uint)UserToCardAuthenticationDevice.VoicePrintScanner) == (uint)UserToCardAuthenticationDevice.VoicePrintScanner)
            {
                sucDevices.Add(UserToCardAuthenticationDevice.VoicePrintScanner);
            }
            if ((sucVal & (uint)UserToCardAuthenticationDevice.DisplayDevice) == (uint)UserToCardAuthenticationDevice.DisplayDevice)
            {
                sucDevices.Add(UserToCardAuthenticationDevice.DisplayDevice);
            }
            if ((sucVal & (uint)UserToCardAuthenticationDevice.VendorDefined) ==
                (uint)UserToCardAuthenticationDevice.VendorDefined)
            {
                sucDevices.Add(UserToCardAuthenticationDevice.VendorDefined);
            }

            var sid = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.Security.UserAuthenticationInputDevice);
            var sidVal = BytesConverter.BytesToUInt32(sid);

            var sidDevices = new List<UserAuthenticationInputDevice>();
            if (sidVal == (uint)UserAuthenticationInputDevice.NoDevice)
            {
                sidDevices.Add(UserAuthenticationInputDevice.NoDevice);
            }
            if ((sidVal & (uint)UserAuthenticationInputDevice.NumericPad) == (uint)UserAuthenticationInputDevice.NumericPad)
            {
                sidDevices.Add(UserAuthenticationInputDevice.NumericPad);
            }
            if ((sidVal & (uint)UserAuthenticationInputDevice.Keyboard) == (uint)UserAuthenticationInputDevice.Keyboard)
            {
                sidDevices.Add(UserAuthenticationInputDevice.Keyboard);
            }
            if ((sidVal & (uint)UserAuthenticationInputDevice.FingerPrintScanner) == (uint)UserAuthenticationInputDevice.FingerPrintScanner)
            {
                sidDevices.Add(UserAuthenticationInputDevice.FingerPrintScanner);
            }
            if ((sidVal & (uint)UserAuthenticationInputDevice.RetinalScanner) == (uint)UserAuthenticationInputDevice.RetinalScanner)
            {
                sidDevices.Add(UserAuthenticationInputDevice.RetinalScanner);
            }
            if ((sidVal & (uint)UserAuthenticationInputDevice.ImageScanner) == (uint)UserAuthenticationInputDevice.ImageScanner)
            {
                sidDevices.Add(UserAuthenticationInputDevice.ImageScanner);
            }
            if ((sidVal & (uint)UserAuthenticationInputDevice.VoicePrintScanner) == (uint)UserAuthenticationInputDevice.VoicePrintScanner)
            {
                sidDevices.Add(UserAuthenticationInputDevice.VoicePrintScanner);
            }
            if ((sidVal & (uint)UserAuthenticationInputDevice.DisplayDevice) == (uint)UserAuthenticationInputDevice.DisplayDevice)
            {
                sidDevices.Add(UserAuthenticationInputDevice.DisplayDevice);
            }
            if ((sidVal & (uint)UserAuthenticationInputDevice.UnencryptedInput) == (uint)UserAuthenticationInputDevice.UnencryptedInput)
            {
                sidDevices.Add(UserAuthenticationInputDevice.UnencryptedInput);
            }
            if ((sidVal & (uint)UserAuthenticationInputDevice.VendorDefined) ==
                (uint)UserAuthenticationInputDevice.VendorDefined)
            {
                sidDevices.Add(UserAuthenticationInputDevice.VendorDefined);
            }


            return new SmartCardAndReaderCapabilities.Security(sucDevices.ToArray(), sidDevices.ToArray());
        }

        public SmartCardAndReaderCapabilities.Mechanical GetMechanicalCapabilities()
        {
            Log.LogInformation("Trying to obtain mechanical capabilities...");

            var mc = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.Mechanical.Characteristics);
            var mcVal = BytesConverter.BytesToUInt32(mc);

            var characteristics = new List<MechanicalCharacteristics>();
            if (mcVal == (uint)MechanicalCharacteristics.NoSpecialCharacteristics)
            {
                characteristics.Add(MechanicalCharacteristics.NoSpecialCharacteristics);
            }

            if ((mcVal & (uint)MechanicalCharacteristics.CardSwallowingMechanism) == (uint)MechanicalCharacteristics.CardSwallowingMechanism)
            {
                characteristics.Add(MechanicalCharacteristics.CardSwallowingMechanism);
            }
            if ((mcVal & (uint)MechanicalCharacteristics.CardEjectionMechanism) == (uint)MechanicalCharacteristics.CardEjectionMechanism)
            {
                characteristics.Add(MechanicalCharacteristics.CardEjectionMechanism);
            }
            if ((mcVal & (uint)MechanicalCharacteristics.CardCaptureMechanism) == (uint)MechanicalCharacteristics.CardCaptureMechanism)
            {
                characteristics.Add(MechanicalCharacteristics.CardCaptureMechanism);
            }
            if ((mcVal & (uint)MechanicalCharacteristics.Contactless) == (uint)MechanicalCharacteristics.Contactless)
            {
                characteristics.Add(MechanicalCharacteristics.Contactless);
            }

            return new SmartCardAndReaderCapabilities.Mechanical(characteristics.ToArray());
        }

        public SmartCardAndReaderCapabilities.ICC GetICCCapabilities()
        {
            Log.LogInformation("Trying to obtain ICC capabilities...");

            var presence = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.ICCState.Presence);
            var interfaceStatus = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.ICCState.InterfaceStatus);
            var currentIOState = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.ICCState.CurrentIOState);
            //byte[] currentIOState = null;
            var atr = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.ICCState.ATR);
            var smartCardType = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.ICCState.SmartCardType);

            var smcState = SmartCardState.NotPresent;
            switch (BytesConverter.BytesToUShort(presence))
            {
                case (uint)SmartCardState.PresentNotSwallowed:
                    smcState = SmartCardState.PresentNotSwallowed;
                    break;
                case (uint)SmartCardState.PresenSwallowed:
                    smcState = SmartCardState.PresenSwallowed;
                    break;
                case (uint)SmartCardState.Confiscated:
                    smcState = SmartCardState.Confiscated;
                    break;
                default: smcState = SmartCardState.NotPresent;
                    break;
            }

            var smcType = SmartCardType.Unknown;
            switch (BytesConverter.BytesToUShort(smartCardType))
            {
                case (uint)SmartCardType.ISO7816Asynchronous:
                    smcType = SmartCardType.ISO7816Asynchronous;
                    break;
                case (uint)SmartCardType.ISO7816Synchronous:
                    smcType = SmartCardType.ISO7816Synchronous;
                    break;
                case (uint)SmartCardType.ISO7816SynchronousType1:
                    smcType = SmartCardType.ISO7816SynchronousType1;
                    break;
                case (uint)SmartCardType.ISO7816SynchronousType2:
                    smcType = SmartCardType.ISO7816SynchronousType2;
                    break;
                case (uint)SmartCardType.ISO14443TypeA:
                    smcType = SmartCardType.ISO14443TypeA;
                    break;
                case (uint)SmartCardType.ISO14443TypeB:
                    smcType = SmartCardType.ISO14443TypeB;
                    break;
                case (uint)SmartCardType.ISO15693:
                    smcType = SmartCardType.ISO15693;
                    break;
                default: smcType = SmartCardType.Unknown;
                    break;
            }

            return new SmartCardAndReaderCapabilities.ICC(
                smcState,
                BytesConverter.BytesToUShort(interfaceStatus) == 0x00 ? SmartCardInterfaceStatus.Inactive : SmartCardInterfaceStatus.Active,
                currentIOState,
                atr,
                smcType
                );
        }

        public SmartCardAndReaderCapabilities.VendorDefined GetVendorDefinedCapabilities()
        {
            Log.LogInformation("Trying to obtain vendor defined capabilities...");

            return new SmartCardAndReaderCapabilities.VendorDefined(
                GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.VendorDefined.Reset),
                GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.VendorDefined.Cancel),
                GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.VendorDefined.AuthenticationRequest),
                GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.VendorDefined.MaxInput)
                );
        }

        public SmartCardAndReaderCapabilities.System GetSystemCapabilities()
        {
            Log.LogInformation("Trying to obtain system capabilities...");

            var unit = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.System.Unit);
            var inUse = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.System.InUse);
            var friendlyName = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.System.FriendlyName);
            var systemName = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.System.SystemName);
            var unicodeFriendlyName = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.System.UnicodeFriendlyName);
            var unicodeSystemName = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.System.UnicodeSystemName);
            var t1 = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.System.SupressT1Request);


            return new SmartCardAndReaderCapabilities.System(
                (int)BytesConverter.BytesToUInt32(unit),
                BytesConverter.BuildBoolFrom1Byte(inUse),
                SmartCardUtils.GetAnsiStringAttribute(friendlyName, "N/A"),
                SmartCardUtils.GetAnsiStringAttribute(systemName, "N/A"),
                SmartCardUtils.GetUnicodeStringAttribute(unicodeFriendlyName, "N/A"),
                SmartCardUtils.GetUnicodeStringAttribute(unicodeSystemName, "N/A"),
                BytesConverter.BuildBoolFrom1Byte(t1)
                );
        }

        public SmartCardAndReaderCapabilities.Performance GetPerformanceIndicators()
        {
            Log.LogInformation("Trying to obtain performance indicators...");

            return new SmartCardAndReaderCapabilities.Performance(
                BytesConverter.BytesToUInt32(GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.Performance.NumberTransmitions)),
                BytesConverter.BytesToUInt32(GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.Performance.BytesTransmitted)),
                BytesConverter.BytesToUInt32(GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.Performance.TransmitionTime))
                );
        }

        public SmartCardAndReaderCapabilities.SmartCardReaderProtocol GetSmartCardReaderProtocolCapabilities()
        {
            Log.LogInformation("Trying to obtain smart card reader protocol capabilities...");

            var type = GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.Type);
            var currentClockRate =
                GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.CurrentClockRate);
            var clockConversionFactor =
                GetAttribute(
                    (UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.ClockConversionFactor);
            var bitRateConversionFactor =
                GetAttribute(
                    (UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.BitRateConversionFactor);
            var currentGuardTime =
                GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.CurrentGuardTime);
            var currentWorkWaitingTime =
                GetAttribute(
                    (UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.CurrentWorkWaitingTime);
            var cardByteSize =
                GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.CardByteSize);
            var deviceByteSize =
                GetAttribute((UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.DeviceByteSize);
            var currentBlockWaitingTime =
                GetAttribute(
                    (UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.CurrentBlockWaitingTime);
            var currentCharacterWaitingTime =
                GetAttribute(
                    (UInt32)
                    SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.CurrentCharacterWaitingTime);
            var extendedBlockWaitingTime =
                GetAttribute(
                    (UInt32)SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.ExtendedBlockWaitingTime);
            var errorBlockControlEncoding =
                GetAttribute(
                    (UInt32)
                    SmartCardAndReaderCapabilitiesAttributes.SmartCardReaderProtocol.CurrentErrorBlockControlEncoding);

            var rt = SmartCardReaderType.VendorDefined;
            if (type != null)
            {
                switch ((uint)(type[1] << 8 | type[0]))
                {
                    case (uint)SmartCardReaderType.Serial:
                        rt = SmartCardReaderType.Serial;
                        break;
                    case (uint)SmartCardReaderType.Parallel:
                        rt = SmartCardReaderType.Parallel;
                        break;
                    case (uint)SmartCardReaderType.Keyboard:
                        rt = SmartCardReaderType.Keyboard;
                        break;
                    case (uint)SmartCardReaderType.SCSI:
                        rt = SmartCardReaderType.SCSI;
                        break;
                    case (uint)SmartCardReaderType.IDE:
                        rt = SmartCardReaderType.IDE;
                        break;
                    case (uint)SmartCardReaderType.USB:
                        rt = SmartCardReaderType.USB;
                        break;
                    case (uint)SmartCardReaderType.PCMCIA:
                        rt = SmartCardReaderType.PCMCIA;
                        break;
                    case (uint)SmartCardReaderType.TPM:
                        rt = SmartCardReaderType.TPM;
                        break;
                    default:
                        rt = SmartCardReaderType.VendorDefined;
                        break;
                }
            }

            var ece = ErrorBlockControlEncoding.LRC;
            if (errorBlockControlEncoding != null)
            {
                switch ((uint)(errorBlockControlEncoding[1] << 8 | errorBlockControlEncoding[0]))
                {
                    case (uint)ErrorBlockControlEncoding.LRC:
                        ece = ErrorBlockControlEncoding.LRC;
                        break;
                    case (uint)ErrorBlockControlEncoding.CRC:
                        ece = ErrorBlockControlEncoding.CRC;
                        break;
                    default:
                        ece = ErrorBlockControlEncoding.LRC;
                        break;
                }
            }

            return new SmartCardAndReaderCapabilities.SmartCardReaderProtocol(
                rt,
                BytesConverter.BytesToUInt32(currentClockRate),
                BytesConverter.BytesToUInt32(clockConversionFactor),
                BytesConverter.BytesToUInt32(bitRateConversionFactor),
                BytesConverter.BytesToUInt32(currentGuardTime),
                BytesConverter.BytesToUInt32(currentWorkWaitingTime),
                BytesConverter.BytesToUInt32(cardByteSize),
                BytesConverter.BytesToUInt32(deviceByteSize),
                BytesConverter.BytesToUInt32(currentBlockWaitingTime),
                BytesConverter.BytesToUInt32(currentCharacterWaitingTime),
                BytesConverter.BytesToUInt32(extendedBlockWaitingTime),
                ece
                );
        }

        #endregion

        #region Set

        public void SetAttribute(UInt32 attributeId, bool value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, byte value)
        {
            SetAttribute(attributeId, new byte[] { value });
        }
        public void SetAttribute(UInt32 attributeId, char value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, char[] value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, short value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, ushort value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, int value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, uint value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, long value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, ulong value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, string value)
        {
            SetAttribute(attributeId, new ByteSerializer().Serialize(value));
        }
        public void SetAttribute(UInt32 attributeId, byte[] buffer)
        {
            var bufferLength = (UInt32)buffer.Length;
            var or = NativeAPI.SCardSetAttrib(GetCard(), attributeId, buffer, bufferLength);
            if (or != NativeAPI.OperationResult.SUCCESS)
            {
                throw new SmartCardException("SmartCardReader: SetAttribute", or);
            }
        }

        #endregion

        #endregion

        #region Helpers

        // verificar se se pode optimiar apenas com 1 chamada: tip... FreeMemory
        private byte[] GetAttribute(UInt32 attribute)
        {
            Log.LogInformation("Trying obtain size for attribute '{0:X8}'", attribute);
            UInt32 bufferLength = 0;
            var or = NativeAPI.SCardGetAttrib(GetCard(), attribute, null, ref bufferLength);
            if (or != NativeAPI.OperationResult.SUCCESS)
            {
                Log.LogWarning("Cannot obtain size for attribute '{0:X8}, error code {1:X8}'", attribute, or);
                //throw new SmartCardException("SmartCardReader: GetAttribute: Obtain size", or);
                return null;
            }

            Log.LogInformation("Trying obtain value for attribute '{0:X8}'", attribute);
            var attributeBuffer = new byte[bufferLength];
            or = NativeAPI.SCardGetAttrib(GetCard(), attribute, attributeBuffer, ref bufferLength);
            if (or != NativeAPI.OperationResult.SUCCESS)
            {
                Log.LogInformation("Cannot obtain value for attribute '{0:X8}, error code {1:X8}'", attribute, or);
                //throw new SmartCardException("SmartCardReader: GetAttribute: Obtain name", or);
                return null;
            }

            return attributeBuffer;
        }

        private SmartCardAndReaderCapabilities.VendorInfo.Version GetIDFVersion(byte[] data)
        {
            if (data == null) return new SmartCardAndReaderCapabilities.VendorInfo.Version(0, 0, 0);

            //Vendor-supplied interface device version (DWORD in the form 0xMMmmbbbb where MM = major version, mm = minor version, and bbbb = build number).
            return new SmartCardAndReaderCapabilities.VendorInfo.Version(data[3], data[2], (ushort)(data[1] << 8 | data[0]));
        }



        #endregion

        #region Resources

        private static readonly object Section = new object();

        private static IntPtr ConnectToReader(SmartCardContext context, string readerName)
        {
            lock (Section)
            {
                IntPtr handle = IntPtr.Zero;

                var activeProtocol = (UInt32)AsynchronousSmartCardProtocol.Tx;
                var or = NativeAPI.SCardConnect(
                    context.GetContext(),
                    readerName,
                    (UInt32)SmartCardShare.Shared,
                    (UInt32)AsynchronousSmartCardProtocol.Tx,
                    ref handle,
                    ref activeProtocol);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardReader: InitializeResources", or);
                }

                return handle;
            }
        }

        private void DoReconnect()
        {
            var activeProtocol = (UInt32)AsynchronousSmartCardProtocol.Tx;
            var or = NativeAPI.SCardReconnect(
                GetCard(),
                (UInt32)SmartCardShare.Exclusive,
                (UInt32)AsynchronousSmartCardProtocol.Tx,
                (UInt32)SmartCardDispositionAction.Leave,
                ref activeProtocol);
            if (or != NativeAPI.OperationResult.SUCCESS)
            {
                throw new SmartCardException("SmartCardReader: InitializeResources", or);
            }
        }

        protected override void DisposeResources()
        {
            if (_smartCardHandle != IntPtr.Zero)
            {
                var or = NativeAPI.SCardDisconnect(_smartCardHandle, (uint)SmartCardDispositionAction.Reset);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardReader: DisposeResources", or);
                }
            }
        }

        #endregion
    }
}