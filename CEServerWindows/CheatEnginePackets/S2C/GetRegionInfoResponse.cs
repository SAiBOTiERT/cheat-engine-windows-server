using System;
using System.IO;
using System.Text;

namespace CEServerWindows.CheatEnginePackets.S2C
{
    public class GetRegionInfoResponse : ICheatEngineResponse 
    {
        public bool Result;
        public WindowsAPI.MemoryAPI.MEMORY_BASIC_INFORMATION MemoryBasicInformation;
        public string ImageName;
        
        public GetRegionInfoResponse(bool result, WindowsAPI.MemoryAPI.MEMORY_BASIC_INFORMATION mbi, string imageName = "")
        {
            this.Result = result;
            this.MemoryBasicInformation = mbi;
            this.ImageName = imageName;
        }

        public byte[] Serialize()
        {

            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            //The number of bytes return by VirtualQueryEx is the number of bytes written to mbi, if it's 0 it failed
            //But in Cheat engise server 1 is success and 0 is failed
            if (Result)
                br.Write((byte)1);
            else
                br.Write((byte)0);
            //Yes this is reversed from VirtualQueryExFull....
            br.Write((int)MemoryBasicInformation.Protect);
            br.Write((int)MemoryBasicInformation.Type);
            br.Write((Int64)MemoryBasicInformation.BaseAddress);
            br.Write((Int64)MemoryBasicInformation.RegionSize);
            br.Write((Byte)ImageName.Length);
            br.Write(Encoding.UTF8.GetBytes(ImageName));

            br.Close();
            return ms.ToArray();
        }
    }
}
