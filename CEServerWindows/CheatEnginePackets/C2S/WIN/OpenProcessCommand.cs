using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.WIN;

namespace CEServerWindows.CheatEnginePackets.C2S.WIN
{
    public class OpenProcessCommand : CheatEngineCommand<HandleResponse>
    {
        public override CommandType CommandType => CommandType.CMD_OPENPROCESS;// throw new NotImplementedException();
        public int ProcessID;
        public OpenProcessCommand()
        {

        }

        public OpenProcessCommand(int pid)
        {
            this.ProcessID = pid;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.ProcessID = reader.ReadInt32();
            this.initialized = true;
        }

        public override HandleResponse Process()
        {

            var flags = WindowsAPI.ToolHelp.ProcessAccessFlags.VirtualMemoryRead | WindowsAPI.ToolHelp.ProcessAccessFlags.QueryInformation;

            if (CheatEngineServer.instance.enableWPM)
            {
                flags |= WindowsAPI.ToolHelp.ProcessAccessFlags.VirtualMemoryWrite;
            }

            IntPtr handle = WindowsAPI.ToolHelp.OpenProcess(
                flags, 
                false, this.ProcessID);

            return new HandleResponse(handle);
        }
    }
}
