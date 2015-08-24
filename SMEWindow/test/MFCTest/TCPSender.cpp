#include "stdafx.h"
#include "TCPSender.h"
#include "MFCTest.h"

#define _CRT_SECURE_NO_WARNINGS
#define _WINSOCK_DEPRECATED_NO_WARNINGS

using namespace Sender;
TCPSender::TCPSender()
{
	m_ServerSocket = INVALID_SOCKET;
}

TCPSender::TCPSender(const char* ip, int port)
{
	m_ServerSocket = INVALID_SOCKET;
	Initialize(ip, port);
}

TCPSender::~TCPSender()
{
	closesocket(m_ServerSocket);
	WSACleanup();
}

void TCPSender::ErrorMsg(char* pch, char* msg, int ret)
{
	memset(pch, NULL, MSG_LENGTH);
	strcpy_s(pch, MSG_LENGTH, msg);
	char cret[32];
	_itoa(ret, cret, 10);
	strcat_s(pch, MSG_LENGTH, cret);
	strcat_s(pch, MSG_LENGTH, "\n");
}

void TCPSender::Initialize(const char* ip, int port)
{
	int ret;
	// Initialize Winsock
	ret = WSAStartup(MAKEWORD(2, 2), &m_wsaData);
	if (ret != NO_ERROR)
	{
		char msg[MSG_LENGTH];
		ErrorMsg(msg, "WSAStartup failed: ", ret);
		throw std::exception(msg);
		return;
	}

	m_ServerSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (m_ServerSocket == INVALID_SOCKET)
	{
		char msg[MSG_LENGTH];
		ErrorMsg(msg, "socket failed: ", WSAGetLastError());
		throw std::exception(msg);
		return;
	}

	// The sockaddr_in structure specifies the address family,
	// IP address, and port of the server to be connected to.
	m_ServerSockaddr.sin_family = AF_INET;
	m_ServerSockaddr.sin_addr.s_addr = inet_addr(ip);
	m_ServerSockaddr.sin_port = htons(port);

	// Connect to server.
	ret = connect(m_ServerSocket, (SOCKADDR*)&m_ServerSockaddr, sizeof(m_ServerSockaddr));
	if (ret == SOCKET_ERROR) {
		closesocket(m_ServerSocket);
		char msg[MSG_LENGTH];
		ErrorMsg(msg, "Unable to connect to server: ", WSAGetLastError());
		throw std::exception(msg);
		return;
	}
}



int TCPSender::Send(char* pmessage, int length)
{
	/*char filesize[8];
	char filenamelength[4];
	char *filename;
	char buffer[BUF_LENGTH];*/

	return 0;
}

int TCPSender::Send(std::ifstream* inFile, char* pfilename, int apikey)
{
	// file open
	std::ifstream::pos_type size;

	char* oData;

	size = inFile->tellg();
	inFile->seekg(0, std::ios::beg);

	oData = new char[1024];
	inFile->read(oData, 1024);

	theApp.DoMessageBox(reinterpret_cast<wchar_t*> (oData), NULL, MB_OK);



	// get file size
	unsigned long fsize = 0;
	byte filesize[8];
	IntToByte(size, filesize);
	// send file size 
	int ret = send(m_ServerSocket, reinterpret_cast<char*>(filesize), 8, 0);

	// send file name
	int fnlength = strlen(pfilename);
	byte filenamelength[4];
	memset(filenamelength, 0, 4);
	IntToByte(fnlength, filenamelength);
	ret = send(m_ServerSocket, reinterpret_cast<char*>(filenamelength), 4, 0);
	ret = send(m_ServerSocket, pfilename, fnlength, 0);

	//apikey
	memset(filenamelength, 0, 4);
	IntToByte(apikey, filenamelength);
	ret = send(m_ServerSocket, reinterpret_cast<char*>(filenamelength), 4, 0);

	char buffer[BUF_LENGTH];
	memset(buffer, 0, BUF_LENGTH);

	int fblocksize = 0;
	//std::ifstream ifile(pfilepath);
	int i = 0;
	while (1)
	{
		/* fblocksize = fread(buffer, sizeof(char), BUF_LENGTH, fp); */

		ret = send(m_ServerSocket, reinterpret_cast<char*>(oData + i), 1024, 0);

		i += 1024;
	}
	return 0;
}
CMFCTestApp theApp;

int TCPSender::Send(char* pfilepath, char* pfilename, int apikey)
{
	// file open
	std::ifstream inFile;
	std::ifstream::pos_type size;

	inFile.open(pfilepath, std::ios::in | std::ios::binary | std::ios::ate);
	char* oData;

	size = inFile.tellg();
	inFile.seekg(0, std::ios::beg);
	oData = new char[1024];
	inFile.read(oData, 1024);

	// get file size
	unsigned long fsize = 0;
	byte filesize[8];
	IntToByte(size, filesize);
	// send file size 
	int ret = send(m_ServerSocket, reinterpret_cast<char*>(filesize), 8, 0);
	
	// send file name
	int fnlength = strlen(pfilename);
	byte filenamelength[4];
	memset(filenamelength, 0, 4);
	IntToByte(fnlength, filenamelength);
	ret = send(m_ServerSocket, reinterpret_cast<char*>(filenamelength), 4, 0);
	ret = send(m_ServerSocket, pfilename, fnlength, 0);

	//apikey
	memset(filenamelength, 0, 4);
	IntToByte(apikey, filenamelength);
	ret = send(m_ServerSocket, reinterpret_cast<char*>(filenamelength), 4, 0);

	char buffer[BUF_LENGTH];
	memset(buffer, 0, BUF_LENGTH);

	int fblocksize = 0;
	//std::ifstream ifile(pfilepath);
	int i = 0;
	while (1)
	{
		/* fblocksize = fread(buffer, sizeof(char), BUF_LENGTH, fp); */

		ret = send(m_ServerSocket, reinterpret_cast<char*>(oData + i), 1024, 0);
		i += 1024;
	}
	inFile.close();

	return 0;
}

void TCPSender::Close()
{
	int ret = shutdown(m_ServerSocket, SD_SEND);
	if (ret == SOCKET_ERROR)
	{
		char msg[MSG_LENGTH];
		ErrorMsg(msg, "socket failed: ", WSAGetLastError());
		throw std::exception(msg);
	}
	closesocket(m_ServerSocket);
	WSACleanup();
}

void Sender::IntToByte(int value, byte* dest)
{
	dest[3] = ((value >> 24) & 0xFF);
	dest[2] = ((value >> 16) & 0xFF);
	dest[1] = ((value >> 8) & 0xFF);
	dest[0] = ((value >> 0) & 0xFF);
}

void Sender::LongToChar(unsigned long value, char* dest)
{
	dest[7] |= (unsigned char)((value >> 56) & 0xFF);
	dest[6] |= (unsigned char)((value >> 48) & 0xFF);
	dest[5] |= (unsigned char)((value >> 40) & 0xFF);
	dest[4] |= (unsigned char)((value >> 32) & 0xFF);
	dest[3] |= (unsigned char)((value >> 24) & 0xFF);
	dest[2] |= (unsigned char)((value >> 16) & 0xFF);
	dest[1] |= (unsigned char)((value >> 8) & 0xFF);
	dest[0] |= (unsigned char)((value >> 0) & 0xFF);
}
void Sender::LongToByte(unsigned long value, byte* dest)
{
	dest[7] |= (byte)((value >> 56) & 0xFF);
	dest[6] |= (byte)((value >> 48) & 0xFF);
	dest[5] |= (byte)((value >> 40) & 0xFF);
	dest[4] |= (byte)((value >> 32) & 0xFF);
	dest[3] |= (byte)((value >> 24) & 0xFF);
	dest[2] |= (byte)((value >> 16) & 0xFF);
	dest[1] |= (byte)((value >> 8) & 0xFF);
	dest[0] |= (byte)((value >> 0) & 0xFF);
}

void Sender::ByteReverse(char* buf, int size)
{
	char* temp = new char[size];
	memset(temp, 0, size);
	for (int i = 0; i < size; i++)
	{
		temp[i] = buf[i];
	}
	for (int i = 0; i < size; i++)
	{
		buf[size - 1 - i] = temp[i];
	}
	delete temp;
}