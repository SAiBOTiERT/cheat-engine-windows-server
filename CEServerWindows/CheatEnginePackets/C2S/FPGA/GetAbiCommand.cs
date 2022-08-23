using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class GetAbiCommand : CheatEngineCommand<GetAbiResponse>
    {

        public override CommandType CommandType => CommandType.CMD_GETABI;

        public GetAbiCommand() 
        {
        }

        public GetAbiCommand(IntPtr handle, UInt64 address) 
        {
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.initialized = true;
        }

        public override GetAbiResponse Process()
        {
            return new GetAbiResponse(Abi.Windows);
        }
    }
}
