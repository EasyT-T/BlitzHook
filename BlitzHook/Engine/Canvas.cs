namespace BlitzHook.Engine;

using unsafe LockBufferFunc = delegate* unmanaged[Stdcall]<int, void>;
using unsafe UnlockBufferFunc = delegate* unmanaged[Stdcall]<int, void>;

using unsafe WritePixelFastFunc = delegate* unmanaged[Stdcall]<int, int, uint, int, void>;

public readonly struct Canvas(int handle) : IEngineObject
{
    private static readonly unsafe LockBufferFunc LockBufferFunc;

    private static readonly unsafe UnlockBufferFunc UnlockBufferFunc;

    private static readonly unsafe WritePixelFastFunc WritePixelFastFunc;

    static unsafe Canvas()
    {
        LockBufferFunc = (LockBufferFunc)Linker.Instance.GetSymbol("LockBuffer%buffer=0");
        UnlockBufferFunc = (UnlockBufferFunc)Linker.Instance.GetSymbol("UnlockBuffer%buffer=0");
        WritePixelFastFunc = (WritePixelFastFunc)Linker.Instance.GetSymbol("WritePixelFast%x%y%argb%buffer=0");
    }

    public int Handle { get; } = handle;

    public unsafe void Lock()
    {
        LockBufferFunc(this.Handle);
    }

    public unsafe void Unlock()
    {
        UnlockBufferFunc(this.Handle);
    }

    public unsafe void WritePixelFast(int x, int y, uint rgba)
    {
        WritePixelFastFunc(x, y, rgba, this.Handle);
    }
}