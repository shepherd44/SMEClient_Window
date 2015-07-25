// 기본 DLL 파일입니다.

#include "stdafx.h"
#include "managedCal.h"

namespace managedCal
{
	AddCalWrap::AddCalWrap()
		: m_pAddCal(new AddCal)
	{

	}

	AddCalWrap::~AddCalWrap()
	{
		if (m_pAddCal)
		{
			delete m_pAddCal;
			m_pAddCal = 0;
		}
	}

	int AddCalWrap::Add(int _num1, int _num2)
	{
		return (m_pAddCal->Add(_num1, _num2));
	}
}