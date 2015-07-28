/**************************************************************
 * SME Collector
 * 
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
 * 
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
        //예외 정보 모으기
        // 시스템 정보는 class 생성과 동시에 만듬.
        public void CollectErrorInformation(object e)
        {
            Exception exception = (Exception)e;
            CollectProjectInfo();
            CollectCallStack(exception);
            CollectExceptionInfo(exception);
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

        //생성자
        public SMECollector() { }
        public SMECollector(Exception exception)
        {
            m_CollectThread = new Thread(new ParameterizedThreadStart(CollectErrorInformation));
            m_CollectThread.Start(exception);
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
        private Thread m_CollectThread = null;

        public void ConsoleTest()
        {
            Console.WriteLine(m_sysInfo.ToString());
            Console.WriteLine(m_projectinfo.ToString());
            Console.WriteLine(m_exceptioninfo.ToString());
            Console.WriteLine(m_callstackinfo.ToString());
        }
        private Thread m_ConsoleThread = null;
    }
    
}
