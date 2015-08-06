using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;
using System.Reflection;

namespace SME.SMECollect
{
    // Error 발생 당시 CallStack 정보 저장
    // string 단위로 method.filename.line
    public class SMECallstackInformation
    {
        List<SMECallStack> m_listCallstack = new List<SMECallStack>();
        private void AddCallStack(SMECallStack smecallstack) { m_listCallstack.Add(smecallstack); }
        
        //public SMECallstackInformation(Exception exception)
        //{
        //    StackTrace stacktrace = new StackTrace(true);
        //    StackFrame[] stackframes = stacktrace.GetFrames();
        //    foreach (StackFrame item in stackframes)
        //    {
        //        SMECallStack smecallstack = new SMECallStack(item);
        //        AddCallStack(smecallstack);
        //    }
        //    List<SMECallStack> listtemp = SMECallStack.ParseFromException(exception);
        //    if(listtemp != null)
        //        m_listCallstack.AddRange(listtemp);
        //}

        // 생성자
        // @stacktrace: null일 경우 thread 사용 안하는 경우로, 현재 Threa에서 Stacktrace 변수를 생성
        public SMECallstackInformation(StackTrace stacktrace)
        {
            StackFrame[] stackframes;
            if (stacktrace == null)
                stackframes = new StackTrace(true).GetFrames();
            else
                stackframes = stacktrace.GetFrames();
            
            foreach (StackFrame item in stackframes)
            {
                SMECallStack smecallstack = new SMECallStack(item);
                AddCallStack(smecallstack);
            }
        }

        public XElement ToXElement()
        {
            XElement xmldoc = new XElement("CallStackInformation");
            XElement domainstack = new XElement("DomainStack");
            xmldoc.Add(domainstack);
            for (int i = 0 ; i < m_listCallstack.Count; i++)
                domainstack.Add(m_listCallstack[i].ToXElement());
            
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

    
}
