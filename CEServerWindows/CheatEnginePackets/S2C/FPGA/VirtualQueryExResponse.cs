using System;
using System.IO;
using CEServerWindows.WindowsAPI;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class VirtualQueryExResponse : ICheatEngineResponse 
    {
        public MemoryAPI.MEMORY_BASIC_INFORMATION? Vad;
        
        public VirtualQueryExResponse(MemoryAPI.MEMORY_BASIC_INFORMATION? vad)
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
                br.Write((uint)Vad?.Protect);
                br.Write((uint)Vad?.Type);
                br.Write((Int64)Vad?.BaseAddress);
                br.Write((Int64)Vad?.RegionSize);
            }
            else
            {
                br.Write((byte)0);
                br.Write((int)0);
                br.Write((int)0);
                br.Write((Int64)0);
                br.Write((Int64)0);
            }

            //Yes this is reversed from VirtualQueryExFull....


            br.Close();
            return ms.ToArray();
        }
    }
}
