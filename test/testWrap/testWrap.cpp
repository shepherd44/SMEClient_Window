// �⺻ DLL �����Դϴ�.

#include "stdafx.h"

#include "testWrap.h"


namespace google_breakpad
{
	Class1::Class1() :obj(new ExceptionHandler(L"C:\\dumps\\", NULL, NULL, NULL, ExceptionHandler::HANDLER_ALL))
	{

	}
	Class1::~Class1()
	{
		delete obj;
	}

}