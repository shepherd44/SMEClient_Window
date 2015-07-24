using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using google_breakpad;

namespace SME
{
    public class SMEClient
    {
        private AppDomain m_currentDomain; // 객체를 생성해준 앱도메인
        private bool m_useCpp; // 프로세스 안에 Cpp를 사용하면 true
        private String m_APIKey; // Server에서 프로젝트를 구별해 줄 APIKey
        private breakpadWrapper m_Wrapper;
        //SMEClient 생성자, 호출해 준 프로세스의 기본 앱도메인을 받아온다.
        public SMEClient(AppDomain currentDomain, bool useCpp, String APIKey)
        {
            m_currentDomain = currentDomain;

            //UnhandledException 이벤트를 추가한다.
            m_currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnHandledExceptionHandler);

            //useCpp가 TRUE일 때, google_breakpad.breakpadWrapper 객체생성
            m_useCpp = useCpp;
            if (m_useCpp)
            {
                m_Wrapper = new breakpadWrapper();
            }

            //Server에 전송할 APIKey
            m_APIKey = APIKey;


        }
        
        // 예외처리 이벤트가 넘어오면 할 일들을 해 주는 함수
        static void UnHandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            //예외처리 조합 SMECollector

            //전송 Sender


            #region StackTrace,Message Out
            ///StackTrace st = new StackTrace(true);
            ///StackFrame[] stf = st.GetFrames();
            ///foreach (var r in stf)
            ///{
            ///    Console.WriteLine("Filename: {0} Method: {1} Line: {2} Column: {3}  ",
            ///    r.GetFileName(), r.GetMethod(), r.GetFileLineNumber(),
            ///    r.GetFileColumnNumber());   // write method name
            ///}
            ///Exception e = (Exception)args.ExceptionObject;
            ///Console.WriteLine("MyHandler caught : " + e.Message);
            ///Console.WriteLine("Runtime terminating : {0}", args.IsTerminating);
            #endregion

        }

    }
}
