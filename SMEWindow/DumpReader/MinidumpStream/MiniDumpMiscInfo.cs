using DumpReader.Native;
using System;

namespace DumpReader.MinidumpStream
{
    [Serializable]
    public enum MiniDumpMiscInfoLevel
    {
        MiscInfo,
        MiscInfo2,
        MiscInfo3,
        MiscInfo4
    }

    [Serializable]
    [Flags]
    public enum MiscInfoFlags
    {
        MINIDUMP_MISC1_PROCESS_ID = 0x00000001,
        MINIDUMP_MISC1_PROCESS_TIMES = 0x00000002,
        MINIDUMP_MISC1_PROCESSOR_POWER_INFO = 0x00000004,
        MINIDUMP_MISC3_PROCESS_INTEGRITY = 0x00000010,
        MINIDUMP_MISC3_PROCESS_EXECUTE_FLAGS = 0x00000020,
        MINIDUMP_MISC3_TIMEZONE = 0x00000040,
        MINIDUMP_MISC3_PROTECTED_PROCESS = 0x00000080,
        MINIDUMP_MISC4_BUILDSTRING = 0x00000100
    }

    [Serializable]
    public class MiniDumpMiscInfo
    {
        private MINIDUMP_MISC_INFO m_miscInfo;

        internal MiniDumpMiscInfo(){ }
        internal MiniDumpMiscInfo(MINIDUMP_MISC_INFO miscInfo) : this()
        {
            this.MiscInfoLevel = MiniDumpMiscInfoLevel.MiscInfo;
            this.m_miscInfo = miscInfo;
        }

        public UInt32 SizeOfInfo { get { return this.m_miscInfo.SizeOfInfo; } }
        public MiscInfoFlags Flags1 { get { return (MiscInfoFlags)this.m_miscInfo.Flags1; } }
        public UInt32 ProcessId { get { return this.m_miscInfo.ProcessId; } }
        public DateTime ProcessCreateTime { get { return SMEMiniDumpReader.TimeTToDateTime(this.m_miscInfo.ProcessCreateTime); } }
        public UInt32 ProcessUserTime { get { return this.m_miscInfo.ProcessUserTime; } }
        public UInt32 ProcessKernelTime { get { return this.m_miscInfo.ProcessKernelTime; } }
        public MiniDumpMiscInfoLevel MiscInfoLevel { get; protected set; } 
    }

    [Serializable]
    public class MiniDumpMiscInfo2 : MiniDumpMiscInfo
    {
        private MINIDUMP_MISC_INFO_2 _miscInfo2;

        internal MiniDumpMiscInfo2(MINIDUMP_MISC_INFO_2 miscInfo2)
            : base((MINIDUMP_MISC_INFO)miscInfo2)
        {
            this.MiscInfoLevel = MiniDumpMiscInfoLevel.MiscInfo2;

            this._miscInfo2 = miscInfo2;
        }
        public UInt32 ProcessorMaxMhz { get { return this._miscInfo2.ProcessorMaxMhz; } }
        public UInt32 ProcessorCurrentMhz { get { return this._miscInfo2.ProcessorCurrentMhz; } }
        public UInt32 ProcessorMhzLimit { get { return this._miscInfo2.ProcessorMhzLimit; } }
        public UInt32 ProcessorMaxIdleState { get { return this._miscInfo2.ProcessorMaxIdleState; } }
        public UInt32 ProcessorCurrentIdleState { get { return this._miscInfo2.ProcessorCurrentIdleState; } }
    }

    [Serializable]
    public class MiniDumpMiscInfo3 : MiniDumpMiscInfo2
    {
        private MINIDUMP_MISC_INFO_3 _miscInfo3;
        private Win32TimeZoneInformation _timeZoneInformation;

        internal MiniDumpMiscInfo3(MINIDUMP_MISC_INFO_3 miscInfo3)
            : base((MINIDUMP_MISC_INFO_2)miscInfo3)
        {
            this.MiscInfoLevel = MiniDumpMiscInfoLevel.MiscInfo3;

            this._miscInfo3 = miscInfo3;
            this._timeZoneInformation = new Win32TimeZoneInformation(miscInfo3.TimeZone);
        }

        public UInt32 ProcessIntegrityLevel { get { return this._miscInfo3.ProcessIntegrityLevel; } }
        public UInt32 ProcessExecuteFlags { get { return this._miscInfo3.ProcessExecuteFlags; } }
        public UInt32 ProtectedProcess { get { return this._miscInfo3.ProtectedProcess; } }
        public UInt32 TimeZoneId { get { return this._miscInfo3.TimeZoneId; } }
        public Win32TimeZoneInformation TimeZone { get { return this._timeZoneInformation; } }
    }

    [Serializable]
    public class MiniDumpMiscInfo4 : MiniDumpMiscInfo3
    {
        private MINIDUMP_MISC_INFO_4 _miscInfo4;

        internal MiniDumpMiscInfo4(MINIDUMP_MISC_INFO_4 miscInfo4)
            : base((MINIDUMP_MISC_INFO_3)miscInfo4)
        {
            this.MiscInfoLevel = MiniDumpMiscInfoLevel.MiscInfo4;
            this._miscInfo4 = miscInfo4;
        }

        public string BuildString { get { return _miscInfo4.BuildString; } }
        public string DbgBldStr { get { return _miscInfo4.DbgBldStr; } }
    }
}
