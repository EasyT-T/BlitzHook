namespace BlitzHook.Engine;

using unsafe CreateTextureFunc = delegate* unmanaged[Stdcall]<int, int, int, int, int>;
using unsafe TextureBufferFunc = delegate* unmanaged[Stdcall]<int, int, int>;
using unsafe FreeTextureFunc = delegate* unmanaged[Stdcall]<int, void>;

public readonly struct Texture(int handle) : IEngineObject
{
    [Flags]
    public enum TextureType
    {
        CanvasTexRgb = 0x0001,
        CanvasTexAlpha = 0x0002,
        CanvasTexMask = 0x0004,
        CanvasTexMipmap = 0x0008,
        CanvasTexClampu = 0x0010,
        CanvasTexClampv = 0x0020,
        CanvasTexSphere = 0x0040,
        CanvasTexCube = 0x0080,
        CanvasTexVidmem = 0x0100,
        CanvasTexHicolor = 0x0200,

        CanvasTexture = 0x10000,
        CanvasNondisplay = 0x20000,
        CanvasHighcolor = 0x40000,
    }

    private static readonly unsafe CreateTextureFunc CreateTextureFunc;

    private static readonly unsafe TextureBufferFunc TextureBufferFunc;

    private static readonly unsafe FreeTextureFunc FreeTextureFunc;

    static unsafe Texture()
    {
        CreateTextureFunc = (CreateTextureFunc)Linker.Instance.GetSymbol("%CreateTexture%width%height%flags=0%frames=1");
        TextureBufferFunc = (TextureBufferFunc)Linker.Instance.GetSymbol("%TextureBuffer%texture%frame=0");
        FreeTextureFunc = (FreeTextureFunc)Linker.Instance.GetSymbol("FreeTexture%texture");
    }

    public int Handle { get; } = handle;

    public static unsafe Texture Create(int width, int height, TextureType flags, int frames)
    {
        return new Texture(CreateTextureFunc(width, height, (int)flags, frames));
    }

    public unsafe Canvas GetBuffer(int frame)
    {
        return new Canvas(TextureBufferFunc(this.Handle, frame));
    }

    public unsafe void Free()
    {
        FreeTextureFunc(this.Handle);
    }
}