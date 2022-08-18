using System.IO;
using System.Text;

namespace CEServerWindows.CheatEnginePackets.S2C.FPGA
{
    public class Module32Response : ICheatEngineResponse
    {
        public bool Result;
        public vmmsharp.Vmm.MAP_MODULEENTRY? Module;

        public Module32Response(vmmsharp.Vmm.MAP_MODULEENTRY? module)
        {
            this.Result = module != null;
            this.Module = module;
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);

            br.Write((int)(this.Result ? 1 : 0));
            if (this.Result)
            {
                br.Write((long)Module?.vaBase);
                br.Write(Module?.cbImageSize ?? 0);
                br.Write(Module?.wszText.Length ?? 0);
                br.Write(Encoding.UTF8.GetBytes(Module?.wszText));
            }
            else 
            {
                br.Write(0L);//Base Address
                br.Write(0);//Mod Size
                br.Write(0);//str Size
            }

            br.Close();
            return ms.ToArray();
        }
    }
}

