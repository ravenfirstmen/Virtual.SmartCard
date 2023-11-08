using System;
using System.Collections.Generic;
using System.Linq;
using Virtual.SmartCard;
using Virtual.SmartCard.PDU;
using Virtual.SmartCard.Serializers;
using Virtual.SmartCard.TLV.Simple;
using Virtual.SmartCard.Utils;

namespace Virtual.SmartCards.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //QueryDatabase();
            TestSmartCardReader();

            //TestSerializer();
            //TesteAPDU();
            //TesteSelectCommand();

            //TesteATR();
        }

        private static void TesteAPDU()
        {
            var apdu1 = new APDUCommand(
                PDUClass.CreateBasicInterindustryClass(), PDUInstruction.SearchBinary.Byte(),
                0x01,
                0x02, new byte[] {0x05, 0x06, 0x07, 0x08}, 0x02);

            DumpAPDUHeader(apdu1.Header);
            DumpAPDUBody(apdu1.Body);
        }

        private static void DumpAPDUHeader(PDUHeader header)
        {
            System.Console.WriteLine("---== HEADER ==--");
            System.Console.WriteLine("Meaning: {0}, Format: {1}, LastOrOnlyInChain: {2}, CHannel Number: {3}",
                                     header.Class.ClassMeaning,
                                     header.Class.MessageFormat,
                                     header.Class.IsLastOrOnlyInChain,
                                     header.Class.ChannelNumber
                );
            System.Console.WriteLine("Value: {0}, Instruction:{1} [{2}], P1: {3}, P2: {4}",
                                     header.Class.ClassValue,
                                     header.Instruction,
                                     ((PDUInstruction) header.Instruction).GetDescription(),
                                     header.P1,
                                     header.P2);

            System.Console.Write("Serialized:");
            var bytes = header.Serialize();
            if (bytes == null)
            {
                System.Console.WriteLine("Vazio");
            }
            else
            {
                foreach (byte b in bytes)
                {
                    System.Console.Write("{0:X2} ", b);
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine("---== FIM HEADER ==--");
        }

        public static void DumpAPDUBody(PDUBody body)
        {
            System.Console.WriteLine("---== BODY ==--");
            System.Console.WriteLine("Data length: {0}", body.Lc);
            if (body.Lc > 0)
            {
                System.Console.Write("Data: ");
                foreach (byte b in body.Data)
                {
                    System.Console.Write("{0:X2} ", b);
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine("Expected return length: {0}", body.Le);

            System.Console.Write("Serialized:");
            var bytes = body.Serialize();
            if (bytes == null)
            {
                System.Console.WriteLine("Vazio");
            }
            else
            {
                foreach (byte b in bytes)
                {
                    System.Console.Write("{0:X2} ", b);
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine("---== FIM BODY ==--");
        }

        private static void TestSerializer()
        {
            var bytes = new ByteSerializer().Serialize(' ');
            foreach (var b in bytes)
            {
                System.Console.WriteLine("{0:X2} ", b);
            }

        }

        private static void TestSmartCardReader()
        {
            try
            {
                var query = new SmartCardDatabaseQuery();
                var readers = query.GetAllReaders();

                foreach (string reader in readers)
                {
                    System.Console.WriteLine(reader);
                }
            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }

            // using (var context = SmartCardContext.Create(SmartCardScope.System))
            // {
            //     using (
            //         var reader = SmartCardReader.Create(context, "SCM Microsystems Inc. SCR33x USB Smart Card Reader 0")
            //         )
            //         //using (var reader = SmartCardReader.Create(context, "RICOH Company, Ltd. RICOH SmartCard Reader 0"))
            //         //using (var reader = SmartCardReader.Create(context, "Alcor Micro USB Smart Card Reader 0"))
            //     {
            //         var vi = reader.GetVersionInfoCapabilities();
            //
            //         System.Console.WriteLine("--====--====-- VERSION INFO");
            //         System.Console.WriteLine("SmartCardReaderName: {0}", vi.Name);
            //         System.Console.WriteLine("Model: {0}", vi.Model);
            //         System.Console.WriteLine("Version: {0}.{1} build {2}", vi.IFDVersion.MajorVersion,
            //                                  vi.IFDVersion.MinorVersion, vi.IFDVersion.BuildNumber);
            //         System.Console.WriteLine("Serial number: {0}", vi.SerialNumber);
            //
            //         System.Console.WriteLine("--====--====-- COMMUNICATIONS");
            //         var comm = reader.GetCommunicationCapabilities();
            //         System.Console.WriteLine("Reader type: {0}, Channel number: {1}", comm.ReaderType,
            //                                  comm.ChannelNumber);
            //
            //         System.Console.WriteLine("--====--====-- SMART CARD PROTOCOL");
            //         var protocol = reader.GetSmartCardProtocolCapabilities();
            //         System.Console.WriteLine("Async protocol support: {0}", protocol.AsynchronousSmartCardProtocol);
            //         System.Console.WriteLine("Default clock rate (Khz): {0}", protocol.DefaultClockRate);
            //         System.Console.WriteLine("Max clock rate (Khz): {0}", protocol.MaxClockRate);
            //         System.Console.WriteLine("Default data rate (bps): {0}", protocol.DefaultDataRate);
            //         System.Console.WriteLine("Max data rate (bps): {0}", protocol.MaxDataRate);
            //         System.Console.WriteLine("Reader information field size (bytes): {0}",
            //                                  protocol.ReaderInformationFieldSize);
            //         System.Console.WriteLine("Sync protocol support: {0}", protocol.SynchronousSmartCardProtocol);
            //
            //         System.Console.WriteLine("--====--====-- POWER MANAGEMENT");
            //         var power = reader.GetPowermanagementCapabilities();
            //         System.Console.WriteLine("Power down support: {0}", power.SupportPowerDown);
            //
            //         System.Console.WriteLine("--====--====-- SECURITY");
            //         var security = reader.GetSecurityCapabilities();
            //         foreach (var userToCardAuthenticationDevice in security.UserToCardAuthenticationDevices)
            //         {
            //             System.Console.WriteLine("User to card device: {0}", userToCardAuthenticationDevice);
            //         }
            //         foreach (var userAuthenticationInputDevice in security.UserAuthenticationInputDevices)
            //         {
            //             System.Console.WriteLine("User input device: {0}", userAuthenticationInputDevice);
            //         }
            //
            //         System.Console.WriteLine("--====--====-- MECHANICAL");
            //         var mechanical = reader.GetMechanicalCapabilities();
            //         foreach (var characteristic in mechanical.MechanicalCharacteristics)
            //         {
            //             System.Console.WriteLine("Mechanical characteristic: {0}", characteristic);
            //         }
            //
            //         System.Console.WriteLine("--====--====-- ICC");
            //         var icc = reader.GetICCCapabilities();
            //         System.Console.WriteLine("Smartcard state: {0}", icc.SmartCardState);
            //         System.Console.WriteLine("Smartcard interface status: {0}", icc.InterfaceStatus);
            //         System.Console.WriteLine("Smartcard type: {0}", icc.SmartCardType);
            //         System.Console.Write("ATR: ");
            //         foreach (byte b in icc.ATR)
            //         {
            //             System.Console.Write("{0:X2} ", b);
            //         }
            //         System.Console.WriteLine();
            //
            //         System.Console.WriteLine("--====--====-- VENDOR DEFINED");
            //         var vd = reader.GetVendorDefinedCapabilities();
            //         System.Console.WriteLine();
            //
            //
            //         System.Console.WriteLine("--====--====-- SYSTEM");
            //         var sys = reader.GetSystemCapabilities();
            //         System.Console.WriteLine("System unit: {0}", sys.Unit);
            //         System.Console.WriteLine("In use: {0}", sys.InUse);
            //         System.Console.WriteLine("Friendly name: {0}", sys.FriendlyName);
            //         System.Console.WriteLine("System name: {0}", sys.SystemName);
            //         System.Console.WriteLine("Unicode Friendly name: {0}", sys.UnicodeFriendlyName);
            //         System.Console.WriteLine("Unicode System name: {0}", sys.UnicodeSystemName);
            //         System.Console.WriteLine("Supress T1 request: {0}", sys.SupressT1Request);
            //
            //         System.Console.WriteLine("--====--====-- PERFORMANCE");
            //         var perf = reader.GetPerformanceIndicators();
            //         System.Console.WriteLine("Number transmitions: {0}", perf.NumberTransmitions);
            //         System.Console.WriteLine("Bytes transmitted: {0}", perf.BytesTransmitted);
            //         System.Console.WriteLine("Transmition time: {0}", perf.TransmitionTime);
            //
            //         System.Console.WriteLine("--====--====-- SMART CARD READER");
            //         var scr = reader.GetSmartCardReaderProtocolCapabilities();
            //         System.Console.WriteLine("Smart card reader type: {0}", scr.Type);
            //
            //         System.Console.WriteLine("Current clock rate: {0}", scr.CurrentClockRate);
            //         System.Console.WriteLine("ClockConversionFactor: {0}", scr.ClockConversionFactor);
            //
            //         System.Console.WriteLine("Bit rate conversion factor: {0}", scr.BitRateConversionFactor);
            //         System.Console.WriteLine("Current guard time: {0}", scr.CurrentGuardTime);
            //         System.Console.WriteLine("Current work waiting time: {0}", scr.CurrentWorkWaitingTime);
            //         System.Console.WriteLine("Card byte size: {0}", scr.CardByteSize);
            //         System.Console.WriteLine("Device byte size: {0}", scr.DeviceByteSize);
            //         System.Console.WriteLine("Current block waiting time: {0}", scr.CurrentBlockWaitingTime);
            //         System.Console.WriteLine("Current character waiting time: {0}", scr.CurrentCharacterWaitingTime);
            //         System.Console.WriteLine("Extended block waiting time: {0}", scr.ExtendedBlockWaitingTime);
            //         System.Console.WriteLine("Error block control encoding: {0}", scr.ErrorBlockControlEncoding);
            //     }
            //}
        }

        private static void QueryDatabase()
        {

            using (var context = SmartCardContext.Create(SmartCardScope.User))
            {
                System.Console.WriteLine(context.IsValid());
            }

            try
            {
                var query = new SmartCardDatabaseQuery();
                var allGroups = query.GetAllReadersGroups();

                foreach (string groupName in allGroups)
                {
                    System.Console.WriteLine(groupName);
                }

            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }

            try
            {
                var query = new SmartCardDatabaseQuery();
                var readers = query.GetAllReaders();

                foreach (string reader in readers)
                {
                    System.Console.WriteLine("{0}, type: {1}", reader, query.GetDeviceType(reader));
                }
            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }

            IList<string> insertedCards = null;
            try
            {
                var query = new SmartCardDatabaseQuery();
                insertedCards = query.GetAllRegisteredCards();

                foreach (string card in insertedCards)
                {
                    System.Console.WriteLine(card);
                }
            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }

            if (insertedCards != null)
            {
                try
                {
                    var query = new SmartCardDatabaseQuery();
                    foreach (string insertedCard in insertedCards)
                    {
                        var guids = query.GetCardInterfaces(insertedCard);

                        foreach (var guid in guids)
                        {
                            System.Console.WriteLine(guid);
                        }
                    }
                }
                catch (Exception exp)
                {
                    System.Console.WriteLine(exp.Message);
                }
            }

            if (insertedCards != null)
            {
                foreach (string insertedCard in insertedCards)
                {
                    try
                    {
                        var query = new SmartCardDatabaseQuery();
                        System.Console.WriteLine("Card name: {0}, {1}", insertedCard,
                                                 query.GetCardProviderId(insertedCard));
                    }
                    catch (Exception exp)
                    {
                        System.Console.WriteLine(exp.Message);
                    }
                }
            }


            if (insertedCards != null)
            {
                foreach (string insertedCard in insertedCards)
                {
                    try
                    {
                        var query = new SmartCardDatabaseQuery();
                        System.Console.WriteLine("Card name: {0}, {1}", insertedCard,
                                                 query.GetCardProviderName(insertedCard));
                    }
                    catch (Exception exp)
                    {
                        System.Console.WriteLine(exp.Message);
                    }
                }
            }

            // RICOH Company, Ltd. RICOH SmartCard Reader 0

            try
            {
                using (var context = SmartCardContext.Create(SmartCardScope.System))
                {
                    using (
                        var reader = SmartCardReader.Create(context,
                                                            "SCM Microsystems Inc. SCR33x USB Smart Card Reader 0"))
                        //using (var reader = SmartCardReader.Create(context, "RICOH Company, Ltd. RICOH SmartCard Reader 0"))
                        //using (var reader = SmartCardReader.Create(context, "Alcor Micro USB Smart Card Reader 0"))
                    {
                        var status = reader.GetStatus();

                        System.Console.WriteLine("State: {0}", status.SmartCardReaderState);
                        System.Console.WriteLine("SmartCardProtocol: {0}", status.AsynchronousSmartCardProtocol);

                        System.Console.Write("ATR: ");
                        byte[] atr = status.ATR;
                        foreach (byte b in atr)
                        {
                            System.Console.Write("{0:X2} ", b);
                        }
                        System.Console.WriteLine();

                        System.Console.WriteLine("---------");

                        reader.Reconnect();
                        status = reader.GetStatus();

                        System.Console.WriteLine("State: {0}", status.SmartCardReaderState);
                        System.Console.WriteLine("SmartCardProtocol: {0}", status.AsynchronousSmartCardProtocol);

                        System.Console.Write("ATR: ");
                        atr = status.ATR;
                        foreach (byte b in atr)
                        {
                            System.Console.Write("{0:X2} ", b);
                        }
                        System.Console.WriteLine();

                    }
                }
            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }
        }


        private static void TesteSelectCommand()
        {
            using (var context = SmartCardContext.Create(SmartCardScope.User))
            {
                using (
                    var reader = SmartCardReader.Create(context, "Yubico YubiKey OTP+FIDO+CCID 0")
                    )
                    //using (var reader = SmartCardReader.Create(context, "RICOH Company, Ltd. RICOH SmartCard Reader 0"))
                {
                    var cmd = new APDUCommand(
                        PDUClass.CreateBasicInterindustryClass(),
                        PDUInstruction.Select.Byte(),
                        0x00, 0x00, new byte[] {0x3F, 0x00});

                    //var cmd = new APDUCommand(
                    //    AsynchronousSmartCardProtocol.T1,
                    //    PDUClass.CreateBasicInterindustryClass(),
                    //    PDUInstruction.GetChallenge.Byte(),
                    //    0x00, 0x00, 0x08);

                    var response = reader.Transmit(cmd);

                    System.Console.WriteLine("Response: {0}", response.ToString());
                }
            }
        }

        private static void TesteATR()
        {
            DumpATRBytes(new byte[] { 0x3B, 0x9F, 0x95, 0x81, 0x31, 0xFE, 0x9F, 0x00, 0x65, 0x46, 0x53, 0x05, 0x30, 0x06, 0x71, 0xDF, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0xB6 });
            DumpATRBytes(new byte[] { 0x3B, 0xB9, 0x18, 0x00, 0x00, 0x54, 0x53, 0x4C, 0x65, 0x50, 0x49, 0x56, 0x90, 0x00});
            DumpATRBytes(new byte[] { 0x3B, 0x16, 0x96, 0x41, 0x73, 0x74, 0x72, 0x69, 0x64 });
            DumpATRBytes(new byte[] { 0x3B, 0x9F, 0x95, 0x81, 0x31, 0xF0, 0x9F, 0x00, 0x66, 0x46, 0x53, 0x07, 0x20, 0x20, 0x10, 0x11, 0x22, 0x33, 0x44, 0x81, 0x90, 0x00, 0xE4 });
        }

        private static void DumpATRBytes(byte[] atrBytes)
        {
            var atr = new SmartCardATR();
            atr.Parse(atrBytes);

            System.Console.WriteLine("----=== ATR ===----");

            System.Console.Write("ATR: ");
            DumBytes(atr.ATR);
            System.Console.WriteLine();

            System.Console.WriteLine("Convention: {0}", atr.Convention);
            System.Console.WriteLine("Default protocol: {0}", atr.DefaultProtocol);
            System.Console.Write("Supported protocols: ");
            foreach (var protocol in atr.SupportedProtocols)
            {
                System.Console.Write("{0} ", protocol);
            }
            System.Console.WriteLine();

            System.Console.WriteLine("NHistoricalBytes: {0}", atr.NHistoricalBytes);
            System.Console.WriteLine("TS: {0:X2}", atr.TS);
            System.Console.WriteLine("T0: {0:X2}", atr.T0);
            System.Console.WriteLine("CheckSum: {0:X2}", atr.CheckSum);
            System.Console.Write("Historical bytes: ");
            DumBytes(atr.HistoricalBytes);
            System.Console.WriteLine();
            System.Console.WriteLine("NInterfaceBytes: {0}", atr.NInterfaceBytes);
            System.Console.Write("TA1: ");
            DumBytes(atr.TAi);
            System.Console.WriteLine();
            System.Console.Write("TB1: ");
            DumBytes(atr.TBi);
            System.Console.WriteLine();
            System.Console.Write("TC1: ");
            DumBytes(atr.TCi);
            System.Console.WriteLine();
            System.Console.Write("TD1: ");
            DumBytes(atr.TDi);
            System.Console.WriteLine();

            System.Console.WriteLine("ChecksumOk?: {0}", atr.CheckSumOk);
        }

        private static void DumBytes(byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                System.Console.Write("{0:X2} ", b);
            }
        }

        private static void DumBytes(byte?[] bytes)
        {
            foreach (byte? b in bytes)
            {
                if (b.HasValue)
                {
                    System.Console.Write("{0:X2} ", b);
                }
                else
                {
                    System.Console.Write("   ");
                }
            }
        }


        // TLV
        private static void DumpTlv(BERTLV tlv, int lvl)
        {
            string ident = Level(lvl);
            System.Console.WriteLine("{0}Class class: {1}", ident, tlv.Tag.Class);
            System.Console.WriteLine("{0}Class as: {1}", ident, tlv.Tag.EncodingForm);
            System.Console.WriteLine("{0}Class type: {1}", ident, tlv.Tag.Type);
            System.Console.WriteLine("{0}Class number: {1}", ident, tlv.Tag.TagNumber);

            System.Console.WriteLine("{0}Length form: {1}", ident, tlv.Length.Form);
            System.Console.WriteLine("{0}Length value: {1}", ident, tlv.Length.Value);

            if (tlv.Contents != null)
            {
                System.Console.Write("{0}Contents: ", ident);
                DumpBytes(tlv.Contents);
            }
            if (tlv.Childs != null)
            {
                foreach (var bertlv in tlv.Childs)
                {
                    DumpTlv(bertlv, lvl + 1);
                }
            }
        }

        private static void DumpBytes(byte[] bytes)
        {
            var revBytes = bytes.Reverse();
            foreach (var b in revBytes)
            {
                System.Console.Write("{0:X2} ", b);
            }
            System.Console.WriteLine();
        }

        private static string Level(int lvl)
        {
            string result = String.Empty;
            for (int i = 0; i < lvl; i++)
            {
                result = String.Concat(result, "\t");
            }
            return result;
        }

    }

}