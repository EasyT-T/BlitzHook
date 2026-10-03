namespace BlitzHook.Engine;

using unsafe AddVertexFunc = delegate* unmanaged[Stdcall]<int, float, float, float, float, float, float, int>;
using unsafe AddTriangleFunc = delegate* unmanaged[Stdcall]<int, int, int, int, int>;
using unsafe ClearSurfaceFunc = delegate* unmanaged[Stdcall]<int, int, int, void>;
using unsafe VertexColorFunc = delegate* unmanaged[Stdcall]<int, int, float, float, float, float, void>;
using unsafe PaintSurfaceFunc = delegate* unmanaged[Stdcall]<int, int, void>;

public readonly struct Surface(int handle) : IEngineObject
{
    private static readonly unsafe AddVertexFunc AddVertexFunc;
    private static readonly unsafe AddTriangleFunc AddTriangleFunc;
    private static readonly unsafe ClearSurfaceFunc ClearSurfaceFunc;
    private static readonly unsafe VertexColorFunc VertexColorFunc;
    private static readonly unsafe PaintSurfaceFunc PaintSurfaceFunc;

    static unsafe Surface()
    {
        AddVertexFunc = (AddVertexFunc)Linker.Instance.GetSymbol("%AddVertex%surface#x#y#z#u=0#v=0#w=1");
        AddTriangleFunc = (AddTriangleFunc)Linker.Instance.GetSymbol("%AddTriangle%surface%v0%v1%v2");
        ClearSurfaceFunc = (ClearSurfaceFunc)Linker.Instance.GetSymbol("ClearSurface%surface%clear_vertices=1%clear_triangles=1");
        VertexColorFunc = (VertexColorFunc)Linker.Instance.GetSymbol("VertexColor%surface%index#red#green#blue#alpha=1");
        PaintSurfaceFunc = (PaintSurfaceFunc)Linker.Instance.GetSymbol("PaintSurface%surface%brush");
    }

    public int Handle { get; } = handle;

    public unsafe int AddVertex(float x, float y, float z, float u, float v, float w)
    {
        return AddVertexFunc(this.Handle, x, y, z, u, v, w);
    }

    public unsafe int AddTriangle(int a, int b, int c)
    {
        return AddTriangleFunc(this.Handle, a, b, c);
    }

    public unsafe void Clear(bool clearVertices, bool clearTriangles)
    {
        ClearSurfaceFunc(this.Handle, clearVertices ? 1 : 0, clearTriangles ? 1 : 0);
    }

    public unsafe void VertexColor(int n, float r, float g, float b, float a)
    {
        VertexColorFunc(this.Handle, n, r, g, b, a);
    }

    public unsafe void PaintSurface(Brush brush)
    {
        PaintSurfaceFunc(this.Handle, brush.Handle);
    }
}