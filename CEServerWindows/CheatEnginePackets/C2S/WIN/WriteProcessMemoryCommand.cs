using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C;

namespace CEServerWindows.CheatEnginePackets.C2S.WIN
{
    public class WriteProcessMemoryCommand : CheatEngineCommand<WriteProcessMemoryResponse>
    {
        public IntPtr Handle;
        public IntPtr Address;
        public int Size;
        public byte[] Data;

        public override CommandType CommandType => CommandType.CMD_WRITEPROCESSMEMORY;

        public WriteProcessMemoryCommand()
        {
        }

        public WriteProcessMemoryCommand(IntPtr handle, IntPtr address, int size, byte[] data)
        {
            this.Handle = handle;
            this.Address = address;
            this.Size = size;
            this.Data = data;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            Handle = (IntPtr)reader.ReadInt32();
            Address = (IntPtr)reader.ReadUInt64();
            Size = reader.ReadInt32();
            Data = reader.ReadBytes(Size);
            this.initialized = true;
        }

        public override WriteProcessMemoryResponse Process()
        {
            int written = 0;
            WindowsAPI.MemoryAPI.WriteProcessMemory(Handle, Address, Data, Size, ref written);
            return new WriteProcessMemoryResponse(written);
        }
    }
}
