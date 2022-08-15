﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class GetArchitectureResponse : ICheatEngineResponse
    {
        public Architecture Architecture;

        public GetArchitectureResponse(Architecture arch)
        {
            this.Architecture = arch;
        }

        public byte[] Serialize()
        {
            return new byte[] { (byte)Architecture };
        }
    }
}
