namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x4091f4b1d589d89dL)]
    public interface FlamethrowerBattleItemTemplate : StreamWeaponTemplate, WeaponTemplate, Template
    {
        FlamethrowerComponent flamethrower();
    }
}

