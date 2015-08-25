using System;
using System.Threading;

using SME.SMENet;

namespace SME.Server
{
    // Server
    public class SMEServer
    {
        #region Members
        //static ThreadPool m_ThreadPool;
        public static String DBServerConnection { get; protected internal set; }
        static SMEListener listener;
        public static AfterReceive AfterR;
        #endregion

        #region Constructors
        static SMEServer()
        {
            
        }
        #endregion

        #region Methods
        public static void Start(string ip, int port, string dbconn)
        {
            DBServerConnection = dbconn;
            listener = new SMEListener(ip, port);
        }

        public static void Close()
        {
            listener.Dispose();
        }
        #endregion
        #region ThreadCallback
        
        #endregion

        
    }
}
