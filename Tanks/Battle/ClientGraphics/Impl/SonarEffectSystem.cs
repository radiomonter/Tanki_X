namespace Tanks.Battle.ClientGraphics.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Battle.ClientHUD.API;
    using Tanks.Battle.ClientHUD.Impl;
    using Tanks.Lobby.ClientEntrance.API;

    public class SonarEffectSystem : ECSSystem
    {
        private const string _outlineColorPropertyName = "_OutlineColor";

        [OnEventFire]
        public void ActivateTankOutlineEffect(NodeAddedEvent e, SonarEffectNode sonarEffect, [Context, JoinByTank] SelfTankNode tank, InitializedMapOutlineEffectNode mapEffect)
        {
            base.ScheduleEvent<ActivateTankOutlineMapEffectEvent>(mapEffect);
        }

        [OnEventFire]
        public void ActivateTankOutlineMapEffect(ActivateTankOutlineMapEffectEvent evt, BlinkerTankOutlineMapEffectNode mapEffect)
        {
            mapEffect.tankOutlineMapEffect.ActivateEffect();
        }

        [OnEventFire]
        public void ActivateTankOutlineMapEffect(ActivateTankOutlineMapEffectEvent evt, DeactivationTankOutlineMapEffectNode mapEffect)
        {
            mapEffect.tankOutlineMapEffect.ActivateEffect();
        }

        [OnEventFire]
        public void ActivateTankOutlineMapEffect(ActivateTankOutlineMapEffectEvent evt, IdleTankOutlineMapEffectNode mapEffect)
        {
            mapEffect.tankOutlineMapEffect.ActivateEffect();
        }

        [OnEventFire]
        public void DeactivateTankOutlineEffect(NodeRemoveEvent e, SonarEffectNode sonarEffect, [JoinByTank] SelfTankNode tank, [JoinAll] InitializedMapOutlineEffectNode mapEffect)
        {
            base.ScheduleEvent<RunBlinkerForTankOutlineMapEffectEvent>(mapEffect);
        }

        [OnEventFire]
        public void DeactivateTankOutlineMapEffect(DeactivateTankOutlineMapEffectEvent evt, ActivationTankOutlineMapEffectNode mapEffect)
        {
            mapEffect.tankOutlineMapEffect.DeactivateEffect();
        }

        [OnEventFire]
        public void DeactivateTankOutlineMapEffect(DeactivateTankOutlineMapEffectEvent evt, BlinkerTankOutlineMapEffectNode mapEffect)
        {
            mapEffect.tankOutlineMapEffect.DeactivateEffect();
        }

        [OnEventFire]
        public void DeactivateTankOutlineMapEffect(DeactivateTankOutlineMapEffectEvent evt, WorkingTankOutlineMapEffectNode mapEffect)
        {
            mapEffect.tankOutlineMapEffect.DeactivateEffect();
        }

        [OnEventFire]
        public void DisableHealthBarForEnemies(NodeRemoveEvent e, SonarEffectNode sonarEffect, [Context, JoinByTank] SelfTankNode selfTank, [Context, Combine] RemoteTankNode remoteTank, [Context, JoinByTank] NameplateNode nameplate)
        {
            if (!remoteTank.Entity.IsSameGroup<TeamGroupComponent>(selfTank.Entity))
            {
                nameplate.nameplate.alwaysVisible = false;
                nameplate.healthBar.HideHealthBar();
            }
        }

        [OnEventFire]
        public void DisableHealthBarOnSpawn(NodeAddedEvent e, [Context, Combine] RemoteTankNode remoteTank, [Context, JoinByTank] NameplateNode nameplate, [Context] SelfTankNode selfTank, [JoinByTank] Optional<SonarEffectNode> sonarEffect)
        {
            if (!sonarEffect.IsPresent() && !remoteTank.Entity.IsSameGroup<TeamGroupComponent>(selfTank.Entity))
            {
                nameplate.healthBar.HideHealthBar();
            }
        }

        [OnEventFire]
        public void DisableHealthBarsForSpectator(NodeAddedEvent e, [Context, Combine] RemoteTankNode remoteTank, [Context, JoinByTank] NameplateNode nameplate, [Context] SelfSpectatorNode spectator)
        {
            if (!spectator.Entity.HasComponent<UserAdminComponent>() && !spectator.Entity.HasComponent<UserTesterComponent>())
            {
                nameplate.healthBar.HideHealthBar();
            }
            nameplate.nameplate.Alpha = 1f;
            nameplate.nameplate.alwaysVisible = true;
        }

        [OnEventFire]
        public void EnableHealthBarForEnemies(NodeAddedEvent e, [Context, Combine] RemoteTankNode remoteTank, [Context, JoinByTank] NameplateNode nameplate, [Context] SelfTankNode selfTank, [Context, JoinByTank] SonarEffectNode sonarEffect)
        {
            if (!remoteTank.Entity.IsSameGroup<TeamGroupComponent>(selfTank.Entity))
            {
                nameplate.nameplate.Alpha = 1f;
                nameplate.nameplate.alwaysVisible = true;
                nameplate.healthBar.ShowHealthBar();
            }
        }

        [OnEventFire]
        public void FinalizeTransparency(TransparencyFinalizeEvent evt, TransparencyTransitionTankNode tank)
        {
            if (tank.baseRenderer.Renderer.enabled)
            {
                tank.tankPartOutlineEffectUnity.UpdateTankPartOutlineEffectTransparency(ClientGraphicsConstants.OPAQUE_ALPHA);
            }
        }

        [OnEventFire]
        public void HideOutlineRenderersOnDeadState(NodeRemoveEvent evt, DeadTankNode tank, [Combine, JoinByTank] InitializedOutlineTankPartNode renderer)
        {
            renderer.tankPartOutlineEffectUnity.SwitchOutlineRenderer(false);
            tank.tankPartOutlineEffectUnity.UpdateTankPartOutlineEffectTransparency(0f);
        }

        [OnEventFire]
        public void HideOutlineRenderersOnDeadState(NodeAddedEvent evt, [Combine] InitializedOutlineTankPartNode renderer, [Context, JoinByTank] AssembledActivatedInitializedOutlineTankNode tank, [Context, JoinByTank] DeadTankNode state)
        {
            renderer.tankPartOutlineEffectUnity.SwitchOutlineRenderer(false);
        }

        [OnEventFire]
        public void InitDMTankPartsForOutlineEffect(InitDeathMatchEvent e, RemoteTankNode remoteTank, [JoinByTank] WeaponNode weapon, [JoinAll] SingleNode<TankOutlineColorsComponent> colors)
        {
            this.InitTankPartsForOutlineEffect(remoteTank, weapon);
            remoteTank.tankPartOutlineEffectUnity.MaterialForTankPart.SetColor("_OutlineColor", colors.component.DmEnemies);
            weapon.tankPartOutlineEffectUnity.MaterialForTankPart.SetColor("_OutlineColor", colors.component.DmEnemies);
        }

        [OnEventFire]
        public void InitOutlineEffect(NodeAddedEvent e, SelfBattleUserNode battleUser, [Context, JoinByUser] SelfTankNode tank, MapOutlineEffectNode mapEffect)
        {
            TankOutlineMapEffectESMComponent component = new TankOutlineMapEffectESMComponent();
            mapEffect.Entity.AddComponent(component);
            component.Esm.AddState<TankOutlineMapEffectStates.IdleState>();
            component.Esm.AddState<TankOutlineMapEffectStates.ActivationState>();
            component.Esm.AddState<TankOutlineMapEffectStates.WorkingState>();
            component.Esm.AddState<TankOutlineMapEffectStates.BlinkerState>();
            component.Esm.AddState<TankOutlineMapEffectStates.DeactivationState>();
            mapEffect.tankOutlineMapEffect.InitializeOutlineEffect(mapEffect.Entity, tank.tankVisualRoot.transform);
            mapEffect.Entity.AddComponent<TankOutlineMapEffectInitializedComponent>();
        }

        private void InitTankPartsForOutlineEffect(RemoteTankNode tank, WeaponNode weapon)
        {
            weapon.tankPartOutlineEffectUnity.InitTankPartForOutlineEffect(tank.tankPartOutlineEffectUnity.InitTankPartForOutlineEffect(null));
            tank.Entity.AddComponent<TankPartOutlineEffectInitializedComponent>();
            weapon.Entity.AddComponent<TankPartOutlineEffectInitializedComponent>();
        }

        [OnEventFire]
        public void InitTDMTankPartsForOutlineEffect(InitTeamMatchEvent e, RemoteTankTeamNode remoteTankTeam, [JoinByTank] WeaponNode weapon, [JoinByTeam] TeamNode team, [JoinAll] SelfBattleUserInTeamNode selfBattleUser, [JoinByTeam] TeamNode selfTeam, [JoinAll] SingleNode<TankOutlineColorsComponent> colors)
        {
            this.InitTankPartsForOutlineEffect(remoteTankTeam, weapon);
            if (selfTeam.Entity.Equals(team.Entity))
            {
                remoteTankTeam.tankPartOutlineEffectUnity.MaterialForTankPart.SetColor("_OutlineColor", colors.component.Allies);
                weapon.tankPartOutlineEffectUnity.MaterialForTankPart.SetColor("_OutlineColor", colors.component.Allies);
            }
            else
            {
                remoteTankTeam.tankPartOutlineEffectUnity.MaterialForTankPart.SetColor("_OutlineColor", colors.component.Enemies);
                weapon.tankPartOutlineEffectUnity.MaterialForTankPart.SetColor("_OutlineColor", colors.component.Enemies);
            }
        }

        [OnEventFire]
        public void RunBlinkerForTankOutlineMapEffect(RunBlinkerForTankOutlineMapEffectEvent evt, ActivationTankOutlineMapEffectNode mapEffect)
        {
            mapEffect.tankOutlineMapEffect.RunBlinkerForEffect();
        }

        [OnEventFire]
        public void RunBlinkerForTankOutlineMapEffect(RunBlinkerForTankOutlineMapEffectEvent evt, WorkingTankOutlineMapEffectNode mapEffect)
        {
            mapEffect.tankOutlineMapEffect.RunBlinkerForEffect();
        }

        [OnEventFire]
        public void SendDMInitEvent(NodeAddedEvent e, DMBattleNode battle, [Context, Combine] RemoteTankNode tank, [Context, JoinByTank] WeaponNode weapon, [JoinAll] SelfBattleUserNode selfBattleUser, [Context, JoinAll] SingleNode<TankOutlineColorsComponent> colors)
        {
            if (!selfBattleUser.Entity.HasComponent<UserInBattleAsSpectatorComponent>())
            {
                base.ScheduleEvent<InitDeathMatchEvent>(tank);
            }
        }

        [OnEventFire]
        public void SendTDMInitEvent(NodeAddedEvent e, TeamBattleNode battle, [Context, Combine] RemoteTankTeamNode tank, [Context, JoinByTank] WeaponNode weapon, [JoinAll] SelfBattleUserInTeamNode selfBattleUser, [Context, JoinAll] SingleNode<TankOutlineColorsComponent> colors)
        {
            if (!selfBattleUser.Entity.HasComponent<UserInBattleAsSpectatorComponent>())
            {
                base.ScheduleEvent<InitTeamMatchEvent>(tank);
            }
        }

        [OnEventFire]
        public void SetInitialTransparency(TransparencyInitEvent evt, SemiActiveTankNode tank)
        {
            tank.tankPartOutlineEffectUnity.UpdateTankPartOutlineEffectTransparency(ClientGraphicsConstants.SEMI_TRANSPARENT_ALPHA);
        }

        [OnEventFire]
        public void SetInitialTransparency(TransparencyInitEvent evt, TransparencyTransitionTankNode tank)
        {
            tank.tankPartOutlineEffectUnity.UpdateTankPartOutlineEffectTransparency(tank.transparencyTransition.CurrentAlpha);
        }

        [OnEventFire]
        public void ShowOutlineRenderersOnActiveState(NodeAddedEvent evt, [Combine] InitializedOutlineTankPartNode renderer, [Context, JoinByTank] AssembledActivatedInitializedOutlineTankNode tank, [Context, JoinByTank] ActiveTankNode state)
        {
            renderer.tankPartOutlineEffectUnity.SwitchOutlineRenderer(true);
        }

        [OnEventFire]
        public void ShowOutlineRenderersOnSemiActiveState(NodeAddedEvent evt, [Combine] InitializedOutlineTankPartNode renderer, [Context, JoinByTank] AssembledActivatedInitializedOutlineTankNode tank, [Context, JoinByTank] SemiActiveTankNode state)
        {
            renderer.tankPartOutlineEffectUnity.SwitchOutlineRenderer(true);
        }

        [OnEventFire]
        public void SwitchTankOutlineMapEffectState(TankOutlineMapEffectSwitchStateEvent e, SingleNode<TankOutlineMapEffectESMComponent> mapEffect)
        {
            mapEffect.component.Esm.ChangeState(e.StateType);
        }

        [OnEventComplete]
        public void UpdateTransparency(TimeUpdateEvent evt, TransparencyTransitionTankNode tank)
        {
            tank.tankPartOutlineEffectUnity.UpdateTankPartOutlineEffectTransparency(tank.transparencyTransition.CurrentAlpha);
        }

        public class ActivationTankOutlineMapEffectNode : SonarEffectSystem.InitializedMapOutlineEffectNode
        {
            public TankOutlineMapEffectActivationStateComponent tankOutlineMapEffectActivationState;
        }

        public class ActiveTankNode : SonarEffectSystem.AssembledActivatedInitializedOutlineTankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class AssembledActivatedInitializedOutlineTankNode : SonarEffectSystem.AssembledActivatedTankNode
        {
            public TankPartOutlineEffectInitializedComponent tankPartOutlineEffectInitialized;
        }

        public class AssembledActivatedTankNode : SonarEffectSystem.TankPartNode
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankComponent tank;
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public MapGroupComponent mapGroup;
            public BattleGroupComponent battleGroup;
        }

        public class BlinkerTankOutlineMapEffectNode : SonarEffectSystem.InitializedMapOutlineEffectNode
        {
            public TankOutlineMapEffectBlinkerStateComponent tankOutlineMapEffectBlinkerState;
        }

        public class DeactivationTankOutlineMapEffectNode : SonarEffectSystem.InitializedMapOutlineEffectNode
        {
            public TankOutlineMapEffectDeactivationStateComponent tankOutlineMapEffectDeactivationState;
        }

        public class DeadTankNode : SonarEffectSystem.AssembledActivatedInitializedOutlineTankNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class DMBattleNode : SonarEffectSystem.BattleNode
        {
            public DMComponent dm;
        }

        public class IdleTankOutlineMapEffectNode : SonarEffectSystem.InitializedMapOutlineEffectNode
        {
            public TankOutlineMapEffectIdleStateComponent tankOutlineMapEffectIdleState;
        }

        public class InitDeathMatchEvent : Event
        {
        }

        public class InitializedMapOutlineEffectNode : SonarEffectSystem.MapOutlineEffectNode
        {
            public TankOutlineMapEffectESMComponent tankOutlineMapEffectEsm;
            public TankOutlineMapEffectInitializedComponent tankOutlineMapEffectInitialized;
        }

        public class InitializedOutlineTankPartNode : SonarEffectSystem.TankPartNode
        {
            public TankPartOutlineEffectInitializedComponent tankPartOutlineEffectInitialized;
        }

        public class InitTeamMatchEvent : Event
        {
        }

        public class MapOutlineEffectNode : Node
        {
            public MapGroupComponent mapGroup;
            public TankOutlineMapEffectComponent tankOutlineMapEffect;
        }

        public class NameplateNode : Node
        {
            public NameplateComponent nameplate;
            public HealthBarComponent healthBar;
        }

        public class RemoteTankNode : SonarEffectSystem.AssembledActivatedTankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class RemoteTankTeamNode : SonarEffectSystem.RemoteTankNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class SelfBattleUserInTeamNode : SonarEffectSystem.SelfBattleUserNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserInBattleAsTankComponent userInBattleAsTank;
            public BattleGroupComponent battleGroup;
            public UserGroupComponent userGroup;
        }

        public class SelfSpectatorNode : Node
        {
            public SelfUserComponent selfUser;
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
            public TankVisualRootComponent tankVisualRoot;
            public UserGroupComponent userGroup;
        }

        public class SemiActiveTankNode : SonarEffectSystem.AssembledActivatedInitializedOutlineTankNode
        {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class SonarEffectNode : Node
        {
            public SonarEffectComponent sonarEffect;
            public TankGroupComponent tankGroup;
        }

        public class TankPartNode : Node
        {
            public BaseRendererComponent baseRenderer;
            public RendererInitializedComponent rendererInitialized;
            public RendererPaintedComponent rendererPainted;
            public TankPartOutlineEffectUnityComponent tankPartOutlineEffectUnity;
            public TankGroupComponent tankGroup;
        }

        public class TeamBattleNode : SonarEffectSystem.BattleNode
        {
            public TeamBattleComponent teamBattle;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
        }

        public class TransparencyTransitionTankNode : SonarEffectSystem.AssembledActivatedInitializedOutlineTankNode
        {
            public TransparencyTransitionComponent transparencyTransition;
        }

        public class WeaponNode : SonarEffectSystem.TankPartNode
        {
            public WeaponComponent weapon;
        }

        public class WorkingTankOutlineMapEffectNode : SonarEffectSystem.InitializedMapOutlineEffectNode
        {
            public TankOutlineMapEffectWorkingStateComponent tankOutlineMapEffectWorkingState;
        }
    }
}

