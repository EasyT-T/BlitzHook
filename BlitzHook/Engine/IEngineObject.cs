namespace BlitzHook.Engine;

public interface IEngineObject : IEquatable<IEngineObject>
{
    int Handle { get; }

    public bool IsNull => this.Handle == 0;

    bool IEquatable<IEngineObject>.Equals(IEngineObject? other) => other != null && this.Handle == other.Handle;
}

public static class EngineObjectExtensions
{
    public static bool IsNull<T>(this T obj) where T : IEngineObject
    {
        return obj.Handle == 0;
    }
}