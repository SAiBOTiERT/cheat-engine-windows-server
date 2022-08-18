﻿using System.IO;
using System.Text;
using CEServerWindows.CheatEnginePackets.S2C.WIN;

namespace CEServerWindows.CheatEnginePackets.C2S.WIN
{
    public class GetSymbolsFromFileCommand : CheatEngineCommand<GetSymbolsFromFileResponse>
    {

        public string SymbolsFile;
        public override CommandType CommandType => CommandType.CMD_GETSYMBOLLISTFROMFILE;// throw new NotImplementedException();

        public GetSymbolsFromFileCommand() { }

        public GetSymbolsFromFileCommand(string path)
        {
            SymbolsFile = path;
            this.initialized = true;
        }

        public override void Initialize(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            this.SymbolsFile = Encoding.UTF8.GetString( reader.ReadBytes(size));
            this.initialized = true;
        }

        public override GetSymbolsFromFileResponse Process()
        {
            return new GetSymbolsFromFileResponse(new byte[0]);
        }
    }
}
