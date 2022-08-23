using System;
using System.IO;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class GetAbiResponse : ICheatEngineResponse
    {
        public Abi Abi;

        public GetAbiResponse(Abi abi)
        {
            this.Abi = abi;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);

            br.Write((byte)this.Abi);
            br.Close();
            return ms.ToArray();
        }
    }
}
