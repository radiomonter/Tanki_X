namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(-2537616944465628484L)]
    public interface ShaftBattleItemTemplate : WeaponTemplate, Template
    {
        DirectionEvaluatorComponent directionEvaluator();
        [PersistentConfig("", false)]
        DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator();
        [AutoAdded]
        EnergyBarComponent energyBar();
        ImpactComponent impact();
        KickbackComponent kickback();
        [AutoAdded, PersistentConfig("reticle", false)]
        ReticleTemplateComponent reticleTemplate();
        ShaftComponent shaft();
        ShaftAimingImpactComponent shaftAimingImpact();
        ShaftEnergyComponent shaftEnergy();
        [PersistentConfig("", false)]
        ShaftStateConfigComponent shaftStateConfig();
        ShaftStateControllerComponent shaftStateController();
        [PersistentConfig("", false)]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();
    }
}

