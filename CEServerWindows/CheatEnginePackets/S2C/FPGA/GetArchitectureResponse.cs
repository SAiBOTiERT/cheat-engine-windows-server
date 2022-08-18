namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class GetArchitectureResponse : ICheatEngineResponse
    {
        public Architecture Architecture;

        public GetArchitectureResponse(Architecture arch)
        {
            this.Architecture = arch;
        }

        public byte[] Serialize()
        {
            return new byte[] { (byte)Architecture };
        }
    }
}
