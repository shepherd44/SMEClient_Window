using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SME.SMECollect
{
    // 발생한 예외 정보 수집
    public class SMEExceptionInformation
    {
        string m_exName = null;
        IDictionary m_exData = null;
        string m_exHelpLink = null;
        int m_exHResult = 0;
        string m_exMessage = null;
        List<SMECallStack> m_listCallstack = new List<SMECallStack>();
        Exception m_innerException = null;

        public SMEExceptionInformation(Exception exception)
        {
            m_exName = exception.GetType().ToString();
            m_exData = exception.Data;
            m_exHelpLink = exception.HelpLink != null ? exception.HelpLink : "";
            m_exHResult = exception.HResult;
            m_exMessage = exception.Message != null ? exception.Message : "";
            m_listCallstack = SMECallStack.ParseFromException(exception);
            m_innerException = exception.InnerException != null ? exception.InnerException : null;
        }

        public XElement ToXElement()
        {
            XElement xmldoc = new XElement("ExeptionInformation",
                                new XElement("Name", m_exName),
                                new XElement("Data", DataToXElement()),
                                new XElement("Hresult", m_exData.ToString()),
                                new XElement("HelpLink", m_exHelpLink),
                                new XElement("Message", m_exMessage)
                                );
            XElement exceptionstack = new XElement("ExceptionStack");
            xmldoc.Add(exceptionstack);
            for (int i = 0; i < m_listCallstack.Count; i++)
                exceptionstack.Add(m_listCallstack[i].ToXElement());

            return xmldoc;
        }

        override public string ToString()
        {

            string temp = "Exception Information\n";
            temp += ":Name:"+m_exName;
            temp += ":Data:";
            if (m_exData != null)
            {
                foreach (DictionaryEntry item in m_exData)
                {
                    temp += ":" + item.Key + ":" + item.Value;
                }
            }
            temp += ":Hresult:" + m_exHResult;
            temp += ":HelpLink:" + m_exHelpLink;
            temp += ":Message:" + m_exMessage;
            return temp;
        }

        XElement DataToXElement()
        {
            XElement xmldoc = new XElement("Data");
            if (m_exData == null)
                return xmldoc;
            foreach (DictionaryEntry item in m_exData)
            {
                xmldoc.Add(item.Key.ToString(), item.Value.ToString());
            }
            return xmldoc;
        }
        
    }
}
