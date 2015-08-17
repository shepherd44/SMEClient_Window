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
        // stacktrace(thread가 달라서 미리 생성할 필요가 있음)
        [NonSerialized]
        StackTrace m_ErrorCallStack = null;
        // 정보를 모아줄 쓰레드
        [NonSerialized]
        private Thread m_CollectThread = null;
        // 정보를 모으는 중 xml파일로 저장하는걸 방지하기 위해 semaphore 생성
        [NonSerialized]
        private Semaphore m_CollectSemaphore = null;
        // xml관리 함수
        [NonSerialized]
        private SMEXMLWriter m_smexmlwriter = null;
        [NonSerialized]
        private Thread m_SaveXMLThread = null;

        // 덤프xml파일 저장 위치
        // 파일명 날짜로 변경 필요
        const string m_XMLFolderPath = "C:\\Dumps\\CS\\";
        DateTime m_currentTime = DateTime.Now;
        string m_XMLFilePath = null;
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
            m_CollectSemaphore.Release(1);
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

        public void SaveToXML(string path)
        {
            m_CollectSemaphore.WaitOne();
            m_smexmlwriter = new SMEXMLWriter(m_projectinfo,
                                                m_sysInfo,
                                                m_exceptioninfo,
                                                m_callstackinfo);
            m_smexmlwriter.SaveToXML(path);
            m_CollectSemaphore.Release(1);
        }

        public void SaveToXML()
        {
            m_CollectSemaphore.WaitOne();
            m_smexmlwriter = new SMEXMLWriter(m_projectinfo,
                                              m_sysInfo,
                                              m_exceptioninfo,
                                              m_callstackinfo);
            DateToFilePath();
            m_smexmlwriter.SaveToXML(m_XMLFilePath);
            m_CollectSemaphore.Release(1);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        private void DateToFilePath()
        {
            m_XMLFilePath = m_XMLFolderPath;
            m_XMLFilePath += m_projectinfo.Name.Trim() + "-";
            m_XMLFilePath += m_projectinfo.m_Version.ToString() + "-";
            m_XMLFilePath += m_currentTime.ToShortDateString() + "-";
            if (m_currentTime.Hour < 10)
                m_XMLFilePath += "0" + m_currentTime.Hour.ToString() + "-";
            else
                m_XMLFilePath += m_currentTime.Hour.ToString() + "-";
            if (m_currentTime.Minute < 10)
                m_XMLFilePath += "0" + m_currentTime.Minute.ToString() + "-";
            else
                m_XMLFilePath += m_currentTime.Minute.ToString() + "-";
            if (m_currentTime.Second < 10)
                m_XMLFilePath += "0" + m_currentTime.Second.ToString();
            else
                m_XMLFilePath += m_currentTime.Second.ToString();
            m_XMLFilePath += ".xml";
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
        // 생성자
        public SMECollector() { }
        
        public SMECollector(Exception exception, StackTrace stack, SMEProjectInformation smeproinfo)
        {
            m_projectinfo = smeproinfo;
            m_ErrorCallStack = stack;
            m_CollectThread = new Thread(new ParameterizedThreadStart(CollectErrorInformation));
            m_CollectThread.Name = "CollectThread";
            m_SaveXMLThread = new Thread(new ThreadStart(SaveToXML));
            m_SaveXMLThread.Name = "SaveThread";

            // collect Thread 안에서 semaphore를 초기화 할 경우 세마포어가 초기화 되기 전
            // Save Thread 안에서 초기화 되지 않은 세마포어를 가지고 wait를 진행하는 경우가 발생.
            m_CollectSemaphore = new Semaphore(0, 1);
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
            m_XMLFilePath = null;

            if (m_CollectThread != null)
            {
                m_CollectThread.Abort();
                m_CollectThread = null;
            }
            if(m_CollectSemaphore != null)
            {
                m_CollectSemaphore.Close();
                m_CollectSemaphore.Dispose();
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
