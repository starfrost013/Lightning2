// This is an install helper DLL written solely so I don't have to write Pascal code.
// As we can't guarantee any .NET is installed before installing this, this is written in C.

#include "pch.h"
#ifdef _WINDOWS
#include <shlobj_core.h>

// Function prototypes

_declspec(dllexport) bool IsNet7Installed();

_declspec(dllexport) bool IsNet7Installed()
{
	// windows version
	const WCHAR* dotNetVersion = L"7.0";

	WCHAR pszPath[MAX_PATH];

	memset(pszPath, 0x00, sizeof(WCHAR) * MAX_PATH);

	if (!SHGetSpecialFolderPath(NULL, pszPath, CSIDL_PROGRAM_FILES, FALSE))
	{
		MessageBox(NULL, L"InstallHelper Fatal Error [THIS IS A BUG] - SHGetSpecialFolderPath for Program Files failed. The installation is now going to fail sorry", 
			L"Sorry", MB_ICONWARNING | MB_OK);
		return FALSE;
	}

	// Build a path string.
	wcsncat_s(pszPath, L"\\dotnet\\sdk\\", MAX_PATH);

	WCHAR pszNetSdkVersion[MAX_PATH];
	WIN32_FIND_DATA findFileData;
	HANDLE fileHandle;

	memset(&pszNetSdkVersion, 0x00, sizeof(WCHAR) * MAX_PATH);
	memset(&findFileData, 0x00, sizeof(WIN32_FIND_DATA));

	fileHandle = FindFirstFileW(pszNetSdkVersion, &findFileData);

	if (fileHandle == INVALID_HANDLE_VALUE)
	{
		// there are no sdks installed
		return FALSE; 
	}
	else
	{
		while (FindNextFileW(fileHandle, &findFileData) != NULL)
		{
			if (wcsstr(pszPath, dotNetVersion))
			{
				// .NET 7.x installation found
				return TRUE;
			}
		}

		// out of files, nothing more to return
		return FALSE;
	}
}
#endif