using System;
using System.IO;

namespace CEServerWindows.CheatEnginePackets.S2C.WIN
{
    public class WriteProcessMemoryResponse : ICheatEngineResponse
    {
        public int Written;
        public WriteProcessMemoryResponse(int written)
        {
            this.Written = written;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(Written);
            br.Close();
            return ms.ToArray();
        }
    }
}
