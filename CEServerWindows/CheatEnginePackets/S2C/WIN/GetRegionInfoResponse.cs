using System;
using System.IO;

namespace CEServerWindows.CheatEnginePackets.S2C.WIN
{
    public class GetRegionInfoResponse : ICheatEngineResponse 
    {
        public int Result;
        public WindowsAPI.MemoryAPI.MEMORY_BASIC_INFORMATION MemoryBasicInformation;
        
        public GetRegionInfoResponse(int result, WindowsAPI.MemoryAPI.MEMORY_BASIC_INFORMATION mbi)
        {
            this.Result = result;
            this.MemoryBasicInformation = mbi;
        }

        public byte[] Serialize()
        {

            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            //The number of bytes return by VirtualQueryEx is the number of bytes written to mbi, if it's 0 it failed
            //But in Cheat engise server 1 is success and 0 is failed
            if (Result > 0)
                br.Write((byte)1);
            else
                br.Write((byte)0);
            //Yes this is reversed from VirtualQueryExFull....
            br.Write((int)MemoryBasicInformation.Protect);
            br.Write((int)MemoryBasicInformation.Type);
            br.Write((Int64)MemoryBasicInformation.BaseAddress);
            br.Write((Int64)MemoryBasicInformation.RegionSize);
            br.Write((Byte)0);

            br.Close();
            return ms.ToArray();
        }
    }
}
