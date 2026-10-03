namespace BlitzHook.Engine;

using unsafe FreeEntityFunc = delegate* unmanaged[Stdcall]<int, void>;

public abstract class Entity : IDisposable
{
    private static readonly unsafe FreeEntityFunc FreeEntity;

    static unsafe Entity()
    {
        FreeEntity = (FreeEntityFunc)Linker.Instance.GetSymbol("FreeEntity%entity");
    }

    protected internal int Handle { get; }

    internal Entity(int handle)
    {
        this.Handle = handle;
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public unsafe void Dispose()
    {
        this.Dispose(true);

        FreeEntity(this.Handle);

        GC.SuppressFinalize(this);
    }
}