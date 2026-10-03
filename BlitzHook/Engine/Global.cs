namespace BlitzHook.Engine;

using unsafe RenderWorldFunc = delegate* unmanaged[Stdcall]<float, void>;
using unsafe GraphicsWidthFunc = delegate* unmanaged[Stdcall]<int>;
using unsafe GraphicsHeightFunc = delegate* unmanaged[Stdcall]<int>;
using unsafe ClsFunc = delegate* unmanaged[Stdcall]<void>;
using unsafe DrawImageFunc = delegate* unmanaged[Stdcall]<int, int, int, int, void>;
using unsafe WireFrameFunc = delegate* unmanaged[Stdcall]<int, void>;

public static class Global
{
    private static readonly unsafe RenderWorldFunc RenderWorldFunc;
    private static readonly unsafe GraphicsWidthFunc GraphicsWidthFunc;
    private static readonly unsafe GraphicsHeightFunc GraphicsHeightFunc;
    private static readonly unsafe DrawImageFunc DrawImageFunc;
    private static readonly unsafe ClsFunc ClsFunc;
    private static readonly unsafe WireFrameFunc WireFrameFunc;

    static unsafe Global()
    {
        RenderWorldFunc = (RenderWorldFunc)Linker.Instance.GetSymbol("RenderWorld#tween=1");
        GraphicsWidthFunc = (GraphicsWidthFunc)Linker.Instance.GetSymbol("%GraphicsWidth");
        GraphicsHeightFunc = (GraphicsHeightFunc)Linker.Instance.GetSymbol("%GraphicsHeight");
        DrawImageFunc = (DrawImageFunc)Linker.Instance.GetSymbol("DrawImage%image%x%y%frame=0");
        WireFrameFunc = (WireFrameFunc)Linker.Instance.GetSymbol("WireFrame%enable");

        ClsFunc = (ClsFunc)Linker.Instance.GetSymbol("Cls");
    }

    public static unsafe void RenderWorld(float tween)
    {
        RenderWorldFunc(tween);
    }

    public static unsafe int GraphicsWidth()
    {
        return GraphicsWidthFunc();
    }

    public static unsafe int GraphicsHeight()
    {
        return GraphicsHeightFunc();
    }

    public static unsafe void Cls()
    {
        ClsFunc();
    }

    public static unsafe void DrawImage(int image, int x, int y, int frame)
    {
        DrawImageFunc(image, x, y, frame);
    }

    public static unsafe void WireFrame(bool enable)
    {
        WireFrameFunc(enable ? 1 : 0);
    }
}