#include "stdafx.h"
#include "SMEException.h"

namespace SME
{
	SMEException::SMEException() : Exception()
	{ }
	SMEException::SMEException(System::String^ message) : Exception(message)
	{ }
	SMEException::SMEException(System::String^ message, System::Exception^ innerexception)
		: Exception(message, innerexception)
	{ }
}
