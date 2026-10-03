#include "include/easyhook.h"
#include <windows.h>

extern "C" __declspec(dllimport) void OnDllLoaded();

namespace
{
    int (__stdcall *bbWinMainFunc)();

    int __stdcall bbWinMain()
    {
        OnDllLoaded();

        return bbWinMainFunc();
    }
}

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved )
{
    if (fdwReason != DLL_PROCESS_ATTACH)
    {
        return TRUE;
    }

    HMODULE processHandle = GetModuleHandle(nullptr);

    bbWinMainFunc = GetProcAddress(processHandle, "_bbWinMain@0");

    HOOK_TRACE_INFO hHook = {nullptr};

    NTSTATUS result = LhInstallHook(reinterpret_cast<void*>(bbWinMainFunc), reinterpret_cast<void*>(&bbWinMain), nullptr, &hHook);

    if (FAILED(result))
    {
        return TRUE;
    }

    ULONG entries[1] = { 0 };

    result = LhSetInclusiveACL(entries, 1, &hHook);

    if (FAILED(result))
    {
        return TRUE;
    }

    return TRUE;
}