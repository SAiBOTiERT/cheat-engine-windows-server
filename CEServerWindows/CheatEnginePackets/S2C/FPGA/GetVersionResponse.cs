using System.IO;
using System.Text;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class GetVersionResponse : ICheatEngineResponse
    {
        public const string VERSION = "CESERVER";
        public string version;

        public GetVersionResponse()
        {
            this.version = VERSION;
        }

        public GetVersionResponse(string version)
        {
            this.version = version;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(version.Length);
            br.Write(Encoding.UTF8.GetBytes(version));

            br.Close();
            return ms.ToArray();
        }
    }
}
