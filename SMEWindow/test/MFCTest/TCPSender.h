#pragma once
#define _CRT_SECURE_NO_WARNINGS
#define _WINSOCK_DEPRECATED_NO_WARNINGS

#include <stdio.h>
#include <stdlib.h>
#include <iostream>

#include <WinSock2.h>
#include <WS2tcpip.h>

#include <stdexcept>
#include <fstream>

#pragma comment(lib, "Ws2_32.lib")

namespace Sender
{


#define BUF_LENGTH		1024
#define MSG_LENGTH		124

	class TCPSender
	{
	public:
		TCPSender();
		TCPSender(const char* ip, int port);
		~TCPSender();

	private:
		void Initialize(const char* ip, int port);
		void ErrorMsg(char* pch, char* msg, int ret);
	public:
		int Send(char* pmessage, int length);
		int Send(std::ifstream* inFile, char* pfilename, int apikey);
		int Send(char* pfilepath, char* pfilename, int apikey);
		void SetSocket(SOCKET socket);
		void SetSocket(char* ip, int length);
		void Close();

	private:
		SOCKET m_ServerSocket;
		struct sockaddr_in m_ServerSockaddr;
		WSADATA m_wsaData;
	};

	class SMETCPSender : TCPSender
	{
	public:
		SMETCPSender();
		SMETCPSender(const char* ip, int port);
		~SMETCPSender();
		
	private:
		int SendToSMEServer(FILE* file);
	};

	void IntToByte(int value, byte* dest);
	void LongToChar(unsigned long value, char* dest);
	void LongToByte(unsigned long value, byte* dest);
	void ByteReverse(char* buf, int size);
}
