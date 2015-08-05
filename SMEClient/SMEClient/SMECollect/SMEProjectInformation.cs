using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SME.SMECollect
{
    // 현재 프로그램의 정보 수집
    public class SMEProjectInformation
    {
        //projectName
        string m_Name;
        //projectVersion
        Version m_Version = null;

        public SMEProjectInformation(string name, Version version)
        {
            m_Name = name != null ? name : "";
            m_Version = version != null ? version : new Version("0.0");
        }
        public XElement ToXElement()
        {
            XElement xmldoc = new XElement("ProjectInformation",
                                    new XElement("Name", m_Name),
                                    new XElement("Version", m_Version.ToString())
                                    ); 
            return xmldoc;
        }
        override public string ToString() { string temp = "Project Name:" + m_Name + ":Version:"+m_Version; return temp; }
    }
}
