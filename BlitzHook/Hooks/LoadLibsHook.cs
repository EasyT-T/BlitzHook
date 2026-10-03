namespace BlitzHook.Hooks;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BlitzHook.Render;
using EasyHook;
using unsafe LoadLibsFunc = delegate* unmanaged[Stdcall]<void*, void>;

public class LoadLibsHook : IHook
{
    private static unsafe LoadLibsFunc func;

    private LocalHook? _hook;

    public unsafe void Setup()
    {
        var loadLibs = Linker.Instance.GetSymbol("_bbLoadLibs");
        this._hook = LocalHook.CreateUnmanaged(loadLibs, (nint)(LoadLibsFunc)(&LoadLibs), 0);
        this._hook.ThreadACL.SetInclusiveACL([0]);

        func = (LoadLibsFunc)this._hook.HookBypassAddress;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static unsafe void LoadLibs(void* p)
    {
        func(p);
    }
}