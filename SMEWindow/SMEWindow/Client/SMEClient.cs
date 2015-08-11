using google_breakpad;
using SME.SMECollect;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Forms;

[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]

namespace SME.Client
{
    public class SMEClient
    {
    #region members
        public static SMEProjectInformation ProjectInfo { get; set; }
        // cpp 감시를 시작할 지 결정
        // 감시를 시작할 경우 true 설정
        public bool UseCPP { get; set; }
        // Server에서 프로젝트를 구별해 줄 APIKey
        public static String APIKEY { get; set; }
        // breakpad initializing wrapper
        private static breakpadWrapper m_Wrapper = null;
    #endregion
        
    #region 생성자
        // SMEClient 생성자
        // @currentapplication: Form을 사용할 경우 대입 아닐 경우 null
        // @currentDomain: 호출해 준 프로세스의 기본 앱도메인
        public SMEClient(string proname, Version proversion, bool usecpp, String APIKey)
        {
            
            // 우선 순위 조정, 최고 우선순위로
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            
            // Application 영역 핸들러 설정
            // Application 영역 Unhandled Exception Mode 선택
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            // Application 영역 UI Thread Unhandled Exception Handler 설정
            Application.ThreadException += new ThreadExceptionEventHandler(SMEThreaqdExceptionHandler);

            // AppDomain 영역 핸들러 설정
            AppDomain CurrentDomain = AppDomain.CurrentDomain;
            // First Chance Exception Handler 설정
            CurrentDomain.FirstChanceException += SMEFirstChanceExceptionHandler;
            // Non-UI Thread Unhandled Exception Handler 설정
            CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(SMEUnHandledExceptionHandler);
            
            // google_breakpad 설정
            UseCPP = usecpp;
            if (UseCPP)
                m_Wrapper = new breakpadWrapper();

            ProjectInfo = new SMEProjectInformation(proname, proversion);
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
            SMECollector smecollector = new SMECollector(exception, stacktrace, ProjectInfo);
            
            //파일 전송

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
            StackTrace stacktrace = new StackTrace(true);
            SMECollector smecollector = new SMECollector(exception, stacktrace, ProjectInfo);
        }
    #endregion
    }
}
