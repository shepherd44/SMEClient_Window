using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;
using System.Collections.Specialized;

namespace SME
{
    /// <summary>
    /// 정보 모으기
    /// XML 형식
    /// -SME
    /// --project
    /// ---
    /// --Exception
    /// ---Exception 종류
    /// ---Data(제외)
    /// ---HelpLink
    /// ---HResult
    /// ---Message
    /// --CallStack(ex - A
    /// ---FuncName
    /// ----FileName
    /// ----Line
    /// </summary>
    class SMECollector
    {
        List<string> SMEContainer = new List<string>();
        private XmlDocument m_xmlDoc;

        public XmlDocument XmlDoc
        {
            get { return m_xmlDoc; }
            set { m_xmlDoc = value; }
        }

        public void ToXml() { }
        public void CollectStack() { }
        public void SaveXml(string path) { }

        public string XmlString
        {
            get { return m_xmlDoc.ToString(); }
        }

        // 생성자
        public SMECollector() { }
    }
}
