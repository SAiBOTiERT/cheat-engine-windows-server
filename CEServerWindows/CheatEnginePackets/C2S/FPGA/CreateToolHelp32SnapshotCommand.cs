using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class CreateToolHelp32SnapshotCommand : CheatEngineCommand<HandleResponse>
    {
        public WindowsAPI.ToolHelp.SnapshotFlags SnapshotFlags;
        public uint ProcessID;

        public sealed override CommandType CommandType => CommandType.CMD_CREATETOOLHELP32SNAPSHOT;// throw new NotImplementedException();

        public CreateToolHelp32SnapshotCommand()
        {

        }
        public CreateToolHelp32SnapshotCommand(WindowsAPI.ToolHelp.SnapshotFlags snapshotFlags, uint pid)
        {
            this.SnapshotFlags = snapshotFlags;
            this.ProcessID = pid;
            this.initialized = true;
        }

        public override HandleResponse Process()
        {
            if (!this.initialized)
            {
                throw new Exceptions.CommandNotInitializedException();
            }

            if (ProcessID == 0) ProcessID = 1;
            return new HandleResponse((IntPtr)ProcessID);
        }

        public override void Initialize(BinaryReader reader)
        {
            this.SnapshotFlags = (WindowsAPI.ToolHelp.SnapshotFlags)reader.ReadUInt32();
            this.ProcessID = reader.ReadUInt32();
            this.initialized = true;

        }
    }
}
