#include <stdio.h>
#include <Windows.h>
#include "Test01.h"

extern "C"
{
	DLLAPI int Plus(const int a, const int b)
	{
		return a + b;
	}

	DLLAPI int Multiply(const int a, const int b)
	{
		return a * b;
	}

	DLLAPI BOOL LoadDll()
	{
		BOOL bTemp = FALSE;
		HMODULE hDll = LoadLibrary(L"someDllTest.dll");
		if (NULL == hDll)
		{
			bTemp = FALSE;
		}
		else
		{
			FreeLibrary(hDll);
			bTemp = TRUE;
		}
		return bTemp;
	}

	DLLAPI LPCTSTR StringEX(LPCTSTR str)
	{
		return str;
	}

}