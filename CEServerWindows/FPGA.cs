using System;
using System.Collections.Generic;
using System.Linq;
using CEServerWindows.WindowsAPI;
using vmmsharp;

namespace CEServerWindows
{
    public class FPGA
    {
        public static FPGA instance;
        private Vmm _vmm;
        private List<Vmm.PROCESS_INFORMATION> _processes = new ();
        private List<Vmm.MAP_MODULEENTRY> _modules = new ();
        private Dictionary<UInt64, MemoryAPI.MEMORY_BASIC_INFORMATION> _vads = new ();
        private Dictionary<UInt64, String> _vadImages = new ();

        public FPGA()
        {
            instance = this;
            _vmm = new Vmm("-printf", "-v", "-device", "fpga");
        }

        public void dumpProcesses()
        {
            _processes.Clear();
            foreach (var pid in _vmm.PidList())
            {
                var proc = _vmm.ProcessGetInformation(pid);
                if (proc.fValid)
                {
                    _processes.Add(proc);
                }
            }
            _processes = _processes.OrderBy(x => x.dwPID).ToList();
        }

        public void dumpModules(uint pid)
        {
            _modules.Clear();
            foreach (var module in _vmm.Map_GetModule(pid))
            {
                if (module.fValid)
                {
                    _modules.Add(module);
                }
            }
            _modules = _modules.OrderBy(x => x.vaBase).ToList();
        }

        public Vmm.MAP_MODULEENTRY? popModule()
        {
            if (_modules.Count() == 0) return null;
            var module = _modules.ElementAt(0);
            _modules.RemoveAt(0);
            return module;
        } 

        public Vmm.PROCESS_INFORMATION? popProcess()
        {
            if (_processes.Count() == 0) return null;
            var proc = _processes.ElementAt(0);
            _processes.RemoveAt(0);
            return proc;
        }

        public String? getImageByMBI(MemoryAPI.MEMORY_BASIC_INFORMATION mbi)
        {
            if (mbi.Type == MemoryAPI.TypeEnum.MEM_IMAGE && _vadImages.ContainsKey((UInt64)mbi.BaseAddress))
            {
                return _vadImages[(UInt64)mbi.BaseAddress];
            }
            return null;
        }

        private Dictionary<UInt64, MemoryAPI.MEMORY_BASIC_INFORMATION> transformVadsToMBIS(Vmm.MAP_VADENTRY[] vads)
        {
            var sortedList = vads.OrderBy(x => x.vaStart).ToList();
            var full = new Dictionary<UInt64, MemoryAPI.MEMORY_BASIC_INFORMATION>();
            UInt64 ptr = 0;
            _vadImages.Clear();
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
                        _vadImages.Add(ptr, nextVad.wszText);
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
            if (!_vads.Any() || !cached)
            {
                _vads = transformVadsToMBIS(_vmm.Map_GetVad(pid));
            }
            return _vads;
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

        public bool WPM(uint pid, ulong qwA, byte[] data)
        {
            return _vmm.MemWrite(pid, qwA, data);
        }


        public unsafe byte[] RPM(uint pid, ulong qwA, uint cb)
        {
            byte[] data = new byte[cb];
            fixed (byte* pb = data)
            {
                vmmi.VMMDLL_MemReadEx(_vmm.hVMM, pid, qwA, pb, cb, out _,
                    Vmm.FLAG_NOCACHE | Vmm.FLAG_ZEROPAD_ON_FAIL);
            }
            return data;
        }

        public uint OpenProcessCommand(uint pid)
        {
            _vads.Clear();
            _vadImages.Clear();
            return pid;
        }

        ~FPGA()
        {
            _vmm.Close();
        }

    }
}