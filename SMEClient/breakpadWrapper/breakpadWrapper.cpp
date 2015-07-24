// 기본 DLL 파일입니다.

#include "stdafx.h"
#include "breakpadWrapper.h"

namespace google_breakpad
{
	breakpadWrapper::breakpadWrapper()
	{
		handler = new ExceptionHandler(L"C:\\dumps\\", NULL, NULL, NULL, ExceptionHandler::HANDLER_ALL);
	}
	breakpadWrapper::~breakpadWrapper()
	{
		delete handler;
	}
	bool breakpadWrapper::WriteMinidump()
	{
		return ExceptionHandler::WriteMinidump(L"C:\\dumps\\", NULL, NULL);
	}
}