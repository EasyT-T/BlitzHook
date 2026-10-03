namespace BlitzHook.Features;

using unsafe CreateMeshFunc = delegate* unmanaged[Stdcall]<int, int>;

public class Mesh : Entity
{
    private static readonly unsafe CreateMeshFunc CreateMesh;

    static unsafe Mesh()
    {
        CreateMesh = (CreateMeshFunc)Linker.Instance.GetSymbol("%CreateMesh%parent=0");
    }

    internal Mesh(int handle) : base(handle)
    {
    }

    public static unsafe Mesh Create(Entity? parent = null)
    {
        return new Mesh(CreateMesh(parent?.Handle ?? 0));
    }
}