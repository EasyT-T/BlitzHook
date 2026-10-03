namespace BlitzHook;

public class Linker
{
    public static Linker Instance { get; } = new Linker();

    private readonly Dictionary<string, nint> _symbols = new Dictionary<string, nint>();

    public void AddSymbol(string symbol, nint value)
    {
        this._symbols[symbol] = value;
    }

    public nint GetSymbol(string symbol)
    {
        return this._symbols[symbol];
    }
}