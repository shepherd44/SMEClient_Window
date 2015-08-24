using System;
using System.Data.SqlClient;
using DumpReader.MinidumpStream;

using SME.SMECollect;
using SME.SMECollect.Data;
using SME.SMEXML;
using DumpReader;

namespace SME.DB
{
    public class SMEDBManager : IDisposable
    {
        /// <summary>
        /// 사용법 : 우선 SMEServer에서 덤프파일들을 받고 덤프 파일 받은시간을 Dumps 테이블에 이름으로 저장한다.
        /// SMEDBManager.InsertDatabase() 함수를 이용하여 파라미터로 쿼리문을 넣어주면 된다.
        /// 그러면 자동으로 Dumps테이블의 기본키인 Dump_ID가 증가되므로 SMEDBManager.GetDumpID()함수를
        /// 이용하여 방금 저장한 덤프이름을 가진 Dump_ID를 받아 올 수 있다.
        /// 이 Dump_ID와 받은 덤프파일에서 분석한 SMEMiniDumpReader객체와 SMECollector 객체를 생성자 파라미터로
        /// 넣어준다.
        /// </summary>

        #region Members
        MiniDumpExceptionStream m_exceptionstream;
        MiniDumpModule[] m_module;
        MiniDumpMiscInfo m_miscinfo;
        MiniDumpBreakpadInfo m_breakpad;
        MiniDumpAssertionInfo m_assertion;
        int m_dumpid;
        SMEXMLReader m_smecollector;
        #endregion

        #region Constructors
        //생성자의 파라미터로 SMEMinidumpReader개체를 넣어준다.
        public SMEDBManager(SMEMiniDumpReader minidumpReader, int dump_id, int apikey)
        {
            //SMEMinidumpReader 객체를 받아 저장되어있는 데이터들을 전역변수로 지정한다.
            m_exceptionstream = minidumpReader.ExceptionStream;
            m_module = minidumpReader.Module;
            m_miscinfo = minidumpReader.MiscInfo;
            m_breakpad = minidumpReader.BreakpadInfo;
            m_assertion = minidumpReader.AssertionInfo;
            m_dumpid = dump_id;
            //클래스 데이터들 DB로 저장
            SaveNativeDumpData();
        }

        public SMEDBManager(string loadpath, int dump_id, int apikey)
        {
            m_dumpid = dump_id;
            m_smecollector = new SMEXMLReader(loadpath);
            SaveCSDumpData();
        }
        #endregion

        #region static Methods
        //클래스 안의 데이터들을 DB에 저장
        public void SaveNativeDumpData()
        {
            InsertDatabase(MakeQueryforException());
            InsertDatabase(MakeQueryforModule());
            InsertDatabase(MakeQueryforMiscInfo());
            InsertDatabase(MakeQueryforBreakpad());
            InsertDatabase(MakeQueryforAssertion());
        }

        public void SaveCSDumpData()
        {
            InsertDatabase(MakeQueryforCSProjectInfo());
            InsertDatabase(MakeQueryforCSSystemInfo());
            InsertDatabase(MakeQueryforCSExceptionInfo());
            if (m_smecollector.ExceptionInfo.InnerException != null)
            {
                int exception_id = GetExcetpionID(m_dumpid);
                InsertDatabase(MakeQueryforCSExceptionInfo(m_smecollector.ExceptionInfo.InnerException, exception_id));
            }
            InsertDatabase(MakeQueryforCSCallStackInfo());
        }
        #endregion

        #region Query for DumpsID
        private string MakeQueryforAddDumpsID()
        {
            string strCommand = "INSERT INTO Dumps(ProName, ProVersion, DumpsDump_ID) VALUES(";
            strCommand += "'" + m_smecollector.ProjectInfo.Name + "',";
            strCommand += "'" + m_smecollector.ProjectInfo.m_Version.ToString() + "',";
            strCommand += "'" + m_dumpid + "')";
            return strCommand;
        }
        #endregion

        #region Query for Minidump
        //1. ExceptionStream 데이터를 DB에 저장하기 위한 Query생성함수
        public string MakeQueryforException()
        {
            MiniDumpException exception = m_exceptionstream.ExceptionRecord;
            string strCommand = "INSERT INTO NExceptionInfo(ThreadID, ExceptionCode, ExceptionFlags, ExceptionAddress, NumberParameters, DataSizePretty, " +
                "ExceptionInfo1, ExceptionInfo2, ExceptionInfo3, ExceptionInfo4, ExceptionInfo5, ExceptionInfo6, ExceptionInfo7, ExceptionInfo8, ExceptionInfo9, " +
                "ExceptionInfo10, ExceptionInfo11, ExceptionInfo12, ExceptionInfo13, ExceptionInfo14, ExceptionInfo15, DumpsDump_ID)" +
                "VALUES (";
            strCommand += "'" + m_exceptionstream.ThreadId.ToString() + "',";
            strCommand += "'" + m_exceptionstream.ExceptionRecord.ExceptionCode.ToString("x") + "',";
            strCommand += "'" + m_exceptionstream.ExceptionRecord.ExceptionFlags.ToString() + "',";
            strCommand += "'" + m_exceptionstream.ExceptionRecord.ExceptionAddress.ToString("x") + "',";
            strCommand += "'" + m_exceptionstream.ExceptionRecord.NumberParameters.ToString() + "',";
            strCommand += "'" + m_exceptionstream.ThreadContext.DataSizePretty + "',";
            for (int i = 0; i < m_exceptionstream.ExceptionRecord.ExceptionInformation.Length; i++)
            {
                strCommand += "'" + m_exceptionstream.ExceptionRecord.ExceptionInformation[i].ToString("x") + "'";
                strCommand += ",";
            }
            strCommand += "'" + m_dumpid + "')";
            return strCommand;
        }

        //2. Module[] 데이터를 DB에 저장하기 위한 Query생성함수
        public string MakeQueryforModule()
        {
            int count = m_module.Length;
            string strFilePathAndName = string.Empty;
            string strFileVersion = string.Empty;
            string strCommand = "INSERT INTO NModule(PathAndFileName, FileVersion, DumpsDump_ID) VALUES('";
            for (int i = 0; i < m_module.Length; i++)
            {
                strFilePathAndName += m_module[i].PathAndFileName;
                strFilePathAndName += "?";
                strFileVersion += m_module[i].FileVersion;
                strFileVersion += "?";
            }
            strCommand += strFilePathAndName + "','" + strFileVersion + "','" + m_dumpid + "')";
            return strCommand;
        }

        //3. MiscInfo 데이터를 DB에 저장하기 위한 Query생성함수
        public string MakeQueryforMiscInfo()
        {
            string strCommand = "INSERT INTO NMiscInfo(SizeOfInfo, ProcessID, ProcessCreateTime, UserTime, KernelTime, ProcessorMaxMHz, CurrentMhz, MhzLimit,"+
                "MaxIdleState, CurrentIdleState, ProcessIntegrityLevel, ExecuteFlags, ProtectedProcess, TimeZoneId, TimeZone," +
                "BuildString, DbgBldStr, DumpsDump_ID) VALUES (";
            string SizeOfInfo = "";
            string ProcessID = "";
            string ProcessCreateTime = "";
            string UserTime = "";
            string KernelTime = "";
            string ProcessorMaxMHz = "";
            string CurrentMhz = "";
            string MhzLimit = "";
            string MaxIdleState = "";
            string CurrentIdleState = "";
            string ProcessIntegrityLevel = "";
            string ExecuteFlags = "";
            string ProtectedProcess = "";
            string TimeZoneId = "";
            string TimeZone = "";
            string BuildString = "";
            string DbgBldStr = "";

            if(m_miscinfo.MiscInfoLevel == MiniDumpMiscInfoLevel.MiscInfo)
            {
                SizeOfInfo = m_miscinfo.SizeOfInfo.ToString();
                ProcessID = m_miscinfo.ProcessId.ToString();
                ProcessCreateTime = m_miscinfo.ProcessCreateTime.ToString();
                UserTime = m_miscinfo.ProcessUserTime.ToString();
                KernelTime = m_miscinfo.ProcessKernelTime.ToString();
            }
            else if(m_miscinfo.MiscInfoLevel == MiniDumpMiscInfoLevel.MiscInfo2)
            {
                MiniDumpMiscInfo2 miscinfo2 = (MiniDumpMiscInfo2)m_miscinfo;
                ProcessorMaxMHz = miscinfo2.ProcessorMaxMhz.ToString();
                CurrentMhz = miscinfo2.ProcessorCurrentMhz.ToString();
                MhzLimit = miscinfo2.ProcessorMhzLimit.ToString();
                MaxIdleState = miscinfo2.ProcessorMaxIdleState.ToString();
                CurrentIdleState = miscinfo2.ProcessorCurrentIdleState.ToString();
            }
            else if(m_miscinfo.MiscInfoLevel == MiniDumpMiscInfoLevel.MiscInfo3)
            {
                MiniDumpMiscInfo3 miscinfo3 = (MiniDumpMiscInfo3)m_miscinfo;
                ProcessIntegrityLevel = miscinfo3.ProcessIntegrityLevel.ToString();
                ExecuteFlags = miscinfo3.ProcessExecuteFlags.ToString();
                ProtectedProcess = miscinfo3.ProtectedProcess.ToString();
                TimeZoneId = miscinfo3.TimeZoneId.ToString();
                TimeZone = miscinfo3.TimeZone.StandardName;
            }
            else if(m_miscinfo.MiscInfoLevel == MiniDumpMiscInfoLevel.MiscInfo4)
            {
                MiniDumpMiscInfo4 miscinfo4 = (MiniDumpMiscInfo4)m_miscinfo;
                BuildString = miscinfo4.BuildString;
                DbgBldStr = miscinfo4.DbgBldStr;
            }

            strCommand += "'"+SizeOfInfo+"','"+ProcessID+"','"+ProcessCreateTime+"','"+UserTime+"','"+KernelTime+"','"+ProcessorMaxMHz+"','"+CurrentMhz+
                "','"+MhzLimit+"','"+MaxIdleState+"','"+CurrentIdleState+"','"+ProcessIntegrityLevel+"','"+ExecuteFlags+"','"+ProtectedProcess+
                "','"+TimeZoneId+"','"+TimeZone+"','"+BuildString+"','"+DbgBldStr+"','"+m_dumpid+"')";
            return strCommand;
        }

        //4. Breakpad 데이터를 DB에 저장하기 위한 Query생성함수
        public string MakeQueryforBreakpad()
        {
            string strCommand = "INSERT INTO NBreakpad(Validate, DumpthreadID, RequestThreadID, DumpsDump_ID) VALUES(";
            strCommand += "'" + m_breakpad.validate + "',";
            strCommand += "'" + m_breakpad.dumpThreadId + "',";
            strCommand += "'" + m_breakpad.requestThreadId + "',";
            strCommand += "'" + m_dumpid + "')";
            return strCommand;
        }

        //5. AssertionInfo 데이터를 DB에 저장하기 위한 Query생성함수
        public string MakeQueryforAssertion()
        {
            string strCommand = "INSERT INTO NAssertion(Line, Type, Expression, Function, File, DumpsDump_ID) VALUES(";
            if(m_assertion != null)
            {
                strCommand += "'" + m_assertion.line + "',";
                strCommand += "'" + m_assertion.type + "',";
                strCommand += "'" + m_assertion.expression + "',";
                strCommand += "'" + m_assertion.function + "',";
                strCommand += "'" + m_assertion.file + "',";
                strCommand += "'" + m_dumpid + "')";
            }
            else
            {
                strCommand = "INSERT INTO NAssertion(DumpsDump_ID) VALUES(";
                strCommand += "'" + m_dumpid + "')";
            }
            return strCommand;
        }
        #endregion

        #region Query for CSDump
        private string MakeQueryforCSProjectInfo()
        {
            string strCommand = "INSERT INTO ProjectInfo(ProName, ProVersion, DumpsDump_ID) VALUES(";
            strCommand += "'" + m_smecollector.ProjectInfo.Name +"',";
            strCommand += "'" + m_smecollector.ProjectInfo.m_Version.ToString() + "',";
            strCommand += "'" + m_dumpid + "')";
            return strCommand;
        }

        private string MakeQueryforCSSystemInfo()
        {
            string strCommand = "INSERT INTO SystemInfo(PlatformID, ServicePack, OSVersion, CLRVersion, Is64BitOS, Is64BitProcess, DumpsDump_ID) VALUES(";
            strCommand += "'" + m_smecollector.SystemInfo.PlatformID.ToString() + "',";
            strCommand += "'" + m_smecollector.SystemInfo.ServicePack.ToString() + "',";
            strCommand += "'" + m_smecollector.SystemInfo.OSVersion.ToString() + "',";
            strCommand += "'" + m_smecollector.SystemInfo.CLRVersion.ToString() + "',";
            strCommand += "'" + m_smecollector.SystemInfo.Is64bitOS.ToString() + "',";
            strCommand += "'" + m_smecollector.SystemInfo.Is64bitProcess.ToString() + "',";
            strCommand += "'" + m_dumpid + "')";
            return strCommand;
        }

        private string MakeQueryforCSExceptionInfo()
        {
            string strCommand = "INSERT INTO ExceptionInfo(EName, HResult, HelpLink, Data, CallStack, DumpsDump_ID) VALUES(";
            strCommand += "'" + m_smecollector.ExceptionInfo.Name + "',";
            strCommand += "'" + m_smecollector.ExceptionInfo.Hresult + "',";
            strCommand += "'" + m_smecollector.ExceptionInfo.HelpLink + "',";
            strCommand += "'" + m_smecollector.ExceptionInfo.DataToString() + "',";
            strCommand += "'" + m_smecollector.ExceptionInfo.CallStackString + "',";
            strCommand += "'" + m_dumpid + "')";
            return strCommand;
        }

        private string MakeQueryforCSExceptionInfo(SMEExceptionInformation exinfo, int exceptionid)
        {
            string strCommand = "INSERT INTO ExceptionInfo(EName, HResult, HelpLink, Data, CallStack, InnerExp, DumpsDump_ID) VALUES(";
            strCommand += "'" + exinfo.Name + "',";
            strCommand += "'" + exinfo.Hresult + "',";
            strCommand += "'" + exinfo.HelpLink + "',";
            strCommand += "'" + exinfo.DataToString() + "',";
            strCommand += "'" + exinfo.CallStackString + "',";
            strCommand += "'" + exceptionid + "',";
            strCommand += "'" + m_dumpid + "')";
            return strCommand;
        }

        private string MakeQueryforCSCallStackInfo()
        {
            string strCommand = "INSERT INTO CallStackInfo(Stack, DumpsDump_ID) VALUES(";
            strCommand += "'" + m_smecollector.CallStackInfo.ToString() + "',";
            strCommand += "'" + m_dumpid + "')";
            return strCommand;
        }
        #endregion

        #region Method
        public static int GetDumpID()
        {
            //SMEServer에서 덤프를 받아 DB에 저장할 때 데이터베이스로부터 Dump_ID를 받아오는 함수

            string strConn = "Data Source=192.168.0.94;Initial Catalog=SME;User ID=SME;Password=bit1234";
            SqlConnection conn = new SqlConnection(strConn);
            string strCmd = "SELECT Dump_ID FROM Dumps";
            SqlCommand cmd = new SqlCommand(strCmd, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int retLastVal = -1;
            while(reader.Read()) //SqlDataReader를 다음 레코드로 이동한다.
                retLastVal = int.Parse(reader[""].ToString());//마지막 데이터가 방금 입력한 Dump의 Dump_ID가 된다.
            conn.Close();
            return retLastVal;

        }

        public static void InsertDatabase(string Insert_Query_String)
        {
            string strConn = "Data Source=192.168.0.94;Initial Catalog=SME;User ID=SME;Password=bit1234";
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            string strCmd = Insert_Query_String;
            SqlCommand cmd = new SqlCommand(strCmd, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public int GetExcetpionID(int dumpid)
        {
            string strConn = "Data Source=192.168.0.94;Initial Catalog=SME;User ID=SME;Password=bit1234";
            SqlConnection conn = new SqlConnection(strConn);
            string strCmd = "SELECT Exp_ID FROM ExceptionInfo WHERE DumpsDump_ID = '" + dumpid + "'";
            SqlCommand cmd = new SqlCommand(strCmd, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int retVal = 0;
            while(reader.Read())
            {
                retVal = int.Parse(reader[0].ToString());
            }
            
            conn.Close();
            return retVal;
        }
        #endregion

        #region Disposable Interface
        void IDisposable.Dispose()
        {
            
        }
        #endregion
    }
}
