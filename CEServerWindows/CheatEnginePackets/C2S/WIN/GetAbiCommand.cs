using System;
using System.IO;
using System.Runtime.InteropServices;
using CEServerWindows.CheatEnginePackets.S2C.WIN;

namespace CEServerWindows.CheatEnginePackets.C2S.WIN
{
    public class GetAbiCommand : CheatEngineCommand<GetAbiResponse>
    {

        public override CommandType CommandType => CommandType.CMD_GETABI;// throw new NotImplementedException();

        public GetAbiCommand() { }

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
