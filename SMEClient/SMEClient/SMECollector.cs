using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace SME
{
    class SMECollector
    {
        /// <summary>
        /// 예외 정보 처리 모으기
        /// </summary>
        /// <param name="exception"></param>
        public void CollectExceptionInformation(Exception exception)
        {
            CollectSystemInfo();
            CollectCallStack(exception);
            CollectExceptionInformation(exception);
            CollectProjectInfo();
        }
        private void CollectCallStack(Exception exception)
        {
            List<string> stacklist = new List<string>();
            StackTrace callstack = new StackTrace(true);


            stacklist.Add(exception.StackTrace);

        }
        private void CollectSystemInfo()
        {
            List<string> systemlist = new List<string>();
            OperatingSystem os = Environment.OSVersion;
        }
        private void CollectProjectInfo()
        {
        }
        private void CollectErrorInfo(Exception exception)
        {
            List<string> ErrorInfoList = new List<string>();

        }
        public SMECollector() { }
        private static SMESystemInformation m_sysInfo;
    }
    class SMESystemInformation
    {
        string m_OS;
        string m_OSVersion;
        string m_CLRVersion;
        bool m_Is64bitOS;
        bool m_Is64bitProcess;
        string m_SystemPageSize;
        string m_TickCount;

        public string ToXMLString() { string temp = ""; return temp; }
        public string ToString() { string temp = ""; return temp; }
    }
    
    class SMEProjectInformation
    {
        //projectName
        string m_Name;
        //projectVersion
        string m_Version;

        public string ToXMLString() { string temp = ""; return temp; }
        public string ToString() { string temp = m_Name+m_Version; return temp; }
    }

    /// <summary>
    /// Exception 정보 저장
    /// </summary>
    class SMEExceptionInformation
    {
        string m_exName;
        public string ExName
        {
            get { return m_exName; }
            set { m_exName = value; }
        }
        string m_exData;
        public string ExData
        {
            get { return m_exData; }
            set { m_exData = value; }
        }
        string m_exHelpLink;
        public string ExHelpLink
        {
            get { return m_exHelpLink; }
            set { m_exHelpLink = value; }
        }
        string m_exHResult;
        public string ExHResult
        {
            get { return m_exHResult; }
            set { m_exHResult = value; }
        }
        string m_exMessage;
        public string ExMessage
        {
            get { return m_exMessage; }
            set { m_exMessage = value; }
        }

        public string ToXMLString() { string temp = ""; return temp; }
        public string ToString() { string temp = ""; return temp; }
    }

    class SMECallstackInformation
    {
        public string ToXMLString() { string temp = ""; return temp; }
        public string ToString() { string temp = ""; return temp; }
    }
}
