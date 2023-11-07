using System;
using System.Collections.Generic;
using System.Linq;
using Virtual.SmartCard.Winscard;

namespace Virtual.SmartCard
{
    public class SmartCardDatabaseQuery
    {

        public IList<string> GetAllReadersGroups()
        {
            // isto é mesmo à microsoft....
            // a mesma função... 1ª chamada para saber o número de bytes para receber os nomes, 2ª para receber os nomes....

            using (var context = SmartCardContext.Create(SmartCardScope.System))
            {
                UInt32 groupNamesSize = 0;
                var or = NativeAPI.SCardListReaderGroups(context.GetContext(), null, ref groupNamesSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetAllReadersGroups: Obtain size", or);
                }

                var fullGroupsString = new String(' ', (int)groupNamesSize);
                or = NativeAPI.SCardListReaderGroups(context.GetContext(), fullGroupsString, ref groupNamesSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetAllReadersGroups: Obtain names", or);
                }

                var result = new List<string>();

                string groupName = String.Empty;
                foreach (char c in fullGroupsString)
                {
                    if (c != '\0')
                    {
                        groupName += c;
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(groupName))
                        {
                            result.Add(groupName);
                        }
                        groupName = String.Empty;
                    }
                }

                return result;
            }
        }

        public IList<string> GetAllReaders()
        {
            // isto é mesmo à microsoft....
            // a mesma função... 1ª chamada para saber o número de bytes para receber os nomes, 2ª para receber os nomes....

            using (var context = SmartCardContext.Create(SmartCardScope.System))
            {

                UInt32 namesSize = 0;
                var or = NativeAPI.SCardListReaders(context.GetContext(), SmartCardReadersGroups.All, null, ref namesSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetAllReaders: Obtain size", or);
                }

                var fullReadersString = new String(' ', (int)namesSize);
                or = NativeAPI.SCardListReaders(context.GetContext(), SmartCardReadersGroups.All, fullReadersString,
                                                ref namesSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetAllReaders: Obtain names", or);
                }

                var result = new List<string>();

                string readerName = String.Empty;
                foreach (char c in fullReadersString)
                {
                    if (c != '\0')
                    {
                        readerName += c;
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(readerName))
                        {
                            result.Add(readerName);
                        }
                        readerName = String.Empty;
                    }
                }

                return result;
            }
        }

        public IList<string> GetAllRegisteredCards()
        {
            // isto é mesmo à microsoft....
            // a mesma função... 1ª chamada para saber o número de bytes para receber os nomes, 2ª para receber os nomes....

            using (var context = SmartCardContext.Create(SmartCardScope.System))
            {
                UInt32 cardNamesSize = 0;
                var or = NativeAPI.SCardListCards(IntPtr.Zero, null, null, 0, null, ref cardNamesSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetAllRegisteredCards: Obtain size", or);
                }

                var fullCardNames = new String(' ', (int)cardNamesSize);
                or = NativeAPI.SCardListCards(IntPtr.Zero, null, null, 0, fullCardNames, ref cardNamesSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetAllRegisteredCards: Obtain names", or);
                }

                var result = new List<string>();

                string cardName = String.Empty;
                foreach (char c in fullCardNames)
                {
                    if (c != '\0')
                    {
                        cardName += c;
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(cardName))
                        {
                            result.Add(cardName);
                        }
                        cardName = String.Empty;
                    }
                }

                return result;
            }
        }

        public IList<Guid> GetCardInterfaces(string cardName)
        {
            using (var context = SmartCardContext.Create(SmartCardScope.System))
            {
                UInt32 namesSize = 0;
                var or = NativeAPI.SCardListInterfaces(context.GetContext(), cardName, null, ref namesSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetCardInterfaces: Obtain size", or);
                }

                var guids = new MarshalGuid[namesSize];
                or = NativeAPI.SCardListInterfaces(context.GetContext(), cardName, guids, ref namesSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetCardInterfaces: Obtain names", or);
                }

                return guids.Select(guid => guid.ToGuid()).ToList();
            }
        }

        public Guid GetCardProviderId(string cardName)
        {
            using (var context = SmartCardContext.Create(SmartCardScope.System))
            {
                var guid = new MarshalGuid();
                var or = NativeAPI.SCardGetProviderId(context.GetContext(), cardName, ref guid);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetCardProviderId", or);
                }
                return guid.ToGuid();
            }
        }

        public string GetCardProviderName(string cardName)
        {
            // isto é mesmo à microsoft....
            // a mesma função... 1ª chamada para saber o número de bytes para receber os nomes, 2ª para receber os nomes....

            using (var context = SmartCardContext.Create(SmartCardScope.System))
            {
                UInt32 nameSize = 0;
                var or = NativeAPI.SCardGetCardTypeProviderName(context.GetContext(), cardName, (UInt32)SmartCardProviderTypeId.Module, null, ref nameSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetCardProviderName: Obtain size", or);
                }

                var fullProviderName = new String(' ', (int)nameSize);
                or = NativeAPI.SCardGetCardTypeProviderName(context.GetContext(), cardName, (UInt32)SmartCardProviderTypeId.Module, fullProviderName, ref nameSize);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetCardProviderName: Obtain name", or);
                }

                return fullProviderName;
            }
        }

        public SmartCardReaderType GetDeviceType(string readerName)
        {
            using (var context = SmartCardContext.Create(SmartCardScope.System))
            {
                UInt32 deviceTypeId = 0;
                var or = NativeAPI.SCardGetDeviceTypeId(context.GetContext(), readerName, ref deviceTypeId);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardDatabaseQuery: GetDeviceType", or);
                }

                var rt = SmartCardReaderType.VendorDefined;
                switch (deviceTypeId)
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

                return rt;
            }
        }

    }





}
