namespace BlitzHook;

using System.Diagnostics;
using System.Runtime.InteropServices;

public unsafe class Runtime(Runtime.Opaque* opaque)
{
    private static Runtime? instance;

    private static nint nativeLibrary;

    [StructLayout(LayoutKind.Sequential)]
    public struct RuntimeVtable
    {
        public delegate* unmanaged[Thiscall]<nint, int> version;

        public delegate* unmanaged[Thiscall]<nint, nint> nextSym;

        public delegate* unmanaged[Thiscall]<nint, byte*, int> symValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Opaque
    {
        public RuntimeVtable* vtable;
    }

    public int GetVersion()
    {
        return opaque->vtable->version((nint)opaque);
    }

    public nint GetNextSym()
    {
        var sym = opaque->vtable->nextSym((nint)opaque);

        return sym;
    }

    public int GetSymValue(nint sym)
    {
        var result = opaque->vtable->symValue((nint)opaque, (byte*)sym);

        return result;
    }

    private static Opaque* RuntimeGetRuntime()
    {
        if (nativeLibrary == 0)
        {
            var name = Path.GetFileName(Environment.ProcessPath) ?? string.Empty;

            nativeLibrary = NativeLibrary.Load(name);

            Debug.Assert(nativeLibrary != 0);
        }

        var runtimeGetRuntime = (delegate* unmanaged[Cdecl]<Opaque*>)NativeLibrary.GetExport(nativeLibrary, "runtimeGetRuntime");

        return runtimeGetRuntime();
    }

    public static Runtime Get()
    {
        return instance ??= new Runtime(RuntimeGetRuntime());
    }
}