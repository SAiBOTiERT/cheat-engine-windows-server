using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class Process32Response : ICheatEngineResponse
    {
        public bool Result;
        public vmmsharp.Vmm.PROCESS_INFORMATION? Proc;

        public Process32Response(vmmsharp.Vmm.PROCESS_INFORMATION? proc)
        {
            this.Result = proc != null;
            this.Proc = proc;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);

            br.Write((int)(this.Result ? 1 : 0));
            if (this.Result)
            {
                br.Write((int)this.Proc?.dwPID);
                br.Write((int)this.Proc?.szNameLong.Length);
                br.Write(Encoding.UTF8.GetBytes(this.Proc?.szNameLong));
            }
            else
            {
                br.Write((int)0);
                br.Write((int)0);
            }

            br.Close();
            return ms.ToArray();
        }
    }
}
