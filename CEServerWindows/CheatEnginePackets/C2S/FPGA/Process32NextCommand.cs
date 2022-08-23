using CEServerWindows.CheatEnginePackets.S2C;
using CEServerWindows.WindowsAPI;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class Process32NextCommand : Process32FirstCommand
    {
        public override CommandType CommandType => CommandType.CMD_PROCESS32NEXT;

        public override Process32Response Process()
        {
            var pe32 = CEServerWindows.FPGA.instance.popProcess();
            return new Process32Response(pe32 != null, pe32 ?? new ToolHelp.PROCESSENTRY32());
        }

    }
}
