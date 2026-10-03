namespace BlitzHook.Engine;

using unsafe FreeEntityFunc = delegate* unmanaged[Stdcall]<int, void>;
using unsafe EntityTextureFunc = delegate* unmanaged[Stdcall]<int, int, int, int, void>;
using unsafe HideEntityFunc = delegate* unmanaged[Stdcall]<int, void>;
using unsafe ShowEntityFunc = delegate* unmanaged[Stdcall]<int, void>;
using unsafe PositionEntityFunc = delegate* unmanaged[Stdcall]<int, float, float, float, int, void>;
using unsafe EntityFXFunc = delegate* unmanaged[Stdcall]<int, int, void>;
using unsafe EntityOrderFunc = delegate* unmanaged[Stdcall]<int, int, void>;
using unsafe CameraZoomFunc = delegate* unmanaged[Stdcall]<int, float, void>;
using unsafe CameraRangeFunc = delegate* unmanaged[Stdcall]<int, float, float, void>;

public interface IEntity : IEngineObject;

public static class EntityExtensions
{
    private static readonly unsafe FreeEntityFunc FreeEntity;
    private static readonly unsafe EntityTextureFunc EntityTexture;
    private static readonly unsafe HideEntityFunc HideEntity;
    private static readonly unsafe ShowEntityFunc ShowEntity;
    private static readonly unsafe PositionEntityFunc PositionEntity;
    private static readonly unsafe EntityFXFunc EntityFX;
    private static readonly unsafe EntityOrderFunc EntityOrder;
    private static readonly unsafe CameraZoomFunc CameraZoom;
    private static readonly unsafe CameraRangeFunc CameraRange;

    static unsafe EntityExtensions()
    {
        FreeEntity = (FreeEntityFunc)Linker.Instance.GetSymbol("FreeEntity%entity");
        EntityTexture = (EntityTextureFunc)Linker.Instance.GetSymbol("EntityTexture%entity%texture%frame=0%index=0");
        HideEntity = (HideEntityFunc)Linker.Instance.GetSymbol("HideEntity%entity");
        ShowEntity = (ShowEntityFunc)Linker.Instance.GetSymbol("ShowEntity%entity");
        PositionEntity = (PositionEntityFunc)Linker.Instance.GetSymbol("PositionEntity%entity#x#y#z%global=0");
        EntityFX = (EntityFXFunc)Linker.Instance.GetSymbol("EntityFX%entity%fx");
        EntityOrder = (EntityOrderFunc)Linker.Instance.GetSymbol("EntityOrder%entity%order");
        CameraZoom = (CameraZoomFunc)Linker.Instance.GetSymbol("CameraZoom%camera#zoom");
        CameraRange = (CameraRangeFunc)Linker.Instance.GetSymbol("CameraRange%camera#near#far");
    }

    public static unsafe void Free<T>(this T entity) where T : IEntity
    {
        FreeEntity(entity.Handle);
    }

    public static unsafe void SetTexture<T>(this T entity, Texture texture, int frame, int index) where T : IEntity
    {
        EntityTexture(entity.Handle, texture.Handle, frame, index);
    }

    public static unsafe void Hide<T>(this T entity) where T : IEntity
    {
        HideEntity(entity.Handle);
    }

    public static unsafe void Show<T>(this T entity) where T : IEntity
    {
        ShowEntity(entity.Handle);
    }

    public static unsafe void Position<T>(this T entity, float x, float y, float z, int global = 0) where T : IEntity
    {
        PositionEntity(entity.Handle, x, y, z, global);
    }

    public static unsafe void SetFX<T>(this T entity, int fx) where T : IEntity
    {
        EntityFX(entity.Handle, fx);
    }

    public static unsafe void SetOrder<T>(this T entity, int order) where T : IEntity
    {
        EntityOrder(entity.Handle, order);
    }

    public static unsafe void Zoom<T>(this T camera, float zoom) where T : IEntity
    {
        CameraZoom(camera.Handle, zoom);
    }

    public static unsafe void Range<T>(this T camera, float near, float far) where T : IEntity
    {
        CameraRange(camera.Handle, near, far);
    }
}