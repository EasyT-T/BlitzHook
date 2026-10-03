namespace BlitzHook;

using System.Runtime.InteropServices;
using BlitzHook.Hooks;

public static unsafe class EntryPoint
{
    private static readonly HashSet<IHook> Hooks = [];

    [UnmanagedCallersOnly(EntryPoint = "OnDllLoaded")]
    public static void OnDllLoaded()
    {
        try
        {
            var runtime = Runtime.Get();

            nint sym;

            while ((sym = runtime.GetNextSym()) != 0)
            {
                Linker.Instance.AddSymbol(Marshal.PtrToStringUTF8(sym)!, runtime.GetSymValue(sym));
            }

            var loadLibsHook = new LoadLibsHook();
            loadLibsHook.Setup();
            Hooks.Add(loadLibsHook);

            Console.WriteLine("Hook initialized");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}