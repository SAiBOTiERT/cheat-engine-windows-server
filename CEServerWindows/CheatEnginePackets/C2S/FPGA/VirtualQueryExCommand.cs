using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C;
using CEServerWindows.WindowsAPI;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class VirtualQueryExCommand : CheatEngineCommand<VirtualQueryExResponse>
    {

        public uint Pid;
        public UInt64 Address;

        public override CommandType CommandType => CommandType.CMD_VIRTUALQUERYEX;

        public VirtualQueryExCommand()
        {

        }

        public VirtualQueryExCommand(uint pid, UInt64 address) 
        {
            this.Pid = pid;
            this.Address = address;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.Pid = (uint)reader.ReadInt32();
            this.Address = reader.ReadUInt64();
            this.initialized = true;
        }

        public override VirtualQueryExResponse Process()
        {
            var bmi = CEServerWindows.FPGA.instance.getVad(Pid, Address);
            return new VirtualQueryExResponse(bmi != null, bmi ?? new MemoryAPI.MEMORY_BASIC_INFORMATION());
        }
    }
}
