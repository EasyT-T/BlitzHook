#pragma comment(lib, "EasyHook32.lib")

#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wcast-function-type-strict"

#include <iosfwd>
#include <stdint.h>

#include "include/easyhook.h"
#include <windows.h>

extern "C" void __stdcall OnDllLoaded();

namespace
{
    using runtimeGetRuntimeType = int32_t (__stdcall *)();
}

namespace
{
    bool initialized = false;

    runtimeGetRuntimeType runtimeGetRuntimeFunc = nullptr;

    int32_t __stdcall runtimeGetRuntime()
    {
        if (!initialized)
        {
            OnDllLoaded();
            initialized = true;
        }

        return runtimeGetRuntimeFunc();
    }
}

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved )
{
    if (fdwReason != DLL_PROCESS_ATTACH)
    {
        return TRUE;
    }

    HMODULE processHandle = GetModuleHandle(nullptr);

    runtimeGetRuntimeFunc = GetProcAddress(processHandle, "runtimeGetRuntime");

    HOOK_TRACE_INFO hHook = {nullptr};

    NTSTATUS result = LhInstallHook(reinterpret_cast<void*>(runtimeGetRuntimeFunc), reinterpret_cast<void*>(&runtimeGetRuntime), nullptr, &hHook);

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