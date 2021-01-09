namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x74a7315aed30e01L)]
    public interface FreezeBattleItemTemplate : StreamWeaponTemplate, WeaponTemplate, Template
    {
        FreezeComponent freeze();
    }
}

