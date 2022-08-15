﻿using CEServerWindows.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class CloseHandleCommand : CheatEngineCommand<CloseHandleResponse>
    {
        public IntPtr Handle;
        public override CommandType CommandType => CommandType.CMD_CLOSEHANDLE;

        public CloseHandleCommand()
        {

        }
        public CloseHandleCommand(IntPtr handle)
        {
            this.Handle = handle;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.Handle = (IntPtr)reader.ReadUInt32();
            this.initialized = true;
        }

        public override CloseHandleResponse Process()
        {
            return new CloseHandleResponse(true);
        }
    }
}
