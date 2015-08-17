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
        public static void Start()
        {
            listener = new SMEListener("127.0.0.1", 3000);
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
