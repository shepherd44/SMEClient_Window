using SME.SMEDumpAnalyze.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SMEDumpAnalyze.MinidumpStream
{
    public unsafe class MiniDumpAssertionInfo
    {
        private MDRawAssertionInfo m_assertionInfo;
        private UInt16[] m_expression;
        private UInt16[] m_function;
        private UInt16[] m_file;

        internal unsafe MiniDumpAssertionInfo(MDRawAssertionInfo assertionInfo)
        {
            m_assertionInfo = assertionInfo;

            //구조체 내의 fixed된 배열의 정보를 읽어오는 방법 : 
            // 클래스 내의 field로 배열을 선언 후 생성자에서 파라미터로 받아온
            // 값을 for문을 이용해 저장을 해 준다.
            m_expression = new UInt16[128];
            m_function = new UInt16[128];
            m_file = new UInt16[128];
            for (int i = 0; i < 128; i++)
                m_expression[i] = assertionInfo.expression[i];
            for (int i = 0; i < 128; i++)
                m_function[i] = assertionInfo.function[i];
            for (int i = 0; i < 128; i++)
                m_file[i] = assertionInfo.file[i];
            
        }

        public UInt16[] expression { get { return this.m_expression; } }
        public UInt16[] function { get { return this.m_function; } }
        public UInt16[] file { get { return this.m_file; } }
        public UInt32 line { get { return this.line; } }
        public UInt32 type { get { return this.type; } }
    }
}
