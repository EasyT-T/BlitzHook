namespace BlitzHook.Engine;

using unsafe CreateBrushFunc = delegate* unmanaged[Stdcall]<float, float, float, int>;
using unsafe FreeBrushFunc = delegate* unmanaged[Stdcall]<int, void>;
using unsafe BrushTextureFunc = delegate* unmanaged[Stdcall]<int, int, int, int, void>;
using unsafe BrushColorFunc = delegate* unmanaged[Stdcall]<int, float, float, float, void>;

public readonly struct Brush(int handle) : IEngineObject
{
    private static readonly unsafe CreateBrushFunc CreateBrushFunc;
    private static readonly unsafe FreeBrushFunc FreeBrushFunc;
    private static readonly unsafe BrushTextureFunc BrushTextureFunc;
    private static readonly unsafe BrushColorFunc BrushColorFunc;

    static unsafe Brush()
    {
        CreateBrushFunc = (CreateBrushFunc)Linker.Instance.GetSymbol("%CreateBrush#red=255#green=255#blue=255");
        FreeBrushFunc = (FreeBrushFunc)Linker.Instance.GetSymbol("FreeBrush%brush");
        BrushTextureFunc = (BrushTextureFunc)Linker.Instance.GetSymbol("BrushTexture%brush%texture%frame=0%index=0");
        BrushColorFunc = (BrushColorFunc)Linker.Instance.GetSymbol("BrushColor%brush#red#green#blue");
    }

    public int Handle { get; } = handle;

    public static unsafe Brush Create(float r, float g, float b)
    {
        return new Brush(CreateBrushFunc(r, g, b));
    }

    public unsafe void Free()
    {
        FreeBrushFunc(this.Handle);
    }

    public unsafe void BrushTexture(Texture texture, int frame, int index)
    {
        BrushTextureFunc(this.Handle, texture.Handle, frame, index);
    }

    public unsafe void BrushColor(float r, float g, float b)
    {
        BrushColorFunc(this.Handle, r, g, b);
    }
}