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
        static SMEListener listener;
        #endregion

        #region Constructors
        static SMEServer()
        {
            
        }
        #endregion

        #region Methods
        public static void Start(string ip, int port)
        {
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
