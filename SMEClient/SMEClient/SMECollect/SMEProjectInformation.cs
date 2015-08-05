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
        #region Members
        //projectName
        public string Name { get; set; }
        //projectVersion
        Version m_Version = null;
        #endregion

        #region Constructor
        public SMEProjectInformation(string name, Version version)
        {
            Name = name != null ? name : "My Project";
            m_Version = version != null ? version : new Version("0.0");
        }
        
        public SMEProjectInformation(SMEProjectInformation proinfo)
        {
            Name = proinfo.Name;
            m_Version = new Version(proinfo.m_Version.ToString());
        }
        #endregion

        #region Functions
        public XElement ToXElement()
        {
            XElement xmldoc = new XElement("ProjectInformation",
                                    new XElement("Name", Name),
                                    new XElement("Version", m_Version.ToString())
                                    ); 
            return xmldoc;
        }

        override public string ToString() { string temp = "Project Name:" + Name + ":Version:" + m_Version; return temp; }
        #endregion
    }
}
