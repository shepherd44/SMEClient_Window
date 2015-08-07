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
        #region Members
        string m_exName = null;
        IDictionary m_exData = null;
        string m_exHelpLink = null;
        int m_exHResult = 0;
        string m_exMessage = null;
        List<SMECallStack> m_listCallstack = null;
        SMEExceptionInformation m_innerException = null;
        #endregion

        #region Constructors
        public SMEExceptionInformation(Exception exception)
        {
            m_exName = exception.GetType().ToString();
            m_exData = exception.Data;
            m_exHelpLink = exception.HelpLink != null ? exception.HelpLink : "";
            m_exHResult = exception.HResult;
            m_exMessage = exception.Message != null ? exception.Message : "";
            m_listCallstack = SMECallStack.ParseFromException(exception);
            m_innerException = exception.InnerException != 
                null ? new SMEExceptionInformation(exception) : null;
        }

        public SMEExceptionInformation(XElement xelement)
        {
            LoadFromXElement(xelement);
        }
        #endregion

        #region Functions
        public XElement ToXElement()
        {
            XElement xmldoc = new XElement("ExeptionInformation",
                                new XElement("Name", m_exName),
                                new XElement(DataToXElement()),
                                new XElement("Hresult", m_exHResult),
                                new XElement("HelpLink", m_exHelpLink),
                                new XElement("Message", m_exMessage)
                                );

            // Exception stacktrace 처리
            XElement exceptionstack = new XElement("ExceptionStack");
            if(m_listCallstack != null)
                for (int i = 0; i < m_listCallstack.Count; i++)
                    exceptionstack.Add(m_listCallstack[i].ToXElement());
            xmldoc.Add(exceptionstack);
            
            // innerexception 처리
            XElement innerexception = new XElement("InnerException");
            if(m_innerException != null)
                innerexception.Add(m_innerException.ToXElement());
            xmldoc.Add(innerexception);
            
            return xmldoc;
        }

        public void LoadFromXElement(XElement xelement)
        {
            if (xelement.Name.ToString().Equals("ExeptionInformation"))
            {
                XElement el = (XElement)xelement.FirstNode;
                m_exName = el.Value;
                
                // Data
                el = (XElement)el.NextNode;
                m_exData = new Dictionary<string, string>();
                for (int i = 0; i < el.Elements().Count(); i++)
                {
                    XElement temp = el.Elements().ElementAt(i);
                    m_exData.Add(temp.Name.ToString(), temp.Value);
                }
                el = (XElement)el.NextNode;
                m_exHResult = int.Parse(el.Value);
                el = (XElement)el.NextNode;
                m_exHelpLink = el.Value;
                el = (XElement)el.NextNode;
                m_exMessage = el.Value;
                
                el = (XElement)el.NextNode;
                m_listCallstack = new List<SMECallStack>();
                for (int i = 0; i < el.Elements().Count(); i++)
			    {
                    m_listCallstack.Add(new SMECallStack(el.Elements().ElementAt(i)));
			    }

                el = (XElement)el.NextNode;
                if (el.FirstNode != null)
                    m_innerException = new SMEExceptionInformation((XElement)el.FirstNode);
                else
                    m_innerException = null;
            }
            else
                throw new Exception("This XElement is not ExceptionInformation XElement");
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
                xmldoc.Add(item.Key.ToString(), item.Value.ToString());

            return xmldoc;
        }
        #endregion
    }
}
