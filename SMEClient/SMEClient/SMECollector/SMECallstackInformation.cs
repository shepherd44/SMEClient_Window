using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SME
{
    // Error 발생 당시 CallStack 정보 저장
    // string 단위로 method.filename.line
    class SMECallstackInformation
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
        public Byte[] ToByteArray() { return null; }
        public string ToXMLString()
        {
            string temp = ""; return temp;
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

    class SMECallStack
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
        public SMECallStack(Exception exception)
        {
            string exceptionstack = exception.StackTrace;
            string[] seperators = new string[] { " 위치: ", "파일 ", ":줄 " };
            string[] array;
            array = exceptionstack.Split(seperators, StringSplitOptions.None);
            m_method = array[1];
            m_file = array[2];
            m_line = int.Parse(array[3]);
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
