// testWrap.h

#pragma once


#include "..\..\handler\exception_handler.h"

namespace google_breakpad {
	
	public ref class Class1
	{
	protected:
		ExceptionHandler* obj;
	public:
		Class1();
		~Class1();
	};
}
