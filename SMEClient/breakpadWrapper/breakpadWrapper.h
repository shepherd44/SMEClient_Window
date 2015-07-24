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
}
