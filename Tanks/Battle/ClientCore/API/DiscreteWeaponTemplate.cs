namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(-1716200834009238305L)]
    public interface DiscreteWeaponTemplate : WeaponTemplate, Template
    {
        DirectionEvaluatorComponent directionEvaluator();
        DiscreteWeaponControllerComponent discreteWeaponController();
        DiscreteWeaponEnergyComponent discreteWeaponEnergy();
        [PersistentConfig("", false)]
        DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator();
        ImpactComponent impact();
        KickbackComponent kickback();
        [AutoAdded]
        WeaponHitStrongComponent weaponHitStrong();
    }
}

