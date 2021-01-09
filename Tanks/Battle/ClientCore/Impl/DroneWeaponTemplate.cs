namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x159d4df5cbfL)]
    public interface DroneWeaponTemplate : Template
    {
        [AutoAdded]
        DirectionEvaluatorComponent directionEvaluator();
        [AutoAdded]
        ShotIdComponent shotIdComponent();
        [AutoAdded, PersistentConfig("", false)]
        VerticalTargetingComponent verticalTargeting();
        [AutoAdded]
        WeaponUnblockedComponent weaponUnblocked();
    }
}

