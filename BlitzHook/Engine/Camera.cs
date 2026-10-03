namespace BlitzHook.Features;

using unsafe CreateCameraFunc = delegate* unmanaged[Stdcall]<int, int>;

public class Camera : Entity
{
    private static readonly unsafe CreateCameraFunc CreateCamera;

    static unsafe Camera()
    {
        CreateCamera = (CreateCameraFunc)Linker.Instance.GetSymbol("%CreateCamera%parent=0");
    }

    internal Camera(int handle) : base(handle)
    {
    }

    public static unsafe Camera Create(Entity? parent = null)
    {
        return new Camera(CreateCamera(parent?.Handle ?? 0));
    }
}