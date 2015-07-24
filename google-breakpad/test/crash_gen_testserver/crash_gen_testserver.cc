#pragma once

#include "client\windows\crash_generation\crash_generation_server.h"
#include "client\windows\crash_generation\crash_generation_client.h"
#include "client\windows\handler\exception_handler.h"
#include "client\windows\crash_generation\client_info.h"
#include "client\windows\sender\crash_report_sender.h"

#include <iostream>
#include <stdio.h>
#include <tchar.h>

namespace google_breakpad
{
	// http sender
	// 생성자에 파일이름 넘겨주면 파일 읽어들임.
	// SendCrashReport(wstring url, map<wstring, wstring> parameters,
	//					wstring dump_file_name, wstring* report_code)
	CrashReportSender* g_crashsender = NULL;
	
	void InitCrashReportSender(std::wstring& filename)
	{
		g_crashsender = new CrashReportSender(filename);
	}
	void ClearCrashReportSender()
	{
		if (g_crashsender == NULL)
			return;
		else
		{
			delete g_crashsender;
		}
	}
	// Named pipe
	wchar_t g_PipeName[] = L"\\\\.\\pipe\\BreakpadCrashServices\\TestServer";
	
	//static ExceptionHandler* handler = NULL;
	//crashserver	
	static CrashGenerationServer* crash_server = NULL;

	const size_t g_MaximumLineLength = 256;
	
	/*
	static DWORD WINAPI AppendTextWorker(void* context) {
		TCHAR* text = reinterpret_cast<TCHAR*>(context);
		std::cout << text << std::endl;
		delete[] text;
		return 0;
	}*/

	static void ShowClientConnected(void* context,
									const ClientInfo* clientinfo)
	{
		TCHAR* text = new TCHAR[g_MaximumLineLength];
		text[0] = _T('\0');
		int ret = swprintf(text, g_MaximumLineLength, TEXT("Client connected:\t\t%d\r\n"),
							clientinfo->pid());
		if (ret == -1)
		{
			delete[] text;
			return;
		}
		std::cout << text << std::endl;
		delete[] text;
	}
	static void ShowClientCrashed(void* context,
								const ClientInfo* clientinfo,
								const wstring* dump_path)
	{
		TCHAR* text = new TCHAR[g_MaximumLineLength];
		text[0] = _T('\0');
		int ret = swprintf(text, g_MaximumLineLength, TEXT("Client requested dump:\t%d\r\n"),
							clientinfo->pid());
		if (ret == -1)
		{
			delete[] text;
			return;
		}
		std::cout << text << std::endl;
		delete[] text;

		CustomClientInfo custominfo = clientinfo->GetCustomInfo();
		std::wstring str_line;
		for (size_t i = 0; i < custominfo.count; ++i)
		{
			if (i > 0)
				str_line += L", ";
			str_line += custominfo.entries[i].name;
			str_line += L": ";
			str_line += custominfo.entries[i].value;
		}
		text = new TCHAR[g_MaximumLineLength];
		text[0] = _T('\0');
		ret = swprintf(text, g_MaximumLineLength, L"%s\n", str_line.c_str());
		if (ret == -1)
		{
			delete[] text;
			return;
		}
		std::cout << text << std::endl;
		delete[] text;
	}
	static void ShowClientExited(void* context,
		const ClientInfo* clientinfo)
	{
		TCHAR* text = new TCHAR[g_MaximumLineLength];
		text[0] = _T('\0');
		int ret = swprintf_s(text, g_MaximumLineLength, TEXT("Client exited:\t\t%d\r\n"),
			clientinfo->pid());

		if (ret == -1) {
			delete[] text;
			return;
		}
		std::cout << text << std::endl;
		delete[] text;
	}

	//crash server 생성
	void CrashServerStart()
	{
		if (crash_server)
			return;
		std::wstring dumpPath = L"C:\\Dumps\\Server\\";
		if (_wmkdir(dumpPath.c_str()) && (errno != EEXIST)) {
			std::cout << "Unable to create dump directory" << std::endl;
			return;
		}
		crash_server = new CrashGenerationServer(g_PipeName,
												NULL, ShowClientConnected,
												NULL, ShowClientCrashed,
												NULL, ShowClientExited,
												NULL, NULL, NULL, true,
												&dumpPath);
		if (!crash_server->Start())
		{
			std::cout << "Unable to start crash server" << std::endl;
			delete crash_server;
			crash_server = NULL;
		}
		std::cout << "Server start!!" << std::endl;
	}
	
	void CrashServerStop()
	{
		delete crash_server;
		crash_server = NULL;
	}

}//google-breakpad

int main(int argc, char** argv)
{
	std::wstring chekpointfilename;
	chekpointfilename = L"C:\\Dump\\checkpoint";
	google_breakpad::InitCrashReportSender(chekpointfilename);
	std::map<std::wstring, std::wstring> umap;
	// 전송객체 초기화
	google_breakpad::g_crashsender->SendCrashReport(L"http://192.168.23.152",
		umap,
		L"C:\\Dumps\\Server\\45055080-e02c-496f-bdc6-c5b0b404953b.dmp",
		NULL
		);

	while (1)
	{
		char ch;
		std::cout << "-----server menu-----" << std::endl;
		std::cout << "1. Start Server\n2. Stop Server\n";
		std::cout << "q. quit" << std::endl << "input menu num: ";
		std::cin >> ch;
		if (ch == '1')
			google_breakpad::CrashServerStart();
		else if (ch == '2')
			google_breakpad::CrashServerStop();
		else if (ch == 'q')
			break;
		else
			std::cout << "input again\n";
	}

	return 0;
}