using SME.SMEDumpAnalyze.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SMEDumpAnalyze.MinidumpStream
{
    public class MiniDumpException
    {
        private MINIDUMP_EXCEPTION m_exception;
        private UInt64[] m_exceptionInformation;

        //생성자
        internal unsafe MiniDumpException(MINIDUMP_EXCEPTION exception)
        {
            m_exception = exception;
            m_exceptionInformation = new UInt64[windows.EXCEPTION_MAXIMUM_PARAMETERS];
            for (int i = 0; i < windows.EXCEPTION_MAXIMUM_PARAMETERS; i++)
                m_exceptionInformation[i] = exception.ExceptionInformation[i];
        }

        public UInt32 ExceptionCode { get { return this.m_exception.ExceptionCode; } }
        public UInt32 ExceptionFlags { get { return this.m_exception.ExceptionFlags; } }
        public UInt64 ExceptionRecordRaw { get { return this.m_exception.ExceptionRecord; } }
        public UInt64 ExceptionAddress { get { return this.m_exception.ExceptionAddress; } }
        public UInt32 NumberParameters { get { return this.m_exception.NumberParameters; } }
        public UInt64[] ExceptionInformation { get { return this.m_exceptionInformation; } }
        
        public static readonly UInt32 EXCEPTION_NONCONTINUABLE = 0x1;
    }
}
