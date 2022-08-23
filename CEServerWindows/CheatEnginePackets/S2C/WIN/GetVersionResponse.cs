using System.IO;
using System.Text;

namespace CEServerWindows.CheatEnginePackets.S2C.WIN
{
    public class GetVersionResponse : ICheatEngineResponse
    {
        public const int VERSIONNR = 4;
        public const string VERSION = "CESERVER";
        public string version;
        public int versionNr;

        public GetVersionResponse()
        {
            this.version = VERSION;
            this.versionNr = VERSIONNR;
        }

        public GetVersionResponse(int versionNr, string version)
        {
            this.version = version;
            this.versionNr = versionNr;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(versionNr);
            br.Write((byte)version.Length);
            br.Write(Encoding.UTF8.GetBytes(version));

            br.Close();
            return ms.ToArray();
        }
    }
}
