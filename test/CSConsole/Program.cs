using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 미니덤프 생성
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

using Utility;
using managedCal;
using google_breakpad;

using System.Xml;
using System.Collections.Specialized;



namespace SME
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //--c++ lib 에러 체크 test
            //구글 브레이크 패드 시작
            //google_breakpad.Class1 cs = new Class1();
            //c++ lib 래퍼클래스 생성, Add 함수에서 에러 발생함.
            //managedCal.AddCalWrap test = new AddCalWrap();
            //test.Add(1, 2);
            //-- 스택정보 뽑아보기 test
            cerror er = new cerror();
            er.error_gen();

            
            try
            {
                //string temp = null;  
                //temp.ToString();  
                throw new NullReferenceException();
                
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Main: " + e.StackTrace.ToString());
                //Utility.MiniDump.TryDump("minidump.dmp", Utility.MiniDumpType.Normal);   
            }

            HttpHelper.func();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    class cerror
    {
        public void error_gen()
        {
            //error_gen();
            try
            {
                //string temp = null;  
                //temp.ToString();  
                //throw new ArgumentNullException("name");
                throw new NullReferenceException();

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.StackTrace.ToString());
                //Console.WriteLine(e.Source.ToString());
                StackTrace stackTrace = new StackTrace(true);
                StackFrame[] stackFrames = stackTrace.GetFrames();
                foreach (StackFrame r in stackFrames)
                {
                    Console.WriteLine("cerror: Filename: {0} Method: {1} Line: {2} Column: {3}  ",
                    r.GetFileName(), r.GetMethod(), r.GetFileLineNumber(),
                    r.GetFileColumnNumber());   // write method name
                }
                Console.WriteLine("cerror: " + e.StackTrace);
                //Utility.MiniDump.TryDump("minidump.dmp", Utility.MiniDumpType.Normal);
            }
        }
    }
}
