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
 *     <PageSize></PageSize>
 *     <TickCount></TickCount>
 *   </SystemInformation>
 *   <ExceptionInformation>
 *     <Name></Name>
 *     <Data>
 *       <Key>Value</Key>
 *     </Data>
 *     <Hresult></Hresult>
 *     <HelpLink></HelpLink>
 *     <Message></Message>
 *   </ExceptionInformation>
 *   <CallStackInformation>
 *     <Stack>
 *       <Method></Method>
 *       <FileName></FileName>
 *       <Line></Line>
 *     </Stack>
 *   </CallStackInformation>
 * </SME>
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Diagnostics;
using System.Collections;

namespace SME
{
    public class SMECollector
    {
        // Collect Error Information
        // 는 class 생성과 동시에 만듬.
        // Exception information, CallStack Information은 예외 발생시 생성
        // xml 파일 쓰기 전 세마포어로 락을 걸어 정보를 모을때까지 기다림
        public void CollectErrorInformation(object e)
        {
            m_CollectSemaphore = new Semaphore(0, 1);
            Exception exception = (Exception)e;
            CollectProjectInfo();
            CollectCallStack(exception);
            CollectExceptionInfo(exception);
            m_CollectSemaphore.Release(1);
        }
        private void CollectCallStack(Exception exception)
        {
            m_callstackinfo = new SMECallstackInformation(exception);
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

        public void XMLSave(string path)
        {
            m_CollectSemaphore.WaitOne();
            m_smexmlwriter = new SMEXMLWriter(m_projectinfo,
                                                m_sysInfo,
                                                m_exceptioninfo,
                                                m_callstackinfo);
            m_smexmlwriter.SaveToFile(path);
            m_CollectSemaphore.Release(1);
        }
        public void XMLSave()
        {
            m_CollectSemaphore.WaitOne();
            m_smexmlwriter = new SMEXMLWriter(m_projectinfo,
                                                m_sysInfo,
                                                m_exceptioninfo,
                                                m_callstackinfo);
            m_smexmlwriter.SaveToFile(k_XMLfilepath);
            m_CollectSemaphore.Release(1);
        }

        //생성자
        public SMECollector() { }
        public SMECollector(Exception exception)
        {
            m_CollectThread = new Thread(new ParameterizedThreadStart(CollectErrorInformation));
            m_SaveXMLThread = new Thread(new ThreadStart(XMLSave));
            m_CollectThread.Start(exception);
            m_SaveXMLThread.Start();
        }

        public override string ToString()
        {
            return base.ToString();
        }
        
        // 정보 저장용 class들
        // SMESytemInformation : 플랫폼, os, clr 정보 등을 가지고 있음, 시작과 동시에 생성
        // SMEProjectInformation : 프로젝트의 버전 및 이름을 저장
        // SMEExceptionInformation : 예외 정보 저장
        // SMECallstackInformation : 콜스택 저장(함수이름, 발생파일이름, 발생라인넘버)
        private SMESystemInformation m_sysInfo = new SMESystemInformation();
        private SMEProjectInformation m_projectinfo = null;
        private SMEExceptionInformation m_exceptioninfo = null;
        private SMECallstackInformation m_callstackinfo = null;
        // 정보를 모아줄 쓰레드
        private Thread m_CollectThread = null;
        // 정보를 모으는 중 xml파일로 저장하는걸 방지하기 위해 semaphore 생성
        private Semaphore m_CollectSemaphore = null;
        // xml관리 함수
        private SMEXMLWriter m_smexmlwriter = null;
        private Thread m_SaveXMLThread = null;
        
        // 상수
        const string k_XMLfilepath = "C:\\Dumps\\CS.xml";

        public void ConsoleTest()
        {
            Console.WriteLine("start-------------------------------------------------------");
            Console.WriteLine(m_sysInfo.ToString());
            Console.WriteLine(m_projectinfo.ToString());
            Console.WriteLine(m_exceptioninfo.ToString());
            Console.WriteLine(m_callstackinfo.ToString());
            Console.WriteLine("end--------------------------------------------------------");
        }
    }
    
}
