using System.IO;
using CEServerWindows.CheatEnginePackets.S2C.FPGA;

namespace CEServerWindows.CheatEnginePackets.C2S.FPGA
{
    public class VirtualQueryExFullCommand : CheatEngineCommand<VirtualQueryExFullResponse>
    {

        public uint Pid;

        public override CommandType CommandType => CommandType.CMD_VIRTUALQUERYEXFULL;// throw new NotImplementedException();

        public VirtualQueryExFullCommand() { }

        public VirtualQueryExFullCommand(uint pid) 
        {
            this.Pid = pid;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            this.Pid = (uint)reader.ReadInt32();
            reader.ReadByte();//Cheat engine/linux specific flags?
            this.initialized = true;
        }

        public override VirtualQueryExFullResponse Process()
        {
            return new VirtualQueryExFullResponse(CEServerWindows.FPGA.instance.getAllVads(Pid));
        }
    }
}
