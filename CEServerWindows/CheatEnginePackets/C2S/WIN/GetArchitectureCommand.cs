using CEServerWindows.CheatEnginePackets.S2C;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CEServerWindows.CheatEnginePackets.S2C.WIN;

namespace CEServerWindows.CheatEnginePackets.C2S.WIN
{
    public class GetArchitectureCommand : CheatEngineCommand<GetArchitectureResponse>
    {
        public override CommandType CommandType => CommandType.CMD_GETARCHITECTURE;

        public GetArchitectureCommand()
        {
            this.initialized = true;
        }
        public override void Initialize(BinaryReader reader)
        {
            this.initialized = true;
        }

        public override GetArchitectureResponse Process()
        {
            return new GetArchitectureResponse(Architecture.x64);
        }
    }
}
