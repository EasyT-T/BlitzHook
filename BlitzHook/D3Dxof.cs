namespace BlitzHook;

using System.Runtime.InteropServices;

internal static unsafe partial class D3Dxof
{
    [LibraryImport("d3dxof_orig.dll", EntryPoint = "DirectXFileCreate", SetLastError = true)]
    private static partial int OrigDirectXFileCreate(IntPtr* lplpDirectXFile);

    [LibraryImport("d3dxof_orig.dll", EntryPoint = "DllCanUnloadNow", SetLastError = true)]
    private static partial int OrigDllCanUnloadNow();

    [LibraryImport("d3dxof_orig.dll", EntryPoint = "DllGetClassObject", SetLastError = true)]
    private static partial int OrigDllGetClassObject(Guid* rclsid, Guid* riid, IntPtr* ppv);

    [UnmanagedCallersOnly(EntryPoint = "DirectXFileCreate")]
    public static int DirectXFileCreate(IntPtr* lplpDirectXFile) => OrigDirectXFileCreate(lplpDirectXFile);

    [UnmanagedCallersOnly(EntryPoint = "DllCanUnloadNow")]
    public static int DllCanUnloadNow() => OrigDllCanUnloadNow();

    [UnmanagedCallersOnly(EntryPoint = "DllGetClassObject")]
    public static int DllGetClassObject(Guid* rclsid, Guid* riid, IntPtr* ppv) => OrigDllGetClassObject(rclsid, riid, ppv);
}