using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Virtual.SmartCard.Winscard
{
    public static class NativeAPI
    {
        public class OperationResult
        {
            public const Int32 SUCCESS = 0x00;
        }

        // ==================================================================================
        // ****** Resource Manager Context Functions

        //LONG WINAPI SCardEstablishContext(
        //  _In_   DWORD dwScope,
        //  _In_   LPCVOID pvReserved1,
        //  _In_   LPCVOID pvReserved2,
        //  _Out_  LPSCARDCONTEXT phContext
        //)

        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardEstablishContext(
            [In] UInt32 scope,
            [In] IntPtr reserved1,
            [In] IntPtr reserved2,
            [In, Out] ref IntPtr context);

        //LONG WINAPI
        //SCardReleaseContext(
        //    _In_      SCARDCONTEXT hContext)
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardReleaseContext(
            [In] IntPtr context);

        //LONG WINAPI SCardIsValidContext(
        //  _In_  SCARDCONTEXT hContext
        //);
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardIsValidContext(
            [In] IntPtr context);

        //LONG WINAPI
        //SCardFreeMemory(
        //    _In_ SCARDCONTEXT hContext,
        //    _In_ LPCVOID pvMem);
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardFreeMemory(
            [In] IntPtr context,
            [In] IntPtr memoryBlock);


        // ==================================================================================
        // ****** Smart Card Tracking Functions

        //LONG WINAPI SCardCancel(
        //  _In_  SCARDCONTEXT hContext
        //);
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardCancel(
            [In] IntPtr context);

        // ==================================================================================
        // ****** Smart Card Database Query Functions

        //LONG WINAPI SCardListReaderGroups(
        //  _In_     SCARDCONTEXT hContext,
        //  _Out_    LPTSTR mszGroups,
        //  _Inout_  LPDWORD pcchGroups
        //);
        [DllImport("winscard.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 SCardListReaderGroups(
            [In] IntPtr context,
            [Out] string groups,
            [In, Out] ref UInt32 namesSize);

        //LONG WINAPI SCardListReaders(
        //  _In_      SCARDCONTEXT hContext,
        //  _In_opt_  LPCTSTR mszGroups,
        //  _Out_     LPTSTR mszReaders,
        //  _Inout_   LPDWORD pcchReaders
        //);
        [DllImport("winscard.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 SCardListReaders(
            [In] IntPtr context,
            [In, Optional] string groups,
            out string readers,
            [In, Out] ref UInt32 namesSize);

        //LONG WINAPI SCardListCards(
        //  _In_      SCARDCONTEXT hContext,
        //  _In_opt_  LPCBYTE pbAtr,
        //  _In_      LPCGUID rgguidInterfaces,
        //  _In_      DWORD cguidInterfaceCount,
        //  _Out_     LPTSTR mszCards,
        //  _Inout_   LPDWORD pcchCards
        //);
        [DllImport("winscard.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 SCardListCards(
            [In] IntPtr context,
            [In, Optional] byte[] atrs,
            [In] MarshalGuid[] interfacesGuid,
            [In] UInt32 interfacesCount,
            [Out] string cards,
            [In, Out] ref UInt32 namesSize);

        //LONG WINAPI SCardListInterfaces(
        //  _In_     SCARDCONTEXT hContext,
        //  _In_     LPCTSTR szCard,
        //  _Out_    LPGUID pguidInterfaces,
        //  _Inout_  LPDWORD pcguidInterfaces
        //);
        [DllImport("winscard.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 SCardListInterfaces(
            [In] IntPtr context,
            [In] string cardName,
            [Out] MarshalGuid[] interfacesGuid,
            [In, Out] ref UInt32 namesSize);

        //LONG WINAPI SCardGetProviderId(
        //  _In_   SCARDCONTEXT hContext,
        //  _In_   LPCTSTR szCard,
        //  _Out_  LPGUID pguidProviderId
        //);
        [DllImport("winscard.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 SCardGetProviderId(
            [In] IntPtr context,
            [In] string cardName,
            [In, Out] ref MarshalGuid providerId);

        //LONG WINAPI SCardGetCardTypeProviderName(
        //  _In_     SCARDCONTEXT hContext,
        //  _In_     LPCTSTR szCardName,
        //  _In_     DWORD dwProviderId,
        //  _Out_    LPTSTR szProvider,
        //  _Inout_  LPDWORD *pcchProvider
        //);
        [DllImport("winscard.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 SCardGetCardTypeProviderName(
            [In] IntPtr context,
            [In] string cardName,
            [In] UInt32 providerId,
            [In, Out] string prividerName,
            [In, Out]  ref UInt32 nameSize);

        //LONG WINAPI SCardGetDeviceTypeId(
        //  _In_     SCARDCONTEXT hContext,
        //  _In_     LPCTSTR szReaderName,
        //  _Inout_  LPDWORD pdwDeviceTypeId
        //);
        [DllImport("winscard.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 SCardGetDeviceTypeId(
            [In] IntPtr context,
            [In] string readerName,
            [In, Out]  ref UInt32 deviceType);


        // ==================================================================================
        // ****** Smart Card and Reader Access Functions

        //LONG WINAPI SCardConnect(
        //  _In_   SCARDCONTEXT hContext,
        //  _In_   LPCTSTR szReader,
        //  _In_   DWORD dwShareMode,
        //  _In_   DWORD dwPreferredProtocols,
        //  _Out_  LPSCARDHANDLE phCard,
        //  _Out_  LPDWORD pdwActiveProtocol
        //);
        [DllImport("winscard.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 SCardConnect(
            [In] IntPtr context,
            [In] string readerName,
            [In] UInt32 sharedMode,
            [In] UInt32 preferedProtocol,
            [In, Out] ref IntPtr cardContext,
            [In, Out] ref UInt32 activeProtocol);


        //LONG WINAPI SCardReconnect(
        //  _In_       SCARDHANDLE hCard,
        //  _In_       DWORD dwShareMode,
        //  _In_       DWORD dwPreferredProtocols,
        //  _In_       DWORD dwInitialization,
        //  _Out_opt_  LPDWORD pdwActiveProtocol
        //);
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardReconnect(
            [In] IntPtr cardContext,
            [In] UInt32 sharedMode,
            [In] UInt32 preferedProtocol,
            [In] UInt32 deinitializationAction,
            [In, Out] ref UInt32 activeProtocol);

        //LONG WINAPI SCardDisconnect(
        //  _In_  SCARDHANDLE hCard,
        //  _In_  DWORD dwDisposition
        //);
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardDisconnect(
            [In] IntPtr cardContext,
            [In] UInt32 dispositionAction);

        //LONG WINAPI SCardStatus(
        //  _In_         SCARDHANDLE hCard,
        //  _Out_        LPTSTR szReaderName,
        //  _Inout_opt_  LPDWORD pcchReaderLen,
        //  _Out_opt_    LPDWORD pdwState,
        //  _Out_opt_    LPDWORD pdwProtocol,
        //  _Out_        LPBYTE pbAtr,
        //  _Inout_opt_  LPDWORD pcbAtrLen
        //);
        [DllImport("winscard.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 SCardStatus(
            [In] IntPtr card,
            [Out] string readerName,
            [In, Out] ref UInt32 readerNameBufferLength,
            [In, Out] ref UInt32 state,
            [In, Out] ref UInt32 protocol,
            [In, Out] byte[] atrBuffer,
            [In, Out] ref UInt32 atrBufferLength);


        //LONG WINAPI
        //SCardTransmit(
        //    _In_        SCARDHANDLE hCard,
        //    _In_        LPCSCARD_IO_REQUEST pioSendPci,
        //    _In_reads_bytes_(cbSendLength) LPCBYTE pbSendBuffer,
        //    _In_        DWORD cbSendLength,
        //    _Inout_opt_ LPSCARD_IO_REQUEST pioRecvPci,
        //    _Out_writes_bytes_(*pcbRecvLength) LPBYTE pbRecvBuffer,
        //    _Inout_     LPDWORD pcbRecvLength);
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardTransmit(
            [In] IntPtr card,
            [In] IntPtr sendSmartCardIORequest,
            [In] byte[] sendBuffer,
            [In] Int32 sendLength,
            [In, Out, Optional] IntPtr receiveSmartCardIORequest, //ref SmartCardIORequest receiveSmartCardIORequest,
            [Out] byte[] recvBuffer,
            [In, Out] ref Int32 recvLength);

        // ==================================================================================
        // ****** Direct Card Access Functions

        //LONG WINAPI SCardControl(
        //  _In_   SCARDHANDLE hCard,
        //  _In_   DWORD dwControlCode,
        //  _In_   LPCVOID lpInBuffer,
        //  _In_   DWORD nInBufferSize,
        //  _Out_  LPVOID lpOutBuffer,
        //  _In_   DWORD nOutBufferSize,
        //  _Out_  LPDWORD lpBytesReturned
        //);
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardControl(
            [In] IntPtr card,
            [In] UInt32 controlCode,
            [In] byte[] sendBuffer,
            [In] UInt32 sendBufferSize,
            [In, Out] byte[] outputBuffer,
            [In] UInt32 outputBufferSize,
            [In, Out] ref UInt32 bytesReturned);

        //LONG WINAPI SCardGetAttrib(
        //  _In_     SCARDHANDLE hCard,
        //  _In_     DWORD dwAttrId,
        //  _Out_    LPBYTE pbAttr,
        //  _Inout_  LPDWORD pcbAttrLen
        //);
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardGetAttrib(
            [In] IntPtr card,
            [In] UInt32 attributeId,
            [In, Out] byte[] attributeBuffer,
            [In, Out] ref UInt32 attributeBufferLength);

        //LONG WINAPI SCardSetAttrib(
        //  _In_  SCARDHANDLE hCard,
        //  _In_  DWORD dwAttrId,
        //  _In_  LPCBYTE pbAttr,
        //  _In_  DWORD cbAttrLen
        //);
        [DllImport("winscard.dll", SetLastError = true)]
        internal static extern Int32 SCardSetAttrib(
            [In] IntPtr card,
            [In] UInt32 attributeId,
            [In] byte[] attributeBuffer,
            [In] UInt32 attributeBufferLength);

        // ===================================================================== ..da-se...
        // Esta tralha em pInvoke.Net (para obter a referencia global SCARD_PCI_T0 ou SCARD_PCI_T1. Ver http://msdn.microsoft.com/en-us/library/windows/desktop/aa379804(v=vs.85).aspx
        [DllImport("kernel32.dll", SetLastError = true)]
        private extern static IntPtr LoadLibrary(string fileName);

        [DllImport("kernel32.dll")]
        private extern static Int32 FreeLibrary(IntPtr handle);

        [DllImport("kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr handle, string procedureName);

        //Get the address of Pci from "Winscard.dll".
        public static IntPtr GetPci(AsynchronousSmartCardProtocol protocol)
        {
            string procedureName = String.Empty;

            switch (protocol)
            {
                case AsynchronousSmartCardProtocol.T0:
                case AsynchronousSmartCardProtocol.Tx:
                    procedureName = "g_rgSCardT0Pci";
                    break;
                case AsynchronousSmartCardProtocol.T1:
                    procedureName = "g_rgSCardT1Pci";
                    break;
                case AsynchronousSmartCardProtocol.RAW:
                    procedureName = "g_rgSCardRawPci";
                    break;
            }

            IntPtr handle = LoadLibrary("Winscard.dll");
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            IntPtr pci = GetProcAddress(handle, procedureName);
            if (pci == IntPtr.Zero)
            {
                FreeLibrary(handle);
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            if (FreeLibrary(handle) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return pci;
        }
    }


    // em winsmcrd.h (SCARD_IO_REQUEST)
    [StructLayout(LayoutKind.Sequential)]
    public struct SmartCardIORequest
    {
        public UInt32 dwProtocol;
        public UInt32 cbPciLength;

        public SmartCardIORequest(uint dwProtocol)
            : this()
        {
            this.dwProtocol = dwProtocol;
            this.cbPciLength = (uint)Marshal.SizeOf(typeof(SmartCardIORequest));
        }
    };

}
