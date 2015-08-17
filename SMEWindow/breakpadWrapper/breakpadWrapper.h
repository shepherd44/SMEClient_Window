// breakpadWrapper.h

#pragma once
#include "client\windows\handler\exception_handler.h"

namespace google_breakpad {

	wchar_t* g_ServerIP = NULL;
	int g_ServerPort = NULL;

	public ref class breakpadWrapper
	{
		ExceptionHandler* handler;
	public:	
		breakpadWrapper();
		breakpadWrapper(wchar_t* ip, int port);
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

	void MakeDirectory(wchar_t *full_path)
	{
		wchar_t temp[256], *sp;
		wcscpy_s(temp, full_path);
		sp = temp;
		while ((sp = wcschr(sp, '\\')))
		{
			if (sp > temp && *(sp - 1) != ':')
			{
				*sp = '\0';
				CreateDirectory(temp, NULL);
				*sp = '\\';
			}
			sp++;
		}
	}
}
