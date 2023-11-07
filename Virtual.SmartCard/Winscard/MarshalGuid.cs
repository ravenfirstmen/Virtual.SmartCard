using System;
using System.Runtime.InteropServices;

namespace Virtual.SmartCard.Winscard
{
    // http://msdn.microsoft.com/en-us/library/windows/desktop/aa373931(v=vs.85).aspx
    // http://pinvoke.net/default.aspx/Structures.GUID
    //typedef struct _GUID {
    //  DWORD Data1;
    //  WORD  Data2;
    //  WORD  Data3;
    //  BYTE  Data4[8];
    //} 
    // só consegui, customizando...

    [StructLayout(LayoutKind.Sequential)]
    public struct MarshalGuid
    {
        public UInt32 Data1;
        public UInt16 Data2;
        public UInt16 Data3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data4;

        public Guid ToGuid()
        {
            return new Guid((int)Data1, (short)Data2, (short)Data3, Data4);
        }

        // Guid.ToByteArray Method MSDN docs, remarks
        public static MarshalGuid FromGuid(Guid guid)
        {
            byte[] data = guid.ToByteArray();

            var mguid = new MarshalGuid
                            {
                                Data1 = (UInt32)(data[3] << 24 | data[2] << 16 | data[1] << 8 | data[0]),
                                Data2 = (UInt16)(data[5] << 8 | data[4]),
                                Data3 = (UInt16)(data[7] << 8 | data[6]),
                                Data4 = new byte[8]
                            };

            for (int i = 0; i < 8; i++)
            {
                mguid.Data4[i] = data[i + 8];
            }

            return mguid;
        }
    }
}