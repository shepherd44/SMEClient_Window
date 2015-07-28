using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME
{
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
        override public string ToString()
        {
            string temp = "System Information";
            temp += ":PlatformID:" + m_platformID.ToString();
            temp += ":ServicePack:" + m_servicePack;
            temp += ":OSVersion:" + m_OSVersion.ToString();
            temp += ":CLRVersion:" + m_CLRVersion.ToString();
            temp += ":Is64bitOS:" + m_Is64bitOS.ToString();
            temp += ":Is64bitProcess:" + m_Is64bitProcess.ToString();
            temp += ":PageSize:" + m_SystemPageSize.ToString();
            temp += ":TickCount:" + m_TickCount.ToString();
            return temp; 
        }
    }
}
