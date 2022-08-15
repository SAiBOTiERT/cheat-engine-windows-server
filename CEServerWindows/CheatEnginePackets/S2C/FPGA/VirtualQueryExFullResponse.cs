using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vmmsharp;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class VirtualQueryExFullResponse : ICheatEngineResponse
    {

        public List<Vmm.MAP_VADENTRY> Regions;
        public VirtualQueryExFullResponse(List<Vmm.MAP_VADENTRY> regions)
        {
            this.Regions = regions;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(Regions.Count);
            //The number of bytes return by VirtualQueryEx is the number of bytes written to mbi, if it's 0 it failed
            //But in Cheat engise server 1 is success and 0 is failed
            foreach (var region in Regions)
            {//Yes this is reversed from VirtualQueryEx....
                br.Write((long)region.vaStart);
                br.Write((long)region.cbSize);
                br.Write(CEServerWindows.FPGA.getWin32Protection(region));
                br.Write(CEServerWindows.FPGA.getWin32Type(region));
            }

            br.Close();
            return ms.ToArray();
        }
    }
}
