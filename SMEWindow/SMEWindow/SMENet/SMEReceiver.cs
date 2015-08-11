using System;
//Tcp를 이용해 파일을 보낼 때 필요한 namespaces
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace SME.SMESend
{
    public class SMEReceiver
    {
        #region Members
        private TcpListener listener;
        private NetworkStream netStream;
        private FileStream file;
        private Thread receiver;
        private TcpClient client = null;
        private int fileLength = 0;

        private IPAddress ipAddress; //Server의 IP
        #endregion

        #region Constructors
        public SMEReceiver()
        {
            file = new FileStream("get.png", FileMode.Create, FileAccess.Write);//Server에 저장할 새로운 파일 생성
            ipAddress = IPAddress.Parse("127.0.0.1");//Server의 IP

            listener = new TcpListener(ipAddress, 3000);//Client와 연결할 TcpListener객체 생성
            listener.Start();

            receiver = new Thread(new ThreadStart(Accepting));//receiver 쓰레드 생성
            receiver.Start();
        }
        #endregion

        #region Functions
        //Client를 받는 쓰레드
        public void Accepting()
        {
            while (true)
            {
                client = listener.AcceptTcpClient();//Client를 Accept한다.
                netStream = client.GetStream();//Client로부터 NetworkStream을 받는다.
                Receive();
                netStream.Close();
                client.Close();
            }
        }

        //Client로부터 데이터를 받는 함수
        public void Receive()
        {
            byte[] buffer = new byte[4];//처음 파일크기를 받아오는데 사용하는 buffer
            netStream.Read(buffer, 0, buffer.Length);//파일크기 수신

            fileLength = BitConverter.ToInt32(buffer, 0);//받아온 파일크기를 Int형으로 변환
            int totalLength = 0;//수신할 총 파일의 크기

            buffer = new byte[1024];

            while( totalLength < fileLength )
            {
                //NetworkStream에서 데이터를 받고 받아온 데이터의 크기를 반환한다.
                int receiveLength = netStream.Read(buffer, 0, buffer.Length);
                file.Write(buffer, 0, buffer.Length);//FileStream에 받아온 데이터를 쓴다.
                totalLength += receiveLength;
            }
        }
        #endregion
    }
}
