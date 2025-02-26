﻿using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C;
using CEServerWindows.WindowsAPI;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class GetRegionInfoCommand : CheatEngineCommand<GetRegionInfoResponse>
    {

        public uint Pid;

        public UInt64 Address;

        public override CommandType CommandType => CommandType.CMD_GETREGIONINFO;

        public GetRegionInfoCommand()
        {
        }

        public GetRegionInfoCommand(uint pid, UInt64 address) 
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

        public override GetRegionInfoResponse Process()
        {
            
            var mbi = CEServerWindows.FPGA.instance.getVad(Pid, Address);
            var result = mbi != null;
            if (!result)
            {
                mbi = new MemoryAPI.MEMORY_BASIC_INFORMATION();
            }
            return new GetRegionInfoResponse(result, (MemoryAPI.MEMORY_BASIC_INFORMATION)mbi!, CEServerWindows.FPGA.instance.getImageByMBI((MemoryAPI.MEMORY_BASIC_INFORMATION)mbi));
        }
    }
}
