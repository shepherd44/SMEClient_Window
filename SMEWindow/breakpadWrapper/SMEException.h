#pragma once

namespace SME
{
	ref class SMEException :System::Exception
	{
	public:
		SMEException();
		SMEException(System::String^ message);
		SMEException(System::String^ message, System::Exception^ innerexception);
	};
}
