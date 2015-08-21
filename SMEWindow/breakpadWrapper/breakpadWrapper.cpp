// 기본 DLL 파일입니다.

#include "stdafx.h"
#include "breakpadWrapper.h"
#include "SMEException.h"

namespace google_breakpad
{
	using namespace NetLib;

	bool ResultCallBack(const wchar_t* dump_path,
		const wchar_t* minidump_id,
		void* context,
		EXCEPTION_POINTERS* exinfo,
		MDRawAssertionInfo* assertion,
		bool succeeded)
	{
		// 서버 전송
		System::String^ filepath = gcnew System::String(dump_path);
		System::String^ filename = gcnew System::String(minidump_id);
		filename += ".dmp";
		filepath += filename;
		TCPSender^ sender = gcnew TCPSender(gcnew System::String("127.0.0.1"), 3000, filepath, filename, g_APIKey);
		//sender->FileSend();
		System::Windows::Forms::MessageBox::Show(L"Native Error occur");
		return true;
	}

	breakpadWrapper::breakpadWrapper(int apikey)
	{
		wchar_t* dumpfolder_path = L"C:\\Dumps\\";
		int ret = GetFileAttributes(dumpfolder_path);
		if (ret == -1)
			CreateDirectory(dumpfolder_path, NULL);

		g_APIKey = apikey;

		handler = new ExceptionHandler(dumpfolder_path, NULL, ResultCallBack, NULL, ExceptionHandler::HANDLER_ALL);
	}
	breakpadWrapper::breakpadWrapper(wchar_t* ip, int port, int apikey)
	{
		g_ServerIP = new wchar_t[wcslen(ip)];
		wcscpy_s(g_ServerIP, wcslen(ip), ip);
		g_ServerPort = port;
		g_APIKey = apikey;

		wchar_t* dumpfolder_path = L"C:\\Dumps\\";
		int ret = GetFileAttributes(dumpfolder_path);
		if (ret == -1)
			CreateDirectory(dumpfolder_path, NULL);
		handler = new ExceptionHandler(dumpfolder_path, NULL, ResultCallBack, NULL, ExceptionHandler::HANDLER_ALL);
	}
	
	breakpadWrapper::~breakpadWrapper()
	{
		delete handler;
		if (g_ServerIP != NULL)
		{
			delete g_ServerIP;
		}
	}
	
	bool breakpadWrapper::WriteMinidump()
	{
		return ExceptionHandler::WriteMinidump(L"C:\\dumps\\", NULL, NULL);
	}
}
