using google_breakpad;
using SME.SMECollect.Data;
using SME.SMECollect;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Forms;

using NetLib;

[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]

namespace SME.Client
{
    public class SMEClient
    {
    #region members
        public static SMEProjectInformation ProjectInfo { get; set; }
        // Naive Exception
        public static bool IsCatchNE { get; protected internal set; }
        // Handled Exception
        public static bool IsCatchHE { get; protected internal set; }
        // 서버로 전송할 것인지 선택
        public static bool IsSend { get; protected internal set; }
        public static string ServerIP { get; protected internal set; }
        public static int ServerPort { get; protected internal set; }
        // Server에서 프로젝트를 구별해 줄 APIKey
        public static String APIKEY { get; protected internal set; }
        // breakpad initializing wrapper
        private static breakpadWrapper m_Wrapper;
    #endregion
        
    #region 생성자
        // SMEClient 생성자
        // @currentapplication: Form을 사용할 경우 대입 아닐 경우 null
        // @currentDomain: 호출해 준 프로세스의 기본 앱도메인
        public SMEClient(string proname,
            Version proversion,
            bool iscatchne,
            bool iscatchhe,
            String APIKey)
        {
            // Project에 대응하는 api key(project id)
            APIKEY = APIKey;
            IsCatchNE = iscatchne;
            IsCatchHE = iscatchhe;
            IsSend = false;
            ServerIP = string.Empty;
            ServerPort = -1;
            ProjectInfo = new SMEProjectInformation(proname, proversion);
            Initialize();
        }

        public SMEClient(string proname,
            Version proversion,
            bool iscatchne,
            bool iscatchhe,
            String APIKey,
            string serverip,
            int serverport)
        {
            // Project에 대응하는 api key(project id)
            APIKEY = APIKey;
            IsCatchNE = iscatchne;
            IsCatchHE = iscatchhe;
            IsSend = true;
            ServerIP = serverip;
            ServerPort = serverport;
            ProjectInfo = new SMEProjectInformation(proname, proversion);
            Initialize();
        }
	#endregion

    #region Methods
        private void Initialize()
        {
            // 우선 순위 조정, 최고 우선순위로
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            // Application 영역 Unhandled Exception Mode 선택
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            
            AppDomain CurrentDomain = AppDomain.CurrentDomain;
            // Non-UI Thread Unhandled Exception Handler 설정(기본 핸들러)
            if (!IsCatchHE)
                CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(SMEUnHandledExceptionHandler);
            
            // Application 영역 UI Thread Unhandled Exception Handler 설정
            Application.ThreadException += new ThreadExceptionEventHandler(SMEThreadExceptionHandler);
            if (IsCatchHE)
                CurrentDomain.FirstChanceException += SMEFirstChanceExceptionHandler;
            if (IsCatchNE)
                m_Wrapper = new breakpadWrapper(int.Parse(APIKEY));
        }
        public static void CollectError(string sme)
        {
            StackTrace stacktrace = new StackTrace(true);
            SMECollector smecollector = new SMECollector(new Exception(""), stacktrace, ProjectInfo);
        }
    #endregion

    #region Exception Handlers
        // Unhandled Exception 처리 함수
        private static void SMEUnHandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;
            if (exception == null)
                exception = new Exception("null");
            StackTrace stacktrace = new StackTrace(true);
            SMECollector smecollector = new SMECollector(exception, stacktrace, ProjectInfo);
            
            //파일 전송
            if (IsSend)
            {
                smecollector.SendToServer(ServerIP, ServerPort);
            }
            
        }

        // Unhandled, Handled 두 경우 모두 반응하게 된다. 
        private static void SMEFirstChanceExceptionHandler(object sender, FirstChanceExceptionEventArgs f)
        {
            Exception exception = (Exception)f.Exception;
            if (exception == null)
                exception = new Exception("null");
            StackTrace stacktrace = new StackTrace(true);
            SMECollector smecollector = null;
            try
            {
                smecollector = new SMECollector(exception, stacktrace, ProjectInfo);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

            //파일 전송
            if (IsSend)
            {
                if (smecollector != null)
                    smecollector.SendToServer(ServerIP, ServerPort);
            }
        }

        private static void SMEThreadExceptionHandler(object sender, ThreadExceptionEventArgs t)
        {
            Exception exception = t.Exception;
            if (exception == null)
                exception = new Exception("null");
            StackTrace stacktrace = new StackTrace(true);
            SMECollector smecollector = null;
            try
            {
                smecollector = new SMECollector(exception, stacktrace, ProjectInfo);
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

            //파일 전송
            if(IsSend)
            {
                if(smecollector != null)
                    smecollector.SendToServer(ServerIP, ServerPort);
            }
        }
    #endregion
    }
}
