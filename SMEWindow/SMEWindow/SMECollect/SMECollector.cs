/**************************************************************
 * SME Collector
 **************************************************************
 * XML 구조
 * <SME>
 *   <ProjectInformation>
 *     <Name></Name>
 *     <Version></Version>
 *   </ProjectInformation>
 *   <SystemInformation>
 *     <PlatformID></PlatformID>
 *     <ServicePack></ServicePack>
 *     <OSVersion></OSVersion>
 *     <CLRVersion></CLRVersion>
 *     <Is64BitOS></Is64BitOS>
 *     <IS64BitProcess></Is64BitProcess>
 *   </SystemInformation>
 *   <ExceptionInformation>
 *      <Name></Name>
 *      <Data>
 *          <Key>Value</Key>
 *      </Data>
 *      <Hresult></Hresult>
 *      <HelpLink></HelpLink>
 *      <Message></Message>
 *      <ExceptionStack>
 *          <Stack>
 *              <Method></Method>
 *              <FileName></FileName>
 *              <Line></Line>
 *          </Stack>
 *      </ExceptionStack>
 *      <InnerException>
 *          <ExceptionInformation>
 *          ...
 *          </ExceptionInformation>
 *      </InnerException>
 *   </ExceptionInformation>
 *   <CallStackInformation>
 *      <DomainStack>
 *          <Stack>
 *              <Method></Method>
 *              <FileName></FileName>
 *              <Line></Line>
 *          </Stack>
 *      </DomainStack>
 *   </CallStackInformation>
 * </SME>
 ***************************************************************/

using System;
using System.Threading;

using System.Diagnostics;
using SME.SMEXML;
using NetLib;

namespace SME.SMECollect
{
    using Data;
    [Serializable]
    public class SMECollector : IDisposable
    {
        #region Members
        // 정보 저장용 class들
        // SMESytemInformation : 플랫폼, os, clr 정보 등을 가지고 있음, 시작과 동시에 생성
        // SMEProjectInformation : 프로젝트의 버전 및 이름을 저장
        // SMEExceptionInformation : 예외 정보 저장
        // SMECallstackInformation : 콜스택 저장(함수이름, 발생파일이름, 발생라인넘버)
        private SMESystemInformation m_sysInfo = new SMESystemInformation();
        private SMEProjectInformation m_projectinfo = null;
        private SMEExceptionInformation m_exceptioninfo = null;
        private SMECallstackInformation m_callstackinfo = null;

        public SMESystemInformation SysInfo { get { return m_sysInfo; } }
        public SMEProjectInformation ProjectInfo { get { return m_projectinfo; } }
        public SMEExceptionInformation ExceptionInfo { get { return m_exceptioninfo; } }
        public SMECallstackInformation CallStackInfo { get { return m_callstackinfo; } }

        // stacktrace(thread가 달라서 미리 생성할 필요가 있음)
        [NonSerialized]
        StackTrace m_ErrorCallStack = null;
        // 정보를 모아줄 쓰레드
        [NonSerialized]
        private Thread m_CollectThread = null;
        // 정보를 모으는 중 xml파일로 저장하는걸 방지하기 위해 semaphore 생성
        public Semaphore CollectSemaphore { get; set; }
        // xml관리 함수
        [NonSerialized]
        private SMEXMLWriter m_smexmlwriter = null;
        [NonSerialized]
        private Thread m_SaveXMLThread = null;
        public Semaphore SaveSemaphore { get; protected internal set; }

        // 덤프xml파일 저장 위치
        // 파일명 날짜로 변경 필요
        DateTime m_currentTime = DateTime.Now;
        public string XMLFolderPath { get; set; }
        public string XMLFileName { get; protected internal set; }
        public string XMLFilePath { get; protected internal set; }
        #endregion

        #region Functions
        // Collect Error Information
        // 는 class 생성과 동시에 만듬.
        // Exception information, CallStack Information은 예외 발생시 생성
        // xml 파일 쓰기 전 세마포어로 락을 걸어 정보를 모을때까지 기다림
        public void CollectErrorInformation(object e)
        {
            // Collect 작업 시작
            Exception exception = (Exception)e;
            //CollectProjectInfo();
            CollectExceptionInfo(exception);
            CollectCallStack(exception);
            // Collect 작업 끝
            // Semaphore lock 해제 -> Save 작업 시작
            CollectSemaphore.Release(1);
        }

        // m_ErrorCallStack = 에러가 발생한 thread의 StackTrace
        private void CollectCallStack(Exception exception)
        {
            m_callstackinfo = new SMECallstackInformation(m_ErrorCallStack);
        }

        private void CollectProjectInfo()
        {
            //fixme//
            //프로젝트의 이름과 버전을 받아둘 방법 생각
            m_projectinfo = new SMEProjectInformation("testApp", null);
            
        }

        private void CollectExceptionInfo(Exception exception)
        {
            m_exceptioninfo = new SMEExceptionInformation(exception);
        }

        private void SaveToXML(string path)
        {
            CollectSemaphore.WaitOne();
            m_smexmlwriter = new SMEXMLWriter(m_projectinfo,
                                                m_sysInfo,
                                                m_exceptioninfo,
                                                m_callstackinfo);
            m_smexmlwriter.SaveToXML(path);
            CollectSemaphore.Release(1);
            SaveSemaphore.Release(1);

        }

        private void SaveToXML()
        {
            CollectSemaphore.WaitOne();
            m_smexmlwriter = new SMEXMLWriter(m_projectinfo,
                                              m_sysInfo,
                                              m_exceptioninfo,
                                              m_callstackinfo);
            DateToFileName();
            try
            {
                m_smexmlwriter.SaveToXML(XMLFilePath);
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            CollectSemaphore.Release(1);
            SaveSemaphore.Release(1);

        }

        public override string ToString()
        {
            return base.ToString();
        }

        private void DateToFileName()
        {
            XMLFileName = string.Empty;
            XMLFileName += m_projectinfo.Name.Trim() + "-";
            XMLFileName += m_projectinfo.m_Version.ToString() + "-";
            XMLFileName += m_currentTime.ToShortDateString() + "-";
            if (m_currentTime.Hour < 10)
                XMLFileName += "0" + m_currentTime.Hour.ToString() + "-";
            else
                XMLFileName += m_currentTime.Hour.ToString() + "-";
            if (m_currentTime.Minute < 10)
                XMLFileName += "0" + m_currentTime.Minute.ToString() + "-";
            else
                XMLFileName += m_currentTime.Minute.ToString() + "-";
            if (m_currentTime.Second < 10)
                XMLFileName += "0" + m_currentTime.Second.ToString();
            else
                XMLFileName += m_currentTime.Second.ToString();
            XMLFileName += ".xml";

            XMLFilePath = string.Format("{0}{1}", XMLFolderPath, XMLFileName);
        }

        public async void SendToServer(string ServerIP, int ServerPort, int apikey)
        {
            SaveSemaphore.WaitOne();
            TCPSender tcpsender = new TCPSender(ServerIP, ServerPort, XMLFilePath, XMLFileName, apikey);
            //tcpsender.FileSend();
            //tcpsender.Dispose();
        }
        //public void ConsoleTest()
        //{
        //    Console.WriteLine("start-------------------------------------------------------");
        //    Console.WriteLine("Current stack trace");
        //    Console.WriteLine(m_sysInfo.ToString());
        //    Console.WriteLine(m_projectinfo.ToString());
        //    Console.WriteLine(m_exceptioninfo.ToString());
        //    Console.WriteLine(m_callstackinfo.ToString());
        //    Console.WriteLine("end--------------------------------------------------------");
        //}
        #endregion

        #region Constructor        
        public SMECollector(Exception exception, StackTrace stack, SMEProjectInformation smeproinfo)
        {
            XMLFolderPath = string.Format("C:\\Dumps\\CS");
            m_projectinfo = smeproinfo;
            m_ErrorCallStack = stack;
            m_CollectThread = new Thread(new ParameterizedThreadStart(CollectErrorInformation));
            m_CollectThread.Name = "CollectThread";
            m_SaveXMLThread = new Thread(new ThreadStart(SaveToXML));
            m_SaveXMLThread.Name = "SaveThread";

            // collect Thread 안에서 semaphore를 초기화 할 경우 세마포어가 초기화 되기 전
            // Save Thread 안에서 초기화 되지 않은 세마포어를 가지고 wait를 진행하는 경우가 발생.
            CollectSemaphore = new Semaphore(0, 1);
            SaveSemaphore = new Semaphore(0, 1);
            m_CollectThread.Start(exception);
            m_SaveXMLThread.Start();
        }
        #endregion

        #region IDispose Interface
        void IDisposable.Dispose()
        {
            m_sysInfo = null;
            m_projectinfo = null;
            m_exceptioninfo = null;
            m_callstackinfo = null;
            m_ErrorCallStack = null;

            m_smexmlwriter = null;
            XMLFilePath = null;

            if (m_CollectThread != null)
            {
                m_CollectThread.Abort();
                m_CollectThread = null;
            }
            if(CollectSemaphore != null)
            {
                CollectSemaphore.Close();
                CollectSemaphore.Dispose();
            }
            
            if(m_SaveXMLThread != null)
            {
                m_SaveXMLThread.Abort();
                m_SaveXMLThread = null;
            }
                
        }
        #endregion
    }
}
