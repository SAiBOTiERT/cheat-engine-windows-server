using CEServerWindows.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CEServerWindows.CheatEnginePackets.S2C.WIN;

namespace CEServerWindows.CheatEnginePackets.C2S.WIN
{
    public class GetRegionInfoCommand : CheatEngineCommand<GetRegionInfoResponse>
    {

        public IntPtr Handle;
        public UInt64 Address;

        public override CommandType CommandType => CommandType.CMD_GETREGIONINFO;// throw new NotImplementedException();

        public GetRegionInfoCommand() { }

        public GetRegionInfoCommand(IntPtr handle, UInt64 address) 
        {
            this.Handle = handle;
            this.Address = address;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.Handle = (IntPtr)reader.ReadInt32();
            this.Address = reader.ReadUInt64();
            this.initialized = true;
        }

        public override GetRegionInfoResponse Process()
        {
            WindowsAPI.MemoryAPI.MEMORY_BASIC_INFORMATION mbi = new WindowsAPI.MemoryAPI.MEMORY_BASIC_INFORMATION();
            int ret = WindowsAPI.MemoryAPI.VirtualQueryEx(Handle, (IntPtr)Address, out mbi, (uint)Marshal.SizeOf(mbi));

            return new GetRegionInfoResponse(ret, mbi);
        }
    }
}
