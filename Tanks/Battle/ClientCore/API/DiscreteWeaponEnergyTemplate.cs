namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14d03afdda2L)]
    public interface DiscreteWeaponEnergyTemplate : DiscreteWeaponTemplate, WeaponTemplate, Template
    {
    }
}

