using System.Runtime.InteropServices;
using CEServerWindows.CheatEnginePackets.S2C.WIN;

namespace CEServerWindows.CheatEnginePackets.C2S.WIN
{
    public class Module32NextCommand : Module32FirstCommand
    {
        public override CommandType CommandType => CommandType.CMD_MODULE32NEXT;

        public override Module32Response Process()
        {
            WindowsAPI.ToolHelp.MODULEENTRY32 moduleEntry = new WindowsAPI.ToolHelp.MODULEENTRY32();
            moduleEntry.dwSize = (uint)Marshal.SizeOf(moduleEntry);

            var result = WindowsAPI.ToolHelp.Module32Next(this.Handle, ref moduleEntry);

            return new Module32Response(result, moduleEntry);
        }

    }
}
