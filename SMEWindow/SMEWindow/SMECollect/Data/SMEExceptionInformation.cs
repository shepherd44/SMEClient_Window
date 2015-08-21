using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SME.SMECollect.Data
{
    // 발생한 예외 정보 수집
    [Serializable]
    public class SMEExceptionInformation
    {
        #region Members
        public string Name { get; protected internal set; }
        public string HelpLink { get; protected internal set; }
        public int Hresult { get; protected internal set; }
        public string Message { get; protected internal set; }
        public IDictionary Data { get; protected internal set; }
        public string CallStackString { get; protected internal set; }
        List<SMECallStack> m_listCallstack = null;
        public SMEExceptionInformation InnerException { get; protected internal set; }

        public List<SMECallStack> ListCallStack { get { return m_listCallstack; } }
        #endregion

        #region Constructors
        public SMEExceptionInformation(Exception exception)
        {
            Name = exception.GetType().ToString();
            Data = exception.Data;
            HelpLink = exception.HelpLink != null ? exception.HelpLink : "";
            Hresult = exception.HResult;
            Message = exception.Message != null ? exception.Message : "";
            CallStackString = exception.StackTrace;
            m_listCallstack = SMECallStack.ParseFromException(exception);
            InnerException = exception.InnerException != null ? new SMEExceptionInformation(exception) : null;
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
                                new XElement("Name", Name),
                                new XElement(DataToXElement()),
                                new XElement("Hresult", Hresult),
                                new XElement("HelpLink", HelpLink),
                                new XElement("Message", Message)
                                );

            // Exception stacktrace 처리
            XElement exceptionstack = new XElement("ExceptionStack");
            if(m_listCallstack != null)
                for (int i = 0; i < m_listCallstack.Count; i++)
                    exceptionstack.Add(m_listCallstack[i].ToXElement());
            xmldoc.Add(exceptionstack);
            
            // innerexception 처리
            XElement innerexception = new XElement("InnerException");
            if(InnerException != null)
                innerexception.Add(InnerException.ToXElement());
            xmldoc.Add(innerexception);
            
            return xmldoc;
        }

        public void LoadFromXElement(XElement xelement)
        {
            if (xelement.Name.ToString().Equals("ExeptionInformation"))
            {
                XElement el = (XElement)xelement.FirstNode;
                Name = el.Value;
                
                // Data
                el = (XElement)el.NextNode;
                Data = new Dictionary<string, string>();
                for (int i = 0; i < el.Elements().Count(); i++)
                {
                    XElement temp = el.Elements().ElementAt(i);
                    Data.Add(temp.Name.ToString(), temp.Value);
                }
                el = (XElement)el.NextNode;
                Hresult = int.Parse(el.Value);
                el = (XElement)el.NextNode;
                HelpLink = el.Value;
                el = (XElement)el.NextNode;
                Message = el.Value;
                
                el = (XElement)el.NextNode;
                m_listCallstack = new List<SMECallStack>();
                for (int i = 0; i < el.Elements().Count(); i++)
			    {
                    m_listCallstack.Add(new SMECallStack(el.Elements().ElementAt(i)));
			    }

                el = (XElement)el.NextNode;
                if (el.FirstNode != null)
                    InnerException = new SMEExceptionInformation((XElement)el.FirstNode);
                else
                    InnerException = null;
            }
            else
                throw new Exception("This XElement is not ExceptionInformation XElement");
        }

        XElement DataToXElement()
        {
            XElement xmldoc = new XElement("Data");
            if (Data == null)
                return xmldoc;
            foreach (DictionaryEntry item in Data)
                xmldoc.Add(item.Key.ToString(), item.Value.ToString());

            return xmldoc;
        }

        public string DataToString()
        {
            string datastring = "";
            if (Data != null)
            {
                foreach (DictionaryEntry item in Data)
                {
                    datastring += ":" + item.Key + ":" + item.Value;
                }
            }
            return datastring;
        }
        #endregion

        #region override
        override public string ToString()
        {
            string temp = "Exception Information\n";
            temp += ":Name:" + Name;
            temp += ":Data:";
            if (Data != null)
            {
                foreach (DictionaryEntry item in Data)
                {
                    temp += ":" + item.Key + ":" + item.Value;
                }
            }
            temp += ":Hresult:" + Hresult;
            temp += ":HelpLink:" + HelpLink;
            temp += ":Message:" + Message;
            return temp;
        }
        #endregion
    }
}
