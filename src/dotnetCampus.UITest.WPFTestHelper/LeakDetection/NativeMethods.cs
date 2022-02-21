// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using System.Runtime.InteropServices;

namespace dotnetCampus.UITest.WPFTestHelper.LeakDetection
{
    internal static class NativeMethods
    {
        internal const int GR_GDIOBJECTS = 0;
        internal const int GR_USEROBJECTS = 1;

        [DllImport("User32.dll")]
        internal static extern int GetGuiResources(IntPtr hProcess, int flags);

        [DllImport("kernel32.dll")]
        internal static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

        // Interop call the get workingset information.
        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern int QueryWorkingSet(IntPtr hProcess, IntPtr info, int size);

        // Interop call the get performance memory counters
        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern int GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS_EX counters, int size);        
    }    

    [StructLayout(LayoutKind.Sequential)]
    internal struct SYSTEM_INFO
    {
        internal _PROCESSOR_INFO_UNION uProcessorInfo;
        public uint dwPageSize;
        public IntPtr lpMinimumApplicationAddress;
        public IntPtr lpMaximumApplicationAddress;
        public IntPtr dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public ushort dwProcessorLevel;
        public ushort dwProcessorRevision;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct _PROCESSOR_INFO_UNION
    {
        [FieldOffset(0)]
        internal uint dwOemId;
        [FieldOffset(0)]
        internal ushort wProcessorArchitecture;
        [FieldOffset(2)]
        internal ushort wReserved;
    }

    // Struct to hold performace memory counters.
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_MEMORY_COUNTERS_EX
    {
        public int cb;
        public int PageFaultCount;
        public int PeakWorkingSetSize;
        public int WorkingSetSize;
        public int QuotaPeakPagedPoolUsage;
        public int QuotaPagedPoolUsage;
        public int QuotaPeakNonPagedPoolUsage;
        public int QuotaNonPagedPoolUsage;
        public int PagefileUsage;
        public int PeakPagefileUsage;
        public int PrivateUsage;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class PSAPI_WORKING_SET_INFORMATION
    {
        public int NumberOfEntries;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
        public PSAPI_WORKING_SET_BLOCK[] WorkingSetInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BLOCK
    {
        public uint bitvector1;

        public uint Protection;
        public uint ShareCount;
        public uint Reserved;
        public uint VirtualPage;

        public uint Shared
        {
            get { return ((uint)(((this.bitvector1 & 256u) >> 8))); }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct PSAPI_WORKING_SET_BLOCK
    {
        [FieldOffset(0)]
        public uint Flags;

        [FieldOffset(0)]
        public BLOCK Block1;
    }
}
