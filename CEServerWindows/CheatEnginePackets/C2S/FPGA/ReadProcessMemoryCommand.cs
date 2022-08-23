using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class ReadProcessMemoryCommand : CheatEngineCommand<ReadProcessMemoryResponse>
    {
        public uint Pid;
        public UInt64 Address;
        public int Size;
        public bool Compress;

        public override CommandType CommandType => CommandType.CMD_READPROCESSMEMORY;

        public ReadProcessMemoryCommand() { }

        public ReadProcessMemoryCommand(uint pid, UInt64 address, int size, bool compress)
        {
            this.Pid = pid;
            this.Address = address;
            this.Size = size;
            this.Compress = compress;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            Pid = (uint)reader.ReadInt32();
            Address = reader.ReadUInt64();
            Size = reader.ReadInt32();
            Compress = reader.ReadByte() == 0 ? false : true;
            this.initialized = true;
        }

        public override ReadProcessMemoryResponse Process()
        {
            return new ReadProcessMemoryResponse(CEServerWindows.FPGA.instance.RPM(Pid, Address, (uint)Size), this.Compress);
        }
    }
}
