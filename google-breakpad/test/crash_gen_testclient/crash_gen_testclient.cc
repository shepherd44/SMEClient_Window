#include "client/windows/handler/exception_handler.h"
#include "client/windows/crash_generation/client_info.h"

#include <iostream>
#include <stdio.h>
#include <tchar.h>

namespace google_breakpad
{
	// Named pipe - out of process를 위한 파이프 이름(내부적으로 Named Pipe IPC 이용
	wchar_t g_PipeName[] = L"\\\\.\\pipe\\BreakpadCrashServices\\TestServer";
	void ChangePipeName(wchar_t* pipename)
	{
		int i = 0;
		while (pipename[i] != '\0')
			i++;
		std::cout << i<<std::endl;
	}
	// handler
	ExceptionHandler* g_handler;
	static int kCustomInfoCount = 2;

	// client 정보를 가지는 변수
	static CustomInfoEntry kCustomInfoEntries[] = {
		CustomInfoEntry(L"prod", L"CrashTestClient"),
		CustomInfoEntry(L"ver", L"1.0"),
	};
	const size_t g_MaximumLineLength = 256;

	// 덤프 콜백 함수
	bool ShowDumpResults(const wchar_t* dump_path,
		const wchar_t* minidump_id,
		void* context,
		EXCEPTION_POINTERS* exinfo,
		MDRawAssertionInfo* assertion,
		bool succeeded)
	{
		TCHAR* text = new TCHAR[g_MaximumLineLength];
		text[0] = _T('\0');
		int ret = swprintf(text, g_MaximumLineLength, TEXT("Dump generation request %s\r\n"),
			succeeded ? TEXT("succeeded") : TEXT("failed"));
		if (ret == -1)
		{
			delete[] text;
		}
		//QueueUserWorkItem(AppendTextWorker, text, WT_EXECUTEDEFAULT);
		std::cout << text << std::endl;
		delete[] text;
		return succeeded;
	}

	void InitExceptionHandler_Out()
	{
		CustomClientInfo custom_info = { kCustomInfoEntries, kCustomInfoCount };

		g_handler = new ExceptionHandler(L"C:\\Dumps\\Client\\",
										NULL,
										ShowDumpResults,
										NULL,
										ExceptionHandler::HANDLER_ALL,
										MiniDumpNormal,
										g_PipeName,
										&custom_info);

		std::cout << "-------------------------" << std::endl;
		std::cout << "Work Out-Of-Process Type." << std::endl;
		std::cout << "If Crash Server don't work now," << std::endl;
		std::cout << "This Work In-Process Type" << std::endl;
		std::cout << "-------------------------" << std::endl;
	}

	void InitExceptionHandler_In()
	{
		CustomClientInfo custom_info = { kCustomInfoEntries, kCustomInfoCount };

		g_handler = new ExceptionHandler(L"C:\\Dumps\\Client\\",
			NULL,
			ShowDumpResults,
			NULL,
			ExceptionHandler::HANDLER_ALL);
		std::cout << "-------------------------" << std::endl;
		std::cout << "Work In-Process Type" << std::endl;
		std::cout << "-------------------------" << std::endl;
	}
	void Crash_derefZero()
	{
		int* i = 0;
		*i = 1;
	}
} // google_breakpad namespace

int main(int arcv, char** argc)
{
	google_breakpad::ExceptionHandler::WriteMinidump(L"C:\\Dumps\\Client\\",NULL,NULL);

	char ch = '1';
	std::cout << "-------------------------------" << std::endl;
	std::cout << "1. Out-Of-Process 동작\n2. In-Process 동작\n3. ChangePipeName \n";
	std::cout << "-------------------------------" << std::endl;
	std::cout<< "input menu: ";
	std::cin >> ch;
	if (ch == '1')
		google_breakpad::InitExceptionHandler_Out();
	else if (ch == '2')
		google_breakpad::InitExceptionHandler_In();
	else if (ch == '3')
	{
		google_breakpad::ChangePipeName(L"\\\\.\\pipe\\BreakpadCrashServices\\TestServer");
		google_breakpad::InitExceptionHandler_Out();
	}
	else
		google_breakpad::InitExceptionHandler_Out();

	while (1)
	{
		std::cout << "-----client menu-----" << std::endl;
		std::cout << "1. crashdrefzero\n";
		std::cout << "q. quit" << std::endl << "input menu num: ";
		std::cin >> ch;
		if (ch == '1')
			google_breakpad::Crash_derefZero();
		else if (ch == 'q')
			break;
		else
			std::cout << "input again\n";
	}
	return 0;
}