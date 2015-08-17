using System;

using System.Xml.Linq;
using SME.SMECollect.Data;

namespace SME.SMEXML
{
    public class SMEXMLReader
    {
        #region Members
        private XDocument m_xmldocument = null;
        private XElement m_rootElement = null;
        public SMEProjectInformation ProjectInfo { get; protected internal set; }
        public SMESystemInformation SystemInfo { get; protected internal set; }
        public SMEExceptionInformation ExceptionInfo { get; protected internal set; }
        public SMECallstackInformation CallStackInfo { get; protected internal set; }
        #endregion

        #region Functions
        public void SaveToXML(string path)
        {
            m_xmldocument.Save(path);
        }

        public void LoadFromXML(string path)
        {
            m_xmldocument = XDocument.Load(path);
            m_rootElement = (XElement)m_xmldocument.FirstNode;
            if(m_rootElement.Name.ToString().Equals("SME"))
            {
                XElement el = (XElement)m_rootElement.FirstNode;
                ProjectInfo = new SMEProjectInformation(el);
                el = (XElement)el.NextNode;
                SystemInfo = new SMESystemInformation(el);
                el = (XElement)el.NextNode;
                ExceptionInfo = new SMEExceptionInformation(el);
                el = (XElement)el.NextNode;
                CallStackInfo = new SMECallstackInformation(el);

            }
            else
            {
                throw new Exception("SME XML 파일이 아닙니다.");
            }
        }
        #endregion

        #region Constructor
        // 생성자
        public SMEXMLReader() { }
        public SMEXMLReader(string loadpath)
        {
            LoadFromXML(loadpath);
        }
        public SMEXMLReader(SMEProjectInformation proinfo,
                            SMESystemInformation sysinfo,
                            SMEExceptionInformation exinfo,
                            SMECallstackInformation callstackinfo)
        {
            m_xmldocument = new XDocument();
            m_rootElement = new XElement("SME");
            m_xmldocument.Add(m_rootElement);
            m_rootElement.Add(proinfo.ToXElement());
            m_rootElement.Add(sysinfo.ToXElement());
            m_rootElement.Add(exinfo.ToXElement());
            m_rootElement.Add(callstackinfo.ToXElement());
        }
        #endregion
    }
}
