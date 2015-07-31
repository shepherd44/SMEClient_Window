using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SME
{
    // 발생한 예외 정보 수집
    public class SMEExceptionInformation
    {
        string m_exName = null;
        IDictionary m_exData = null;
        string m_exHelpLink = null;
        int m_exHResult = 0;
        string m_exMessage = null;

        public SMEExceptionInformation(Exception exception)
        {
            m_exName = exception.GetType().ToString();
            m_exData = exception.Data;
            m_exHelpLink = exception.HelpLink != null ? exception.HelpLink : "";
            m_exHResult = exception.HResult;
            m_exMessage = exception.Message != null ? exception.Message : "";
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
