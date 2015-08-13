using System;
//Tcp를 이용해 파일을 보낼 때 필요한 namespaces
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace SME.SMENet
{
    //public delegate void AfterReceive();

    public class SMEListener : ISMEListener, IDisposable
    {
        #region Members
        private TcpListener tcpListener;
        private Thread listenThread;
        private IPAddress ipAddress; //Server의 IP
        private int Port;
        //const int MaxWorkingNum = 10;
        //Queue<SMEAsyncReceiver> waitList = new Queue<SMEAsyncReceiver>();
        //List<SMEAsyncReceiver> pauseList = new List<SMEAsyncReceiver>();
        //SMEAsyncReceiver[] workingList = new SMEAsyncReceiver[MaxWorkingNum];

        //public static AfterReceive afterReceive;
        //private Object thislock = new Object();
        #endregion

        #region Constructors
        public SMEListener(string ip, int port)
        {
            ipAddress = IPAddress.Parse(ip);//Server의 IP
            Port = port;

            //afterReceive += SMEListener_AfterReceive;

            tcpListener = new TcpListener(ipAddress, Port);//Client와 연결할 TcpListener객체 생성
            tcpListener.Start();

            listenThread = new Thread(new ThreadStart(Accepting));//receiver 쓰레드 생성
            listenThread.Name = "listenThread";
            listenThread.Start();
        }
       
        #endregion

        #region Functions
        //Client를 받는 쓰레드
        public void Accepting()
        {
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                SMEAsyncReceiver receiver = new SMEAsyncReceiver(client);
                //waitList.Enqueue(receiver);
                ThreadPool.QueueUserWorkItem(new WaitCallback(receiver.Receive));

            }
        }

        //private bool IsEmpty()
        //{
        //    lock(thislock)
        //    {
        //        for (int i = 0; i < MaxWorkingNum; i++)
        //        {
        //            if (workingList[i] == null)
        //                return true;
        //        }
        //        return false;
        //    }
        //}

        //public void SMEListener_AfterReceive()
        //{
        //    lock(thislock)
        //    {
                
        //    }
        //}

        #endregion

        #region Dispose Interface Method
        public void Dispose()
        {
            if (tcpListener != null)
                tcpListener.Stop();
            tcpListener = null;

            if (listenThread.IsAlive)
                listenThread.Abort();
            listenThread = null;
        }
        #endregion
        
    }

    public class SMEAsyncReceiver : IDisposable
    {
        #region Members
        private FileStream fileStream;
        private Thread receiver;
        private TcpClient tcpClient = null;
        private int fileLength = 0;
        private string fileName;

        private NetworkStream netStream;
        #endregion

        #region Constructor
        public SMEAsyncReceiver(TcpClient client)
        {
            tcpClient = client;
            netStream = client.GetStream();
            //netStream.ReadTimeout = 1000;
        }
        #endregion

        #region Functions
        // Client로부터 데이터를 받는 함수
        public async void Receive(Object state)
        {
            // 파일 크기
            byte[] buffer = new byte[8];//처음 파일크기를 받아오는데 사용하는 buffer
            netStream.Read(buffer, 0, buffer.Length);//파일크기 수신
            fileLength = BitConverter.ToInt32(buffer, 0);
            
            // 파일 이름 길이 
            buffer = new byte[4];
            netStream.Read(buffer, 0, buffer.Length);
            int fileNameLength = BitConverter.ToInt32(buffer, 0);

            // 파일 이름
            buffer = new byte[fileNameLength];
            netStream.Read(buffer, 0, fileNameLength);
            fileName = System.Text.Encoding.UTF8.GetString(buffer);

            // 파일 내용
            buffer = new byte[1024];
            fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            int totalLength = 0;
            int receiveLength = 0;
            while (totalLength < fileLength)
            {
                receiveLength = netStream.Read(buffer, 0, buffer.Length);
                fileStream.Write(buffer, 0, receiveLength);
                totalLength += receiveLength;
            }
            
            //SMEListener.afterReceive();
            this.Dispose();
        }
        #endregion

        public void Dispose()
        {
            if(tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }
            if(fileStream != null)
            {
                fileStream.Close();
                fileStream = null;
            }
            
        }
    }
}
