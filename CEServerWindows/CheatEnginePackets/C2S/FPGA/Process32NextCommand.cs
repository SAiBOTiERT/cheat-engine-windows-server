using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class Process32NextCommand : Process32FirstCommand
    {
        public override CommandType CommandType => CommandType.CMD_PROCESS32NEXT;

        public override Process32Response Process()
        {
            var proc = CEServerWindows.FPGA.instance.popProcess();
            return new Process32Response(proc);
        }

    }
}
