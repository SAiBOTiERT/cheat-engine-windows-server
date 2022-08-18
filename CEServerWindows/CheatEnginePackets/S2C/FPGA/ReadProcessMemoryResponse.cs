using System;
using System.IO;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class ReadProcessMemoryResponse : ICheatEngineResponse
    {
        public byte[] Data;
        public bool Compression;
        public ReadProcessMemoryResponse(byte[] data, bool compression)
        {
            this.Data = data;
            this.Compression = compression;
        }

        public byte[] Serialize()
        {
            if(this.Compression)
                throw new NotImplementedException("Compression is not implemented");

            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(Data.Length);
            br.Write(Data);
            br.Close();
            return ms.ToArray();
        }
    }
}
