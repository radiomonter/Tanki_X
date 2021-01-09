namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class MainHUDSystem : ECSSystem
    {
        [OnEventFire]
        public void AttachHudChildren(NodeAddedEvent e, MainHudScreenGroup mainHud)
        {
            EntityBehaviour behaviour = mainHud.mainHUD.GetComponent<EntityBehaviour>();
            foreach (EntityBehaviour behaviour2 in mainHud.mainHUD.GetComponentsInChildren<EntityBehaviour>(true))
            {
                if (!behaviour2.Equals(behaviour))
                {
                    AttachToScreenComponent component = behaviour2.gameObject.GetComponent<AttachToScreenComponent>();
                    if (component == null)
                    {
                        component = behaviour2.gameObject.AddComponent<AttachToScreenComponent>();
                    }
                    component.JoinEntityBehaviour = behaviour;
                    Entity entity = behaviour2.Entity;
                    if ((entity != null) && behaviour2.handleAutomaticaly)
                    {
                        entity.RemoveComponentIfPresent<AttachToScreenComponent>();
                        entity.RemoveComponentIfPresent<ScreenGroupComponent>();
                        entity.AddComponent(component);
                    }
                }
            }
        }

        [OnEventFire]
        public void ChangeHP(HealthChangedEvent e, HUDNodes.SelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            hud.component.CurrentHpValue = tank.health.CurrentHealth;
        }

        [OnEventFire]
        public void ChangeHP(NodeAddedEvent e, HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud, SingleNode<HPContainerComponent> hpContainer)
        {
            hud.component.CurrentHpValue = tank.health.CurrentHealth;
        }

        [OnEventFire]
        public void InitForSpectator(NodeAddedEvent e, HUDNodes.SelfBattleUserAsSpectatorNode spec, SingleNode<MainHUDComponent> hud, SingleNode<DMHUDMessagesComponent> messages)
        {
            hud.component.SetSpecatatorMode();
            hud.component.ShowMessageWithPriority(messages.component.SpectatorMessage, 0);
        }

        [OnEventFire]
        public void InitForTank(NodeAddedEvent e, HUDNodes.SelfBattleUserAsTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            hud.component.SetTankMode();
        }

        [OnEventFire]
        public void InitHP(NodeAddedEvent e, HUDNodes.DeadSelfTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            hud.component.CurrentHpValue = 0f;
            hud.component.CurrentEnergyValue = 0f;
        }

        [OnEventFire]
        public void InitHP(NodeAddedEvent e, HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            hud.component.MaxHpValue = tank.healthConfig.BaseHealth;
            hud.component.CurrentHpValue = tank.health.CurrentHealth;
        }

        [OnEventFire]
        public void InitHUD(NodeAddedEvent e, HUDNodes.ActiveSelfTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            hud.component.Activate();
        }

        [OnEventFire]
        public void InitHUD(NodeAddedEvent e, MainHUDNode hud, HUDNodes.SemiActiveSelfTankNode tank)
        {
            hud.mainHUD.Activate();
            hud.mainHUD.CurrentEnergyValue = 0f;
            if (!hud.Entity.HasComponent<ScreenGroupComponent>())
            {
                hud.Entity.CreateGroup<ScreenGroupComponent>();
            }
        }

        [OnEventFire]
        public void InitHUDForSpectator(NodeAddedEvent e, HUDNodes.SelfBattleUserAsSpectatorNode spectator, SingleNode<MainHUDComponent> hud)
        {
            hud.component.Activate();
        }

        [OnEventFire]
        public void InitTankIcons(NodeAddedEvent e, SingleNode<MainHUDComponent> hud, SelfUser selfUser)
        {
            hud.component.HullId = selfUser.userEquipment.HullId;
            hud.component.TurretId = selfUser.userEquipment.WeaponId;
        }

        [OnEventFire]
        public void SetCTFMessagePosition(NodeAddedEvent e, SingleNode<MainHUDComponent> hud, HUDNodes.SelfBattleUserAsSpectatorNode spec, [JoinByBattle] CTFBattleNode battle)
        {
            hud.component.SetMessageCTFPosition();
        }

        [OnEventFire]
        public void SetDefaultHUD(NodeAddedEvent e, SelfUser selfuser)
        {
            string key = "BattleHudVersion";
            if (!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetInt(key, (((int) (selfuser.Entity.Id % 2L)) != 0) ? 2 : 1);
            }
        }

        [OnEventFire]
        public void SetDMMessagePosition(NodeAddedEvent e, SingleNode<MainHUDComponent> hud, HUDNodes.SelfBattleUserAsSpectatorNode spec, [JoinByBattle] DMBattleNode battle)
        {
            hud.component.SetMessageTDMPosition();
        }

        [OnEventFire]
        public void SetTDMMessagePosition(NodeAddedEvent e, SingleNode<MainHUDComponent> hud, HUDNodes.SelfBattleUserAsSpectatorNode spec, [JoinByBattle] TDMBattleNode battle)
        {
            hud.component.SetMessageTDMPosition();
        }

        [OnEventFire]
        public void UpdateTimer(UpdateEvent e, BattleNode battle, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByBattle] RoundNode round, [JoinAll] MainHUDNode hud)
        {
            float timeLimitSec = battle.timeLimit.TimeLimitSec;
            float warmingUpTimeLimitSec = battle.timeLimit.WarmingUpTimeLimitSec;
            if (battle.Entity.HasComponent<BattleStartTimeComponent>())
            {
                float unityTime = battle.Entity.GetComponent<BattleStartTimeComponent>().RoundStartTime.UnityTime;
                if (!round.Entity.HasComponent<RoundWarmingUpStateComponent>())
                {
                    timeLimitSec -= Date.Now.UnityTime - unityTime;
                }
                else
                {
                    warmingUpTimeLimitSec = unityTime - Date.Now.UnityTime;
                }
            }
            hud.mainHUDTimers.Timer.Set(timeLimitSec);
            hud.mainHUDTimers.WarmingUpTimer.Set(warmingUpTimeLimitSec);
        }

        public class BattleNode : Node
        {
            public TimeLimitComponent timeLimit;
            public BattleGroupComponent battleGroup;
        }

        public class BattleScreenNode : Node
        {
            public BattleGroupComponent battleGroup;
            public BattleScreenComponent battleScreen;
        }

        public class BattleWithTimeNode : MainHUDSystem.BattleNode
        {
            public BattleStartTimeComponent battleStartTime;
        }

        public class CTFBattleNode : MainHUDSystem.BattleNode
        {
            public CTFComponent ctf;
        }

        public class DMBattleNode : MainHUDSystem.BattleNode
        {
            public DMComponent dm;
        }

        public class MainHUDNode : Node
        {
            public MainHUDComponent mainHUD;
            public MainHUDTimersComponent mainHUDTimers;
        }

        public class MainHudScreenGroup : MainHUDSystem.MainHUDNode
        {
            public ScreenGroupComponent screenGroup;
        }

        public class RoundNode : Node
        {
            public RoundStopTimeComponent roundStopTime;
            public RoundActiveStateComponent roundActiveState;
        }

        public class SelfUser : Node
        {
            public SelfUserComponent selfUser;
            public UserEquipmentComponent userEquipment;
        }

        public class TDMBattleNode : MainHUDSystem.BattleNode
        {
            public TDMComponent tdm;
        }

        public class TeamBattleNode : MainHUDSystem.BattleNode
        {
            public TeamBattleComponent teamBattle;
        }
    }
}

