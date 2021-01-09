namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(-3936735916503799349L)]
    public interface VulcanBattleItemTemplate : WeaponTemplate, Template
    {
        [PersistentConfig("", false)]
        DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator();
        ImpactComponent impact();
        KickbackComponent kickback();
        [AutoAdded, PersistentConfig("reticle", false)]
        ReticleTemplateComponent reticleTemplate();
        [PersistentConfig("", false)]
        VerticalSectorsTargetingComponent verticalSectorsTargeting();
        VulcanComponent vulcan();
        [AutoAdded]
        VulcanEnergyBarComponent vulcanEnergyBar();
        [PersistentConfig("", false)]
        VulcanWeaponComponent vulcanWeapon();
        [AutoAdded]
        VulcanWeaponStateComponent vulcanWeaponState();
    }
}

