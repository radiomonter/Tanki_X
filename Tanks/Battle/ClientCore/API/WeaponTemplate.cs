namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(0x14d03a77a29L)]
    public interface WeaponTemplate : Template
    {
        CooldownTimerComponent cooldownTimer();
        [PersistentConfig("", false)]
        CTFTargetEvaluatorComponent ctfTargetEvaluator();
        [AutoAdded]
        ShotIdComponent shotId();
        [AutoAdded]
        ShotValidateComponent shotValidate();
        TankPartComponent tankPart();
        TargetCollectorComponent targetCollector();
        TeamTargetEvaluatorComponent teamTargetEvaluator();
        WeaponComponent weapon();
        WeaponCooldownComponent weaponCooldown();
        [AutoAdded, PersistentConfig("", true)]
        WeaponGyroscopeComponent weaponGyroscope();
        [AutoAdded]
        WeaponGyroscopeRotationComponent weaponGyroscopeRotationComponent();
        WeaponInstanceComponent weaponInstance();
        [AutoAdded]
        WeaponRotationControlComponent weaponRotationControl();
    }
}

