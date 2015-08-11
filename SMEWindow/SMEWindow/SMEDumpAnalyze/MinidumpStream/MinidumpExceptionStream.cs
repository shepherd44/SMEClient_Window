using SME.SMEDumpAnalyze.Native;
using System;

namespace SME.SMEDumpAnalyze.MinidumpStream
{
    public class MiniDumpExceptionStream
    {
        private MINIDUMP_EXCEPTION_STREAM m_exceptionStream;
        private MiniDumpException m_exceptionRecord;
        private MiniDumpLocationDescriptor m_threadContext;

        internal MiniDumpExceptionStream(MINIDUMP_EXCEPTION_STREAM exceptionStream)
        {
            m_exceptionStream = exceptionStream;
            m_exceptionRecord = new MiniDumpException(exceptionStream.ExceptionRecord);
            m_threadContext = new MiniDumpLocationDescriptor(exceptionStream.ThreadContext);
        }

        public UInt32 ThreadId { get { return this.m_exceptionStream.ThreadId; } }
        public MiniDumpException ExceptionRecord { get { return this.m_exceptionRecord; } }
        public MiniDumpLocationDescriptor ThreadContext { get { return this.m_threadContext; } }
        
    }
}
