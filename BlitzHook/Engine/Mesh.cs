namespace BlitzHook.Engine;

using unsafe CreateMeshFunc = delegate* unmanaged[Stdcall]<int, int>;
using unsafe CreateSurfaceFunc = delegate* unmanaged[Stdcall]<int, int, int>;

public readonly struct Mesh(int handle) : IEntity
{
    private static readonly unsafe CreateMeshFunc CreateMeshFunc;
    private static readonly unsafe CreateSurfaceFunc CreateSurfaceFunc;

    static unsafe Mesh()
    {
        CreateMeshFunc = (CreateMeshFunc)Linker.Instance.GetSymbol("%CreateMesh%parent=0");
        CreateSurfaceFunc = (CreateSurfaceFunc)Linker.Instance.GetSymbol("%CreateSurface%mesh%brush=0");
    }

    public int Handle { get; } = handle;

    public static unsafe Mesh Create(IEntity? parent = null)
    {
        return new Mesh(CreateMeshFunc(parent?.Handle ?? 0));
    }

    public unsafe Surface CreateSurface(Brush brush)
    {
        return new Surface(CreateSurfaceFunc(this.Handle, brush.Handle));
    }
}