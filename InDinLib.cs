﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace K3log
{
	class InDinLib
	{
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
     ProcessAccessFlags processAccess,
     bool bInheritHandle,
     int processId);
        public static IntPtr OpenProcess(Process proc, ProcessAccessFlags flags)
        {
            return OpenProcess(flags, false, proc.Id);
        }

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
       int dwSize, AllocationType dwFreeType);

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            IntPtr dwSize,
            AllocationType flAllocationType,
            MemoryProtection flProtect);

      
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        [MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
        int dwSize,
        out IntPtr lpNumberOfBytesWritten);

   
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

     
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

      
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(
        IntPtr hProcess,
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public void monitorProcAndInj(String procName, string DllPath) {
            bool inj = false;
            while (!inj) {
                
                var processes = Process.GetProcessesByName(procName);
                foreach (var p in processes)
                {
                   int pid =  p.Id;
                    try
                    {
                        startInj(pid, DllPath);
                        inj = true;
                    }
                    catch { 
                        
                    }
                }
                Thread.Sleep(5000);
            }
        
        }

        private void startInj(int ProcId, string DllPath) {
            IntPtr Size;
            Size = (IntPtr)DllPath.Length;

            Process localById = Process.GetProcessById(ProcId);
            string ProcName = localById.ProcessName;

            IntPtr ProcHandle = OpenProcess(
                ProcessAccessFlags.All,
                false,
                ProcId);

            IntPtr DllSpace = VirtualAllocEx(
                ProcHandle,
                IntPtr.Zero,
                Size,
                AllocationType.Reserve | AllocationType.Commit,
                MemoryProtection.ExecuteReadWrite);

            byte[] bytes = Encoding.ASCII.GetBytes(DllPath);
            bool DllWrite = WriteProcessMemory(
                ProcHandle,
                DllSpace,
                bytes,
                (int)bytes.Length,
                out var bytesread
                );

            IntPtr Kernel32Handle = GetModuleHandle("Kernel32.dll");
            IntPtr LoadLibraryAAddress = GetProcAddress(Kernel32Handle, "LoadLibraryA");

            IntPtr RemoteThreadHandle = CreateRemoteThread(
                ProcHandle,
                IntPtr.Zero,
                0,
                LoadLibraryAAddress,
                DllSpace,
                0,
                IntPtr.Zero
                );

            bool FreeDllSpace = VirtualFreeEx(
                ProcHandle,
                DllSpace,
                0,
                AllocationType.Release);

            CloseHandle(RemoteThreadHandle);
            CloseHandle(ProcHandle);

        }
    }
}
