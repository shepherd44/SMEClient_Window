using System;
using System.Collections;
using System.IO;
using System.Xml.Linq;

using SME.SMECollect;

namespace SME
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


    public class XMLHelper
    {
        public static XElement FindElement(XElement parent, string child)
        {
            IEnumerable childnodes = parent.Nodes();
            foreach (XElement item in childnodes)
            {
                if (item.Name.ToString().Equals(child))
                    break;
            }
            return null;
        }
        public static void PrintChildElement(XElement parent)
        {
            foreach (XElement item in parent.Nodes())
            {
                Console.WriteLine(item.Name);
            }
        }
        public static void PrintAllElement(XElement root)
        {
        }
    }
    
}