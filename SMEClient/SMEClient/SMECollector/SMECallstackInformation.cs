using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;

namespace SME
{
    // Error 발생 당시 CallStack 정보 저장
    // string 단위로 method.filename.line
    public class SMECallstackInformation
    {
        List<SMECallStack> m_listCallstack = new List<SMECallStack>();

        public void AddCallStack(SMECallStack smecallstack) { m_listCallstack.Add(smecallstack); }
        
        public SMECallstackInformation(Exception exception)
        {
            StackTrace stacktrace = new StackTrace(true);
            StackFrame[] stackframes = stacktrace.GetFrames();
            foreach (StackFrame item in stackframes)
            {
                SMECallStack smecallstack = new SMECallStack(item);
                AddCallStack(smecallstack);
            }
            AddCallStack(new SMECallStack(exception));
        }
        public XElement ToXElement()
        {
            XElement xmldoc = new XElement("CallStackInformation");
            for (int i = 0; i < m_listCallstack.Count; i++)
            {
                SMECallStack temp = m_listCallstack[i];
                xmldoc.Add(temp.ToXElement());
            }
            return xmldoc;
        }
        override public string ToString()
        {
            string temp = "CallStackInformation\n";
            foreach (SMECallStack item in m_listCallstack)
            {
                temp += item.ToString();
                temp += "\n";
            }
            return temp;
        }
    }

    public class SMECallStack
    {
        string m_method;
        
        string m_file;
        int m_line;
        public SMECallStack(string method, string file, int line)
        {
            m_method = method;
            m_file = file;
            m_line = line;
        }
        public SMECallStack(StackFrame stackframe)
        {
            m_method = stackframe.GetMethod().Name;
            m_file = stackframe.GetFileName();
            m_line = stackframe.GetFileLineNumber();
        }

        // 생성자
        public SMECallStack(Exception exception)
        {
            string exceptionstack = exception.StackTrace;
            if(exceptionstack == null)
            {
                m_method = "null";
                m_file = "null";
                m_line = 0;
                return;
            }
            string[] seperators = new string[] { " 위치: ", "파일 ", ":줄 " };
            string[] array;
            array = exceptionstack.Split(seperators, StringSplitOptions.None);
            m_method = array[1];
            m_file = array[2];
            m_line = int.Parse(array[3]);
        }
        public XElement ToXElement()
        {
            XElement xmldoc = new XElement("Stack",
                                    new XElement("Method", m_method),
                                    new XElement("File", m_file),
                                    new XElement("Line", m_line.ToString()
                                  ));
            return xmldoc;
        }
        public override string ToString()
        {
            string temp = "CallStack";
            temp += ":Method:" + m_method;
            temp += ":FileName:" + m_file;
            temp += ":Line:" + m_line;
            return temp;
        }
    }
}
