using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class Module32FirstCommand : CheatEngineCommand<Module32Response>
    {
        public override CommandType CommandType => CommandType.CMD_MODULE32FIRST;

        public uint Pid;
        public Module32FirstCommand()
        {
        }

        public Module32FirstCommand(uint pid)
        {
            this.Pid = pid;
            this.initialized = true;
        }

        public sealed override void Initialize(BinaryReader reader)
        {
            this.Pid = reader.ReadUInt32();
            this.initialized = true;
        }

        public override Module32Response Process()
        {
            CEServerWindows.FPGA.instance.dumpModules(this.Pid);
            var module = CEServerWindows.FPGA.instance.popModule();
            return new Module32Response(module);
        }
    }
}
