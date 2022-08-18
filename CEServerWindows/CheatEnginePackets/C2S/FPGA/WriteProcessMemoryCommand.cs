using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class WriteProcessMemoryCommand : CheatEngineCommand<WriteProcessMemoryResponse>
    {
        public uint Pid;
        public IntPtr Address;
        public int Size;
        public byte[] Data;

        public override CommandType CommandType => CommandType.CMD_WRITEPROCESSMEMORY;

        public WriteProcessMemoryCommand() { }

        public WriteProcessMemoryCommand(uint pid, IntPtr address, int size, byte[] data)
        {
            this.Pid = pid;
            this.Address = address;
            this.Size = size;
            this.Data = data;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            Pid = reader.ReadUInt32();
            Address = (IntPtr)reader.ReadUInt64();
            Size = reader.ReadInt32();
            Data = reader.ReadBytes(Size);
            this.initialized = true;
        }

        public override WriteProcessMemoryResponse Process()
        {
            int written = 0;
            if (CEServerWindows.FPGA.instance.WPM(Pid, (ulong)Address, Data))
            {
                written = Data.Length;
            }
            return new WriteProcessMemoryResponse(written);
        }
    }
}
