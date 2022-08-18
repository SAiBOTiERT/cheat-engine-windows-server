using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class VirtualQueryExCommand : CheatEngineCommand<VirtualQueryExResponse>
    {

        public uint Pid;
        public UInt64 Address;

        public override CommandType CommandType => CommandType.CMD_VIRTUALQUERYEX;// throw new NotImplementedException();

        public VirtualQueryExCommand() { }

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
            return new VirtualQueryExResponse(CEServerWindows.FPGA.instance.getVad(Pid, Address));
        }
    }
}
