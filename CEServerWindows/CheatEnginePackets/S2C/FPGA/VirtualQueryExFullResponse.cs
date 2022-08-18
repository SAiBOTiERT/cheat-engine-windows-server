using System.IO;
using CEServerWindows.WindowsAPI;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class VirtualQueryExFullResponse : ICheatEngineResponse
    {

        public MemoryAPI.MEMORY_BASIC_INFORMATION[] Regions;
        public VirtualQueryExFullResponse(MemoryAPI.MEMORY_BASIC_INFORMATION[] regions)
        {
            this.Regions = regions;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(Regions.Length);
            //The number of bytes return by VirtualQueryEx is the number of bytes written to mbi, if it's 0 it failed
            //But in Cheat engise server 1 is success and 0 is failed

            foreach (var region in Regions)
            {//Yes this is reversed from VirtualQueryEx....
                br.Write((long)region.BaseAddress);
                br.Write((long)region.RegionSize);
                br.Write((uint)region.Protect);
                br.Write((uint)region.Type);
            }

            br.Close();
            return ms.ToArray();
        }
    }
}
