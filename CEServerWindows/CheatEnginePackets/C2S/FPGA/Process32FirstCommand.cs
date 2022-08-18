using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class Process32FirstCommand : CheatEngineCommand<Process32Response>
    {
        public override CommandType CommandType => CommandType.CMD_PROCESS32FIRST;

        public IntPtr Handle;
        public Process32FirstCommand()
        {
        }

        public Process32FirstCommand(IntPtr handle)
        {
            this.Handle = handle;
            this.initialized = true;
        }

        public sealed override void Initialize(BinaryReader reader)
        {
            this.Handle = (IntPtr)reader.ReadUInt32();
            this.initialized = true;
        }

        public override Process32Response Process()
        {
            CEServerWindows.FPGA.instance.dumpProcesses();
            var proc = CEServerWindows.FPGA.instance.popProcess();
            return new Process32Response(proc);
        }
    }
}
