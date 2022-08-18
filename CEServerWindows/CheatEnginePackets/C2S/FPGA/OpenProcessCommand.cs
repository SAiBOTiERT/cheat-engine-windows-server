using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class OpenProcessCommand : CheatEngineCommand<HandleResponse>
    {
        public override CommandType CommandType => CommandType.CMD_OPENPROCESS;// throw new NotImplementedException();
        public uint ProcessID;
        public OpenProcessCommand()
        {

        }

        public OpenProcessCommand(uint pid)
        {
            this.ProcessID = pid;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.ProcessID = (uint)reader.ReadInt32();
            this.initialized = true;
        }

        public override HandleResponse Process()
        {
            return new HandleResponse((IntPtr)this.ProcessID);
        }
    }
}
