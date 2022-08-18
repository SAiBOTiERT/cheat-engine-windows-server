using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CEServerWindows.WindowsAPI;
using vmmsharp;

namespace CEServerWindows
{
    public class FPGA
    {
        public static FPGA instance;
        private Vmm vmm;
        private List<Vmm.PROCESS_INFORMATION> processes = new ();
        private List<Vmm.MAP_MODULEENTRY> modules = new ();
        private Dictionary<UInt64, MemoryAPI.MEMORY_BASIC_INFORMATION> vads = new ();
        private Dictionary<UInt64, String> vadImages = new ();

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

        public String? getImageByMBI(MemoryAPI.MEMORY_BASIC_INFORMATION mbi)
        {
            if (mbi.Type == MemoryAPI.TypeEnum.MEM_IMAGE && vadImages.ContainsKey((UInt64)mbi.BaseAddress))
            {
                return vadImages[(UInt64)mbi.BaseAddress];
            }
            return null;
        }

        private Dictionary<UInt64, MemoryAPI.MEMORY_BASIC_INFORMATION> transformVadsToMBIS(Vmm.MAP_VADENTRY[] vads)
        {
            var sortedList = vads.OrderBy(x => x.vaStart).ToList();
            var full = new Dictionary<UInt64, MemoryAPI.MEMORY_BASIC_INFORMATION>();
            UInt64 ptr = 0;
            vadImages.Clear();
            while (sortedList.Any())
            {
                var nextVad = sortedList.ElementAt(0);
                var curr = new MemoryAPI.MEMORY_BASIC_INFORMATION();
                if(nextVad.vaStart == ptr)
                {
                    sortedList.RemoveAt(0);
                    curr.BaseAddress = (IntPtr)nextVad.vaStart;
                    curr.RegionSize = (IntPtr)nextVad.cbSize;
                    curr.Protect = MemoryAPI.AllocationProtectEnum.PAGE_READWRITE;
                    curr.Type = getWin32Type(nextVad);
                    curr.State = 0;
                    full.Add(ptr, curr);
                    if (curr.Type == MemoryAPI.TypeEnum.MEM_IMAGE)
                    {
                        vadImages.Add(ptr, nextVad.wszText);
                    }
                    ptr += (UInt64)nextVad.cbSize;
                }
                else
                {
                    curr.BaseAddress = (IntPtr)ptr;
                    curr.RegionSize = (IntPtr)(nextVad.vaStart - (UInt64)curr.BaseAddress);
                    curr.Protect = MemoryAPI.AllocationProtectEnum.PAGE_NOACCESS;
                    curr.Type = 0;
                    curr.State = MemoryAPI.StateEnum.MEM_FREE;
                    full.Add(ptr, curr);
                    ptr += (UInt64)curr.RegionSize;
                }
            }
            return full;
        }

        private Dictionary<UInt64, MemoryAPI.MEMORY_BASIC_INFORMATION> dumpVads(uint pid, bool cached = true)
        {
            if (!vads.Any() || !cached)
            {
                vads = transformVadsToMBIS(vmm.Map_GetVad(pid));
            }
            return vads;
        }

        public MemoryAPI.MEMORY_BASIC_INFORMATION? getVad(uint pid, UInt64 Address)
        {
            var vads = dumpVads(pid, Address != 0);

            foreach (var mbi in vads.Values)
            {
                if (Address >= (UInt64)mbi.BaseAddress &&
                    Address <= (UInt64)mbi.BaseAddress + (UInt64)mbi.RegionSize - 1)
                {
                    return mbi;
                }
            }
            return null;
        }

        public MemoryAPI.MEMORY_BASIC_INFORMATION[] getAllVads(uint pid)
        {
            return dumpVads(pid, false).Values.ToArray();
        }

        private MemoryAPI.TypeEnum getWin32Type(Vmm.MAP_VADENTRY vad)
        {
            if(vad.fImage) return MemoryAPI.TypeEnum.MEM_IMAGE;
            if(vad.fPrivateMemory) return MemoryAPI.TypeEnum.MEM_PRIVATE;
            return MemoryAPI.TypeEnum.MEM_MAPPED;
        }

        public unsafe byte[] RPM(uint pid, ulong qwA, uint cb)
        {
            uint cbRead,cbRead2;
            byte[] data = new byte[cb];
            fixed (byte* pb = data)
            {
                if (!vmmi.VMMDLL_MemReadEx(vmm.hVMM, pid, qwA, pb, cb, out cbRead, Vmm.FLAG_NOCACHE))
                {
                    return null;
                }
            }
            if (cbRead != cb)
            {
                byte[] data2 = new byte[cb - cbRead];
                fixed (byte* pb2 = data2)
                {
                    if (!vmmi.VMMDLL_MemReadEx(vmm.hVMM, pid, qwA + cbRead, pb2, cb - cbRead, out cbRead2, Vmm.FLAG_NOCACHE))
                    {
                        Array.Resize<byte>(ref data, (int)cbRead);
                        return data;
                    }
                    Array.Copy(data2, 0, data, cbRead, data2.Length);
                }

            }
            return data;
        }

        ~FPGA()
        {
            vmm.Close();
        }

    }
}