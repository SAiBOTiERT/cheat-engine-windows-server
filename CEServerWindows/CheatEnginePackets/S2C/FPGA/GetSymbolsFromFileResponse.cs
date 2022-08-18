using System.IO;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class GetSymbolsFromFileResponse : ICheatEngineResponse
    {
        public byte[] Data;

        public GetSymbolsFromFileResponse(byte[] data)
        {
            this.Data = data;
        }
        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(0L);

            br.Close();
            return ms.ToArray();
        }
    }
}
