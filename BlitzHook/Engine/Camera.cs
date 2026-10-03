namespace BlitzHook.Engine;

using unsafe CreateCameraFunc = delegate* unmanaged[Stdcall]<int, int>;
using unsafe CameraViewportFunc = delegate* unmanaged[Stdcall]<int, int, int, int, int, void>;
using unsafe CameraProjModeFunc = delegate* unmanaged[Stdcall]<int, int, void>;
using unsafe CameraClsModeFunc = delegate* unmanaged[Stdcall]<int, int, int, void>;

public readonly struct Camera(int handle) : IEntity
{
    public enum ProjectMode
    {
        None = 0,
        Persp = 1,
        Ortho = 2,
    }

    private static readonly unsafe CreateCameraFunc CreateCameraFunc;
    private static readonly unsafe CameraViewportFunc CameraViewportFunc;
    private static readonly unsafe CameraProjModeFunc CameraProjModeFunc;
    private static readonly unsafe CameraClsModeFunc CameraClsModeFunc;

    static unsafe Camera()
    {
        CreateCameraFunc = (CreateCameraFunc)Linker.Instance.GetSymbol("%CreateCamera%parent=0");
        CameraViewportFunc = (CameraViewportFunc)Linker.Instance.GetSymbol("CameraViewport%camera%x%y%width%height");
        CameraProjModeFunc = (CameraProjModeFunc)Linker.Instance.GetSymbol("CameraProjMode%camera%mode");
        CameraClsModeFunc = (CameraClsModeFunc)Linker.Instance.GetSymbol("CameraClsMode%camera%cls_color%cls_zbuffer");
    }

    public int Handle { get; } = handle;

    public static unsafe Camera Create(IEntity? parent = null)
    {
        return new Camera(CreateCameraFunc(parent?.Handle ?? 0));
    }

    public unsafe void CameraViewport(int x, int y, int width, int height)
    {
        CameraViewportFunc(this.Handle, x, y, width, height);
    }

    public unsafe void CameraProjMode(ProjectMode mode)
    {
        CameraProjModeFunc(this.Handle, (int)mode);
    }

    public unsafe void CameraClsMode(bool clearColor, bool clearZBuffer)
    {
        CameraClsModeFunc(this.Handle, clearColor ? 1 : 0, clearZBuffer ? 1 : 0);
    }
}