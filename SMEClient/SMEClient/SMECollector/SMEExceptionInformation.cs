using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME
{
    // 발생한 예외 정보 수집
    class SMEExceptionInformation
    {
        string m_exName;
        IDictionary m_exData;
        string m_exHelpLink;
        int m_exHResult;
        string m_exMessage;

        public SMEExceptionInformation(Exception exception)
        {
            m_exName = exception.GetType().ToString();
            m_exData = exception.Data;
            m_exHelpLink = exception.HelpLink;
            m_exHResult = exception.HResult;
            m_exMessage = exception.Message;
        }
        public string ToXMLString() { string temp = ""; return temp; }
        override public string ToString()
        {

            string temp = "Exception Information\n";
            temp += ":Name:"+m_exName;
            temp += ":Data:";
            foreach (DictionaryEntry item in m_exData)
            {
                temp += ":" + item.Key + ":" + item.Value;
            }
            temp += ":Hresult:" + m_exHResult;
            temp += ":HelpLink:" + m_exHelpLink;
            temp += ":Message:" + m_exMessage;
            return temp;
        }
    }
}
