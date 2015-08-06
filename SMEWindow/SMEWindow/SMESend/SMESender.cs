using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Tcp를 이용해 파일을 보낼 때 필요한 namespaces
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace SME.SMESend
{
    class SMESender
    {
        #region members
        private TcpClient client;
        private NetworkStream netStream;
        private FileStream file;
        private Thread send;
        private long fileLength = 0; //보내는 파일의 크기
        #endregion

        #region Creator
        //생성자 인자로 ServerIP와 보낼 파일의 FilePath를 받는다.
        public SMESender(string ipAddress, string FilePath)
        {
            client = new TcpClient(ipAddress, 3000); //TcpClient 객체 생성 및 ip와 연결
            netStream = client.GetStream(); //데이터를 보내고 받는 NetworkStream 반환
            file = File.OpenRead(FilePath); //서버로 보낼 파일을 FileStream으로 부른다.
            fileLength = file.Length; //파일의 크기를 저장한다.
            byte[] buffer = BitConverter.GetBytes(fileLength);//long을 byte로 변환한다.

            netStream.Write(buffer, 0, buffer.Length); //파일크기를 서버에 보낸다.
            send = new Thread(new ThreadStart(Send)); //send 쓰레드 생성
            send.Start(); //쓰레드 시작으로 상태 전환
        }
        //소멸자
        ~SMESender()
        {
            file.Close();
            client.Close();
        }
        #endregion

        #region Functions
        //Send 쓰레드
        public void Send()
        {
            long count = fileLength / 1024 + 1; //1024바이트씩 보낼 횟수 계산
            byte[] buffer = new byte[1024]; //데이터를 읽어서 보낼 byte변수

            for (int i = 0; i < count; i++)
            {
                file.Read(buffer, 0, buffer.Length); //FileStream에서 읽어서
                netStream.Write(buffer, 0, buffer.Length); //NetworkStream에 쓴다.
            }
        }
        #endregion
    }
}
