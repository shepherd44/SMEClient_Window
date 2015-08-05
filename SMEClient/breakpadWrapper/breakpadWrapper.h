// breakpadWrapper.h

#pragma once
#include "client\windows\handler\exception_handler.h"

namespace google_breakpad {

	public ref class breakpadWrapper
	{
		ExceptionHandler* handler;
	public:	
		breakpadWrapper();
		~breakpadWrapper();
		static bool WriteMinidump();
	};

	// breakpad dump callback function
	bool ResultCallBack(const wchar_t* dump_path,
		const wchar_t* minidump_id,
		void* context,
		EXCEPTION_POINTERS* exinfo,
		MDRawAssertionInfo* assertion,
		bool succeeded);
}
