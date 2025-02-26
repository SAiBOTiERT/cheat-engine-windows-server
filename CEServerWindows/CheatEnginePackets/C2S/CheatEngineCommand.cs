﻿using CEServerWindows.CheatEnginePackets.S2C; 

namespace CEServerWindows.CheatEnginePackets.C2S
{
    public abstract class CheatEngineCommand<T> : ICheatEngineCommand  where T : ICheatEngineResponse
    {
        public bool initialized { get; internal set; }



        /**
         * Initializes a command from a binary reader
         */
        public abstract void Initialize(System.IO.BinaryReader reader);
        public abstract CommandType CommandType { get; }


        public abstract T Process();


        public byte[] ProcessAndGetBytes()
        {
            T output = this.Process();
            return output.Serialize();
        }

        public void Unintialize()
        {
            this.initialized = false;
        }
    }
}
