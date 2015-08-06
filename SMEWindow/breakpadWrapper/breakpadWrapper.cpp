// 기본 DLL 파일입니다.

#include "stdafx.h"
#include "breakpadWrapper.h"
#include "SMEException.h"

namespace google_breakpad
{
	bool ResultCallBack(const wchar_t* dump_path,
		const wchar_t* minidump_id,
		void* context,
		EXCEPTION_POINTERS* exinfo,
		MDRawAssertionInfo* assertion,
		bool succeeded)
	{
		//throw gcnew System::ArgumentNullException();
		//throw gcnew SME::SMEException("test SMEException");
		return true;
	}

	breakpadWrapper::breakpadWrapper()
	{
		handler = new ExceptionHandler(L"C:\\dumps\\", NULL, ResultCallBack, NULL, ExceptionHandler::HANDLER_ALL);
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
