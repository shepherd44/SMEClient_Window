using System.Xml.Linq;
using SME.SMECollect;

namespace SME.SMEXML
{
    public class SMEXMLWriter
    {
        #region Members
        private XDocument m_xmldocument = null;
        private XElement m_rootElement = null;
        #endregion

        public void SaveToXML(string path)
        {
            m_xmldocument.Save(path);
        }

        public void LoadFromXML(string path)
        {
            m_xmldocument = XDocument.Load(path);
            m_rootElement = (XElement)m_xmldocument.FirstNode;
        }

        #region Constructor
        // 생성자
        public SMEXMLWriter() { }
        public SMEXMLWriter(string loadpath)
        {
            m_xmldocument = XDocument.Load(loadpath);
            m_rootElement = (XElement)m_xmldocument.FirstNode;
        }
        public SMEXMLWriter(SMEProjectInformation proinfo,
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
