using System;
//Tcp를 이용해 파일을 보낼 때 필요한 namespaces
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace NetLib
{
    public class TCPSender : IDisposable
    {
        #region members
        private TcpClient tcpClient;
        private NetworkStream netStream;
        private FileStream fileStream;
        private Thread send;
        private long fileLength = 0; //보내는 파일의 크기
        #endregion

        #region Creator
        //생성자 인자로 ServerIP와 보낼 파일의 FilePath를 받는다.
        public TCPSender(string ipAddress, string FilePath, string FileName)
        {
            tcpClient = new TcpClient(ipAddress, 3000); //TcpClient 객체 생성 및 ip와 연결
            netStream = tcpClient.GetStream(); //데이터를 보내고 받는 NetworkStream 반환
            fileStream = File.OpenRead(FilePath); //서버로 보낼 파일을 FileStream으로 부른다.
            
            // 파일 크기 전송
            fileLength = fileStream.Length; //파일의 크기를 저장한다.
            byte[] buffer = BitConverter.GetBytes(fileLength);
            netStream.Write(buffer, 0, buffer.Length);

            // 파일 이름, 이름 크기 전송
            byte[] filenamebuffer = System.Text.Encoding.UTF8.GetBytes(FileName);
            buffer = BitConverter.GetBytes(filenamebuffer.Length);
            netStream.Write(buffer, 0, buffer.Length);
            netStream.Write(filenamebuffer, 0, filenamebuffer.Length);

            send = new Thread(new ThreadStart(FileSend)); //send 쓰레드 생성
            send.Start(); //쓰레드 시작으로 상태 전환
        }
        //소멸자
        ~TCPSender()
        {
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream = null;
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }
        }
        #endregion

        #region Functions
        //Send 쓰레드
        public async void FileSend()
        {
            long count = fileLength / 1024 + 1; //1024바이트씩 보낼 횟수 계산
            byte[] buffer = new byte[1024]; //데이터를 읽어서 보낼 byte변수

            int readLength = 0;
            for (int i = 0; i < count; i++)
            {
                readLength = fileStream.Read(buffer, 0, buffer.Length);
                netStream.Write(buffer, 0, readLength);
            }

            tcpClient.Close();
        }

        //public void StreamSned(Stream stream)
        //{
        //    long count = fileLength / 1024 + 1;
        //    byte[] buffer = new byte[1024];

        //    for (int i = 0; i < count ; i++)
        //    {
        //        stream.Read(buffer, 0, buffer.Length);
        //        netStream.Write(buffer, 0, buffer.Length);
        //    }
        //}
        #endregion

        public void Dispose()
        {
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream = null;
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }
            if (send.IsAlive)
                send.Abort();
            send = null;
        }
    }
}
