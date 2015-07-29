using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SME
{
    class XMLDoc
    {
        XmlDocument m_xmldocument = new XmlDocument();

        public void AddNode() { }
         
    }
    public class SMEXMLWriter
    {
        XElement m_xmldocument = new XElement("SME");
        
        //
        public void SaveToFile(string path)
        {
            m_xmldocument.Save(path);
        }

        public SMEXMLWriter(SMEProjectInformation proinfo,
                            SMESystemInformation sysinfo,
                            SMEExceptionInformation exinfo,
                            SMECallstackInformation callstackinfo)
        {
            m_xmldocument.Add(proinfo.ToXElement());
            m_xmldocument.Add(sysinfo.ToXElement());
            m_xmldocument.Add(exinfo.ToXElement());
            m_xmldocument.Add(callstackinfo.ToXElement());
        }
    }
    
}