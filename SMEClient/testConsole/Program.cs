using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

using SME;
using google_breakpad;
using managedCal;

namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("test Console");
            Console.WriteLine("-------------------------------------------");
            SME.SMEClient smeclient = new SMEClient(AppDomain.CurrentDomain, false, "test key");

            //testclass.stacktracetest();
            //testclass.msdnenvtestcode();
            //testclass.systeminfotest();
            //testclass.exceptioninfotest(new NullReferenceException());
            testclass.UnhandleExceptiontest();
            
            Console.WriteLine();
        }
    }
    class testclass
    {
        public static void UnhandleExceptiontest()
        {
            throw new NullReferenceException();
        }
        public static void stacktracetest()
        {
            
            Console.WriteLine(Environment.StackTrace);

            StackTrace st = new StackTrace(true);
            StackFrame[] sfs = st.GetFrames();
            foreach (StackFrame item in sfs)
            {
                
                Console.WriteLine(item.GetMethod().Name);
                Console.WriteLine(item.GetFileName());
                Console.WriteLine(item.ToString());
            }
        }
        public static void msdnenvtestcode()
        {
            String str;
            String nl = Environment.NewLine;
        //
            Console.WriteLine();
            Console.WriteLine("-- Environment members --");

        //  Invoke this sample with an arbitrary set of command line arguments.
            Console.WriteLine("CommandLine: {0}", Environment.CommandLine);

            String[] arguments = Environment.GetCommandLineArgs();
            Console.WriteLine("GetCommandLineArgs: {0}", String.Join(", ", arguments));

        //  <-- Keep this information secure! -->
            Console.WriteLine("CurrentDirectory: {0}", Environment.CurrentDirectory);

            Console.WriteLine("ExitCode: {0}", Environment.ExitCode);

            Console.WriteLine("HasShutdownStarted: {0}", Environment.HasShutdownStarted);

        //  <-- Keep this information secure! -->
            Console.WriteLine("MachineName: {0}", Environment.MachineName);

            Console.WriteLine("NewLine: {0}  first line{0}  second line{0}  third line",
                                  Environment.NewLine);

            Console.WriteLine("OSVersion: {0}", Environment.OSVersion.ToString());

            Console.WriteLine("StackTrace: '{0}'", Environment.StackTrace);

        //  <-- Keep this information secure! -->
            Console.WriteLine("SystemDirectory: {0}", Environment.SystemDirectory);

            Console.WriteLine("TickCount: {0}", Environment.TickCount);

        //  <-- Keep this information secure! -->
            Console.WriteLine("UserDomainName: {0}", Environment.UserDomainName);

            Console.WriteLine("UserInteractive: {0}", Environment.UserInteractive);

        //  <-- Keep this information secure! -->
            Console.WriteLine("UserName: {0}", Environment.UserName);

            Console.WriteLine("Version: {0}", Environment.Version.ToString());

            Console.WriteLine("WorkingSet: {0}", Environment.WorkingSet);

        //  No example for Exit(exitCode) because doing so would terminate this example.

        //  <-- Keep this information secure! -->
            String query = "My system drive is %SystemDrive% and my system root is %SystemRoot%";
            str = Environment.ExpandEnvironmentVariables(query);
            Console.WriteLine("ExpandEnvironmentVariables: {0}  {1}", nl, str);

            Console.WriteLine("GetEnvironmentVariable: {0}  My temporary directory is {1}.", nl,
                                   Environment.GetEnvironmentVariable("TEMP"));

            Console.WriteLine("GetEnvironmentVariables: ");
            IDictionary	environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry de in environmentVariables)
                {
                Console.WriteLine("  {0} = {1}", de.Key, de.Value);
                }

            Console.WriteLine("GetFolderPath: {0}", 
                         Environment.GetFolderPath(Environment.SpecialFolder.System));

            String[] drives = Environment.GetLogicalDrives();
            Console.WriteLine("GetLogicalDrives: {0}", String.Join(", ", drives));
        }
        public static void systeminfotest()
        {
            PlatformID m_platformID;
            string m_servicePack;
            Version m_OSVersion;
            Version m_CLRVersion;
            bool m_Is64bitOS;
            bool m_Is64bitProcess;
            int m_SystemPageSize;
            int m_TickCount;

            OperatingSystem os = Environment.OSVersion;

            m_platformID = os.Platform;
            m_OSVersion = os.Version;
            m_CLRVersion = Environment.Version;
            m_servicePack = os.ServicePack;
            m_Is64bitOS = Environment.Is64BitOperatingSystem;
            m_Is64bitProcess = Environment.Is64BitProcess;
            m_SystemPageSize = Environment.SystemPageSize;
            m_TickCount = Environment.TickCount;

            Console.WriteLine(Environment.StackTrace);
            Console.WriteLine(os.ToString());
            Console.WriteLine(m_platformID.ToString());
            Console.WriteLine(m_OSVersion.ToString());
            Console.WriteLine(m_CLRVersion.ToString());

        }
        public static void exceptioninfotest(Exception exception)
        {
            string m_exName;
            IDictionary m_exData = exception.Data;
            string m_exHelpLink;
            int m_exHResult;
            string m_exMessage;
            m_exName = exception.GetType().ToString();
            m_exData = exception.Data;
            m_exHelpLink = exception.HelpLink;
            m_exHResult = exception.HResult;
            m_exMessage = exception.Message;

            Console.WriteLine(m_exName);
            foreach (DictionaryEntry item in m_exData)
            {
                Console.WriteLine("{0} : {1}", item.Key, item.Value);
            }

            Console.WriteLine(exception.StackTrace);

            try
            {
                throw new NullReferenceException();
            }
            catch (Exception e)
            {
                string exceptionstack = e.StackTrace;
                char[] charray = e.StackTrace.ToCharArray();
                string[] seperators = new string[] { "   위치: ", "파일 ", ":줄 " };
                string[] array;
                array = exceptionstack.Split(seperators, StringSplitOptions.None);
                Console.WriteLine(e.StackTrace);
            }
        }
        public static void breakpadWrappertest()
        {
            //google-breakpad wrapper class test
            google_breakpad.breakpadWrapper eh = new breakpadWrapper();
            //error gen in c++lib
            managedCal.AddCalWrap error_gen = new AddCalWrap();
            error_gen.Add(1, 2);
            // write minidump
            google_breakpad.breakpadWrapper.WriteMinidump();
        }
    }
}
