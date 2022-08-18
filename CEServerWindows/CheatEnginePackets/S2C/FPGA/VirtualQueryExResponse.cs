using System;
using System.IO;
using CEServerWindows.WindowsAPI;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class VirtualQueryExResponse : ICheatEngineResponse 
    {
        public bool Result;
        public MemoryAPI.MEMORY_BASIC_INFORMATION Vad;
        
        public VirtualQueryExResponse(MemoryAPI.MEMORY_BASIC_INFORMATION? vad)
        {
            this.Result = vad != null;
            this.Vad = vad ?? new MemoryAPI.MEMORY_BASIC_INFORMATION();
        }

        public byte[] Serialize()
        {

            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            //The number of bytes return by VirtualQueryEx is the number of bytes written to mbi, if it's 0 it failed
            //But in Cheat engise server 1 is success and 0 is failed
            if (Result)
            {
                br.Write((byte)1);
            }
            else
            {
                br.Write((byte)0);
            }

            br.Write((uint)Vad.Protect);
            br.Write((uint)Vad.Type);
            br.Write((Int64)Vad.BaseAddress);
            br.Write((Int64)Vad.RegionSize);

            br.Close();
            return ms.ToArray();
        }
    }
}
