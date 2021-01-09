namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d4f6a83121217cL)]
    public interface GoldBonusWithTeleportTemplate : GoldBonusTemplate, BonusTemplate, Template
    {
    }
}

