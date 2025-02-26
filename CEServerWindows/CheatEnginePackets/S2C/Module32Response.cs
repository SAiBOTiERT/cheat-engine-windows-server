﻿using System.IO;
using System.Text;

namespace CEServerWindows.CheatEnginePackets.S2C
{
    public class Module32Response : ICheatEngineResponse
    {
        public bool Result;
        public WindowsAPI.ToolHelp.MODULEENTRY32 ModuleEntry;

        public Module32Response(bool result, WindowsAPI.ToolHelp.MODULEENTRY32 moduleEntry)
        {
            this.Result = result;
            this.ModuleEntry = moduleEntry;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);

            br.Write((int)(this.Result ? 1 : 0));
            if (this.Result)
            {
                br.Write((long)ModuleEntry.modBaseAddr);
                br.Write(ModuleEntry.GlblcntUsage);
                br.Write(ModuleEntry.modBaseSize);
                br.Write(ModuleEntry.szModule.Length);
                br.Write(Encoding.UTF8.GetBytes(ModuleEntry.szModule));
            }
            else 
            {
                br.Write(0L);//Base Address
                br.Write(0);//GlblcntUsage
                br.Write(0);//Mod Size
                br.Write(0);//str Size
            }

            br.Close();
            return ms.ToArray();
        }
    }
}

