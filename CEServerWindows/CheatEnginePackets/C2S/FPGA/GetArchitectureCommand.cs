using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
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
