using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vmmsharp;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class GetRegionInfoResponse : ICheatEngineResponse 
    {
        public Vmm.MAP_VADENTRY? Vad;

        public GetRegionInfoResponse(Vmm.MAP_VADENTRY? vad)
        {
            this.Vad = vad;
        }

        public byte[] Serialize()
        {

            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            //The number of bytes return by VirtualQueryEx is the number of bytes written to mbi, if it's 0 it failed
            //But in Cheat engise server 1 is success and 0 is failed
            if (Vad != null)
            {
                br.Write((byte)1);
                //br.Write(CEServerWindows.FPGA.getWin32Protection((int)Vad?.Protection));
                br.Write(CEServerWindows.FPGA.getWin32Protection(Vad ?? new Vmm.MAP_VADENTRY()));
                br.Write(CEServerWindows.FPGA.getWin32Type(Vad ?? new Vmm.MAP_VADENTRY()));
                br.Write((Int64)Vad?.vaStart);
                br.Write((Int64)Vad?.cbSize);
                br.Write((Byte)Vad?.wszText.Length);
                br.Write(Encoding.UTF8.GetBytes(Vad?.wszText));

            }
            else
            {
                br.Write((byte)0);
                br.Write((int)0);
                br.Write((int)0);
                br.Write((Int64)0);
                br.Write((Int64)0);
                br.Write((byte)0);
            }

            //Yes this is reversed from VirtualQueryExFull....


            br.Close();
            return ms.ToArray();
        }
    }
}
