using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using CEServerWindows.CheatEnginePackets.S2C.WIN;

namespace CEServerWindows.CheatEnginePackets.C2S.WIN
{
    public class VirtualQueryExFullCommand : CheatEngineCommand<VirtualQueryExFullResponse>
    {

        public IntPtr Handle;
        public byte Config;

        public override CommandType CommandType => CommandType.CMD_VIRTUALQUERYEXFULL;// throw new NotImplementedException();

        public VirtualQueryExFullCommand() { }

        public VirtualQueryExFullCommand(IntPtr handle, byte config) 
        {
            this.Handle = handle;
            this.Config = config;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.Handle = (IntPtr)reader.ReadInt32();
            this.Config = reader.ReadByte();
            this.initialized = true;
        }

        public override VirtualQueryExFullResponse Process()
        {
            var regions = new List<WindowsAPI.MemoryAPI.MEMORY_BASIC_INFORMATION>();
            long currentAddress = 0;
            int ret = 0;
            while(true)
            {
                WindowsAPI.MemoryAPI.MEMORY_BASIC_INFORMATION mbi = new WindowsAPI.MemoryAPI.MEMORY_BASIC_INFORMATION();
                ret = WindowsAPI.MemoryAPI.VirtualQueryEx(Handle, (IntPtr)currentAddress, out mbi, (uint)Marshal.SizeOf(mbi));
                if (ret == 0)
                    break;
                currentAddress += (long)mbi.RegionSize;
                regions.Add(mbi);
            }

            return new VirtualQueryExFullResponse(regions);
        }
    }
}
