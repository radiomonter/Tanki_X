namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class MouseControlSettingsSystem : ECSSystem
    {
        private const float MouseSensivityRatio = 1.5f;

        [OnEventFire]
        public void InitWeaponRotationControl(NodeAddedEvent e, SelfBattleUser selfBattleUser, SingleNode<GameMouseSettingsComponent> settings)
        {
            selfBattleUser.mouseControlStateHolder.MouseControlAllowed = settings.component.MouseControlAllowed;
            selfBattleUser.mouseControlStateHolder.MouseControlEnable = settings.component.MouseControlAllowed;
            selfBattleUser.mouseControlStateHolder.MouseVerticalInverted = settings.component.MouseVerticalInverted;
            selfBattleUser.mouseControlStateHolder.MouseSensivity = settings.component.MouseSensivity * 1.5f;
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class SelfBattleUser : Node
        {
            public MouseControlStateHolderComponent mouseControlStateHolder;
            public SelfBattleUserComponent selfBattleUser;
        }
    }
}

