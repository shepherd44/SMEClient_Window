using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using google_breakpad;
using SME.SMECollect;
using System.Runtime.ExceptionServices;

namespace SME
{
    public class SMEClient
    {
    #region members
        // 객체를 생성해준 앱도메인
        private AppDomain m_currentDomain;
        // cpp 감시를 시작할 지 결정
        // 감시를 시작할 경우 true 설정
        public bool UseCPP { get; set; }
        // Server에서 프로젝트를 구별해 줄 APIKey
        public String APIKEY { get; protected internal set; }
        // breakpad initializing wrapper
        private breakpadWrapper m_Wrapper = null;
        // exception information collector
        private static SMECollector m_SMECollector = null;
    #endregion
        
    #region 생성자
        // SMEClient 생성자
        // @currentapplication: Form을 사용할 경우 대입 아닐 경우 null
        // @currentDomain: 호출해 준 프로세스의 기본 앱도메인
        public SMEClient(bool usecpp, String APIKey)
        {
            // 우선 순위 조정, 최고 우선순위로
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            
            // Application 영역 핸들러 설정
            // Application 영역 Unhandled Exception Mode 선택
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            // Application 영역 UI Thread Unhandled Exception Handler 설정
            Application.ThreadException += new ThreadExceptionEventHandler(SMEThreaqdExceptionHandler);

            // AppDomain 영역 핸들러 설정
            m_currentDomain = AppDomain.CurrentDomain;
            // First Chance Handled Exception Handler 설정
            m_currentDomain.FirstChanceException += SMEFirstChanceExceptionHandler;
            // Non-UI Thread Unhandled Exception Handler 설정
            m_currentDomain.UnhandledException += new UnhandledExceptionEventHandler(SMEUnHandledExceptionHandler);
            
            // google_breakpad 설정
            UseCPP = usecpp;
            if (UseCPP)
                m_Wrapper = new breakpadWrapper();
            
            // Server에 전송할 APIKey
            APIKEY = APIKey;
        }		 
	#endregion

    #region Exception Handlers
        // Unhandled Exception 처리 함수
        private static void SMEUnHandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;
            //예외처리 조합 SMECollector
            StackTrace stacktrace = new StackTrace(true);
            m_SMECollector = new SMECollector(exception, stacktrace);
            
            //전송 Sender
        }

        // Unhandled, Handled 두 경우 모두 반응하게 된다. 
        private static void SMEFirstChanceExceptionHandler(object sender, FirstChanceExceptionEventArgs f)
        {
            Exception exception = (Exception)f.Exception;
            Console.WriteLine("FirstChanceException event raised in {0}: {1}",
            AppDomain.CurrentDomain.FriendlyName, f.Exception.Message);
        }

        private static void SMEThreaqdExceptionHandler(object sender, ThreadExceptionEventArgs t)
        {
            Exception exception = t.Exception;
            Console.WriteLine("ThreadException");
        }
    #endregion

    }
}
