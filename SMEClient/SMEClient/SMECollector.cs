using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Collections;

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
        public SMECollector()
        {
        }

        //
        private static SMESystemInformation m_sysInfo = new SMESystemInformation();
    }

    //현재 system의 정보 수집
    class SMESystemInformation
    {
        PlatformID m_platformID;
        string m_servicePack;
        Version m_OSVersion;
        Version m_CLRVersion;
        bool m_Is64bitOS;
        bool m_Is64bitProcess;
        int m_SystemPageSize;
        int m_TickCount;

        public SMESystemInformation()
        {
            OperatingSystem os = Environment.OSVersion;

            m_platformID = os.Platform;
            m_OSVersion = os.Version;
            m_CLRVersion = Environment.Version;
            m_servicePack = os.ServicePack;
            m_Is64bitOS = Environment.Is64BitOperatingSystem;
            m_Is64bitProcess = Environment.Is64BitProcess;
            m_SystemPageSize = Environment.SystemPageSize;
            m_TickCount = Environment.TickCount;
        }
        public string ToXMLString() { string temp = ""; return temp; }
        public string ToString() { string temp = ""; return temp; }
    }
    
    // 현재 프로그램의 정보 수집
    class SMEProjectInformation
    {
        //projectName
        string m_Name;
        //projectVersion
        string m_Version;

        public SMEProjectInformation(string name, string version)
        {
            m_Name = name;
            m_Version = version;
        }
        public string ToXMLString() { string temp = ""; return temp; }
        public string ToString() { string temp = m_Name+m_Version; return temp; }
    }

    // 발생한 예외 정보 수집
    class SMEExceptionInformation
    {
        string m_exName;
        IDictionary m_exData;
        string m_exHelpLink;
        int m_exHResult;
        string m_exMessage;
        
        public SMEExceptionInformation(Exception exception)
        {
            m_exName = exception.GetType().ToString();
            m_exData = exception.Data;
            m_exHelpLink = exception.HelpLink;
            m_exHResult = exception.HResult;
            m_exMessage = exception.Message;
        }
        public string ToXMLString() { string temp = ""; return temp; }
        public string ToString() { string temp = ""; return temp; }
    }

    //Error 발생 당시 CallStack 정보 저장
    class SMECallstackInformation
    {
        List<string> m_listCallstack = new List<string>();

        public void AddCallStack(string callstack) { m_listCallstack.Add(callstack); }

        public SMECallstackInformation()
        {

        }
        public Byte[] ToByteArray() { return null; }
        public string ToXMLString() 
        {
            string temp = ""; return temp; }
        public string ToString()
        {
            string temp = "";
            foreach (string item in m_listCallstack)
	        {
		        temp += item;
                temp += "\n";
	        }
            return temp;
        }
    }
}
