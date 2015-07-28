using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME
{
    // 현재 프로그램의 정보 수집
    class SMEProjectInformation
    {
        //projectName
        string m_Name;
        //projectVersion
        Version m_Version;

        public SMEProjectInformation(string name, Version version)
        {
            m_Name = name;
            m_Version = version;
        }
        public string ToXMLString() { string temp = ""; return temp; }
        override public string ToString() { string temp = "Project Name:" + m_Name + ":Version:"+m_Version; return temp; }
    }
}
