namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class VulcanEnergyBarSystem : ECSSystem
    {
        [OnEventFire]
        public void Idle(TimeUpdateEvent e, VulcanWeaponIdleNode vulcan, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            hud.component.CurrentEnergyValue = 0f;
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, VulcanWeaponNode vulcan, [JoinByTank, Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            hud.component.EnergyBarEnabled = true;
            hud.component.EnergyAmountPerSegment = 1f;
            hud.component.MaxEnergyValue = 2f;
            hud.component.CurrentEnergyValue = 0f;
        }

        [OnEventFire]
        public void SlowDown(NodeAddedEvent e, VulcanWeaponSlowDownNode vulcan, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            hud.component.StopEnergyBlink();
            if (vulcan.Entity.HasComponent<AnimationDataComponent>())
            {
                vulcan.Entity.RemoveComponent<AnimationDataComponent>();
            }
            AnimationDataComponent component = new AnimationDataComponent {
                CooldownScale = hud.component.CurrentEnergyValue
            };
            vulcan.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SlowDown(TimeUpdateEvent e, VulcanWeaponSlowDownNode vulcan, [JoinByTank] SingleNode<AnimationDataComponent> animData, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            hud.component.CurrentEnergyValue = vulcan.vulcanWeaponState.State * animData.component.CooldownScale;
            if (InputManager.GetActionKeyDown(ShotActions.SHOT))
            {
                hud.component.EnergyBlink(false);
            }
        }

        [OnEventFire]
        public void SpeedUp(TimeUpdateEvent e, VulcanWeaponSpeedUpNode vulcan, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            hud.component.CurrentEnergyValue = vulcan.vulcanWeaponState.State;
        }

        [OnEventFire]
        public void Working(TimeUpdateEvent e, VulcanWeaponWorkingNode vulcan, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            float temperatureHittingTime = vulcan.vulcanWeapon.TemperatureHittingTime;
            float num3 = temperatureHittingTime - (Date.Now - vulcan.weaponStreamShooting.StartShootingTime);
            if (num3 < 0f)
            {
                num3 = 0f;
                hud.component.EnergyBlink(false);
            }
            hud.component.CurrentEnergyValue = (vulcan.vulcanWeaponState.State + 1f) - (num3 / temperatureHittingTime);
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        [Inject]
        public static UnityTime Time { get; set; }

        public class AnimationDataComponent : Component
        {
            public float CooldownScale { get; set; }
        }

        public class VulcanWeaponIdleNode : VulcanEnergyBarSystem.VulcanWeaponNode
        {
            public VulcanIdleComponent vulcanIdle;
        }

        public class VulcanWeaponNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanWeaponStateComponent vulcanWeaponState;
            public TankGroupComponent tankGroup;
            public CooldownTimerComponent cooldownTimer;
            public VulcanEnergyBarComponent vulcanEnergyBar;
        }

        public class VulcanWeaponSlowDownNode : VulcanEnergyBarSystem.VulcanWeaponNode
        {
            public VulcanSlowDownComponent vulcanSlowDown;
        }

        public class VulcanWeaponSpeedUpNode : VulcanEnergyBarSystem.VulcanWeaponNode
        {
            public VulcanSpeedUpComponent vulcanSpeedUp;
        }

        public class VulcanWeaponWorkingNode : VulcanEnergyBarSystem.VulcanWeaponNode
        {
            public WeaponStreamShootingComponent weaponStreamShooting;
        }
    }
}

