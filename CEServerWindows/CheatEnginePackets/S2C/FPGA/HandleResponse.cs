using System;
using System.IO;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class HandleResponse : ICheatEngineResponse
    {
        public IntPtr Handle;

        public HandleResponse(IntPtr handle)
        {
            this.Handle = handle;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);

            br.Write((int)this.Handle);
            br.Close();
            return ms.ToArray();
        }
    }
}