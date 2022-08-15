using CEServerWindows.CheatEnginePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CEServerWindows.CheatEnginePackets.C2S;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using vmmsharp;

namespace CEServerWindows
{
    public class FPGA
    {
        public static FPGA instance;
        public Vmm? vmm = null;
        private List<Vmm.PROCESS_INFORMATION> processes = new ();
        private List<Vmm.MAP_MODULEENTRY> modules = new ();
        private List<Vmm.MAP_VADENTRY> vads = new ();
        public int vadPtr = 0;

        public FPGA()
        {
            instance = this;
            vmm = new Vmm("-printf", "-v", "-device", "fpga");
        }

        public void dumpProcesses()
        {
            processes.Clear();
            foreach (var pid in vmm.PidList())
            {
                var proc = vmm.ProcessGetInformation(pid);
                if (proc.fValid)
                {
                    processes.Add(proc);
                }
            }
            processes = processes.OrderBy(x => x.dwPID).ToList();
        }

        public void dumpModules(uint pid)
        {
            modules.Clear();
            foreach (var module in vmm.Map_GetModule(pid))
            {
                if (module.fValid)
                {
                    modules.Add(module);
                }
            }
            modules = modules.OrderBy(x => x.vaBase).ToList();
        }
        //modules = new List<Vmm.MAP_VADENTRY>(vads.ToArray().Where(vad => vad.fImage));

        public Vmm.MAP_MODULEENTRY? popModule()
        {
            if (modules.Count() == 0) return null;
            var module = modules.ElementAt(0);
            modules.RemoveAt(0);
            return module;
        } 

        public Vmm.PROCESS_INFORMATION? popProcess()
        {
            if (processes.Count() == 0) return null;
            var proc = processes.ElementAt(0);
            processes.RemoveAt(0);
            return proc;
        }

        public List<Vmm.MAP_VADENTRY> dumpVads(uint pid, bool cached = true)
        {
            if (vads.Count() == 0 || !cached)
            {
                vads = new List<Vmm.MAP_VADENTRY>(vmm.Map_GetVad(pid));
            }
            return vads.OrderBy(x => x.vaStart).ToList();
        }

        public Vmm.MAP_VADENTRY? getVadEntry(uint pid, ulong Address)
        {
            if (Address == 0)
            {
                vadPtr = 0;
            }
            var vads = dumpVads(pid).ToArray();
            if (vadPtr < vads.Length)
            {
                var vad  =vads.ElementAtOrDefault(vadPtr);
                vadPtr++;
                return vad;
            }
            return null;
        }

        public static int getWin32Type(Vmm.MAP_VADENTRY vad)
        {
            switch (vad.VadType) {
                case 2:
                    return 0x1000000; //TYPE_MEM_IMAGE;
                case 1:
                    return 0x40000; //TYPE_MEM_MAPPED;
                default:
                    return 0x20000; //TYPE_MEM_PRIVATE;
            }
        }

        public static uint getWin32Protection(Vmm.MAP_VADENTRY vad)
        {
            return 0x04; //PAGE_READWRITE
            if (vad.MemCommit)
            {
                return 0x04; //PAGE_READWRITE
            }
            //if (vad.Protection == 1) return 2;
            return vad.Protection; //PAGE_READWRITE
        }

        ~FPGA()
        {
            vmm?.Close();
        }

    }
}