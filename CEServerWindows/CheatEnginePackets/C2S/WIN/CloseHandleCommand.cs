using System;
using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.WIN;

namespace CEServerWindows.CheatEnginePackets.C2S.WIN
{
    public class CloseHandleCommand : CheatEngineCommand<CloseHandleResponse>
    {
        public IntPtr Handle;
        public override CommandType CommandType => CommandType.CMD_CLOSEHANDLE;

        public CloseHandleCommand()
        {

        }
        public CloseHandleCommand(IntPtr handle)
        {
            this.Handle = handle;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.Handle = (IntPtr)reader.ReadUInt32();
            this.initialized = true;
        }

        public override CloseHandleResponse Process()
        {
            try
            {
                return new CloseHandleResponse(WindowsAPI.ToolHelp.CloseHandle(this.Handle));
            }
            catch
            {
                return new CloseHandleResponse(true);
            }

        }
    }
}
