namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class HammerEnergyBarSystem : ECSSystem
    {
        [OnEventFire]
        public void AddAnimationData(NodeAddedEvent e, HammerReloadEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank)
        {
            if (!weapon.Entity.HasComponent<ReloadAnimationDataComponent>())
            {
                weapon.Entity.AddComponent<ReloadAnimationDataComponent>();
            }
            weapon.Entity.GetComponent<ReloadAnimationDataComponent>().CurrentDuration = 0f;
        }

        private float EaseInQuad(float time, float from, float delta) => 
            ((delta * time) * time) + from;

        [OnEventFire]
        public void Init(NodeAddedEvent e, HammerEnergyNode weapon, [JoinByTank, Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            hud.component.EnergyBarEnabled = true;
            hud.component.MaxEnergyValue = weapon.magazineWeapon.MaxCartridgeCount;
            hud.component.EnergyAmountPerSegment = 1f;
            hud.component.CurrentEnergyValue = 0f;
        }

        [OnEventComplete]
        public void Init(NodeAddedEvent e, HammerReadyEnergyNode weapon, [JoinByTank, Context] HUDNodes.ActiveSelfTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            this.SetMagazineAsReady(weapon, hud);
        }

        [OnEventComplete]
        public void Init(SetMagazineReadyEvent e, HammerReadyEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            this.SetMagazineAsReady(weapon, hud);
        }

        [OnEventFire]
        public void Reload(TimeUpdateEvent e, HammerEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            if (((weapon.cooldownTimer.CooldownTimerSec > 0f) || (weapon.Entity.HasComponent<MagazineReloadStateComponent>() || !weapon.Entity.HasComponent<ShootableComponent>())) && InputManager.GetActionKeyDown(ShotActions.SHOT))
            {
                hud.component.EnergyBlink(false);
            }
        }

        [OnEventFire]
        public void Reload(TimeUpdateEvent e, HammerReloadingEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            float num3 = weapon.magazineWeapon.ReloadMagazineTimePerSec / ((float) weapon.magazineWeapon.MaxCartridgeCount);
            float currentDuration = weapon.reloadAnimationData.CurrentDuration;
            int num5 = (int) (currentDuration / num3);
            hud.component.CurrentEnergyValue = this.EaseInQuad((currentDuration - (num5 * num3)) / num3, (float) num5, 1f);
            weapon.reloadAnimationData.CurrentDuration += e.DeltaTime;
        }

        private void SetMagazineAsReady(HammerReadyEnergyNode weapon, SingleNode<MainHUDComponent> hud)
        {
            hud.component.CurrentEnergyValue = weapon.magazineWeapon.MaxCartridgeCount;
            if (weapon.Entity.HasComponent<ReloadAnimationDataComponent>())
            {
                weapon.Entity.RemoveComponent<ReloadAnimationDataComponent>();
            }
            hud.component.EnergyBlink(true);
        }

        [OnEventFire]
        public void UpdateOnTrigger(BaseShotEvent evt, HammerReadyEnergyNode hammerEnergy, [JoinByTank] HUDNodes.ActiveSelfTankNode selfNode, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            float num = hammerEnergy.magazineStorage.CurrentCartridgeCount - 1;
            hud.component.CurrentEnergyValue = num;
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class HammerEnergyNode : Node
        {
            public WeaponComponent weapon;
            public HammerComponent hammer;
            public MagazineWeaponComponent magazineWeapon;
            public MagazineStorageComponent magazineStorage;
            public TankGroupComponent tankGroup;
            public HammerEnergyBarComponent hammerEnergyBar;
            public CooldownTimerComponent cooldownTimer;
        }

        public class HammerReadyEnergyNode : HammerEnergyBarSystem.HammerEnergyNode
        {
            public MagazineReadyStateComponent magazineReadyState;
        }

        public class HammerReloadEnergyNode : HammerEnergyBarSystem.HammerEnergyNode
        {
            public MagazineReloadStateComponent magazineReloadState;
        }

        public class HammerReloadingEnergyNode : HammerEnergyBarSystem.HammerReloadEnergyNode
        {
            public HammerEnergyBarSystem.ReloadAnimationDataComponent reloadAnimationData;
        }

        public class ReloadAnimationDataComponent : Component
        {
            public float CurrentDuration { get; set; }
        }
    }
}

