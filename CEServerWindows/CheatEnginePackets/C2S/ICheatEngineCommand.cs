namespace CEServerWindows.CheatEnginePackets.C2S
{
    public interface ICheatEngineCommand
    {
       bool initialized { get;  }

       void Initialize(System.IO.BinaryReader reader);

       void Unintialize();
       CommandType CommandType { get; }
       byte[] ProcessAndGetBytes();

    }
}
