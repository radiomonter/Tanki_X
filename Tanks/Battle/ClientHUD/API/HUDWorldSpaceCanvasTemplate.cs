namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientHUD.Impl;

    [SerialVersionUID(0x14f49b11cdcL)]
    public interface HUDWorldSpaceCanvasTemplate : Template
    {
        Tanks.Battle.ClientHUD.Impl.HUDWorldSpaceCanvas HUDWorldSpaceCanvas();
    }
}

