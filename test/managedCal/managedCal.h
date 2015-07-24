// managedCal.h

#pragma once

#include "..\UnmanagedCal\AddCal.h"
using namespace System;

namespace managedCal
{
    public ref class AddCalWrap
	{
    protected:
        AddCal *m_pAddCal;

    public:
        AddCalWrap();
        virtual ~AddCalWrap();

        int Add(int _num1, int _num2);
	};
}