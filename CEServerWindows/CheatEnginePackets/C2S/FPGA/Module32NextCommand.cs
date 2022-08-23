using CEServerWindows.CheatEnginePackets.S2C;
using CEServerWindows.WindowsAPI;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class Module32NextCommand : Module32FirstCommand
    {
        public override CommandType CommandType => CommandType.CMD_MODULE32NEXT;

        public override Module32Response Process()
        {
            var me32 = CEServerWindows.FPGA.instance.popModule();
            return new Module32Response(me32 != null, me32 ?? new ToolHelp.MODULEENTRY32());
        }

    }
}
