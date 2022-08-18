using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class Module32NextCommand : Module32FirstCommand
    {
        public override CommandType CommandType => CommandType.CMD_MODULE32NEXT;

        public override Module32Response Process()
        {
            var module = CEServerWindows.FPGA.instance.popModule();
            return new Module32Response(module);
        }

    }
}
