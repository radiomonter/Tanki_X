namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class NameplateOpacitySystem : ECSSystem
    {
        private void ChangeAlpha(NameplateComponent nameplate, float deltaAlpha)
        {
            nameplate.Alpha = Mathf.Clamp01(nameplate.Alpha + deltaAlpha);
        }

        private void DecreaseAlpha(NameplateComponent nameplateComponent, float dt)
        {
            float deltaAlpha = -nameplateComponent.disappearanceSpeed * dt;
            this.ChangeAlpha(nameplateComponent, deltaAlpha);
        }

        [OnEventFire]
        public void DeleteNameplate(TimeUpdateEvent e, NameplateDeletionNode nameplate)
        {
            NameplateComponent nameplateComponent = nameplate.nameplate;
            this.DecreaseAlpha(nameplateComponent, e.DeltaTime);
            if (nameplateComponent.Alpha <= 0f)
            {
                Object.Destroy(nameplateComponent.gameObject);
            }
        }

        [OnEventFire]
        public void DeleteNameplateOnReincarnation(NodeRemoveEvent e, TankIncarnationNode tank, [JoinByTank] SingleNode<NameplateComponent> nameplate)
        {
            Object.Destroy(nameplate.component.gameObject);
        }

        [OnEventFire]
        public void HideNameplate(TimeUpdateEvent e, NameplateConclealmentNode nameplate)
        {
            NameplateComponent nameplateComponent = nameplate.nameplate;
            if (!nameplate.nameplate.alwaysVisible && (nameplateComponent.Alpha > 0f))
            {
                this.DecreaseAlpha(nameplateComponent, e.DeltaTime);
            }
        }

        private void IncreaseAlpha(NameplateComponent nameplateComponent, float dt)
        {
            float deltaAlpha = nameplateComponent.appearanceSpeed * dt;
            if (nameplateComponent.Alpha < 1f)
            {
                this.ChangeAlpha(nameplateComponent, deltaAlpha);
            }
        }

        [OnEventFire]
        public void RevealNameplate(TimeUpdateEvent e, NameplateAppearanceNode nameplate)
        {
            NameplateComponent nameplateComponent = nameplate.nameplate;
            this.IncreaseAlpha(nameplateComponent, e.DeltaTime);
        }

        [OnEventFire]
        public void StopOpacityChange(NodeAddedEvent e, NameplateDeletionNode nameplate)
        {
            nameplate.Entity.RemoveComponent<NameplateOpacityComponent>();
        }

        private void SwitchToAppearanceByDistance(NameplateNode nameplate)
        {
            if (nameplate.nameplatePosition.sqrDistance < nameplate.nameplateOpacity.sqrConcealmentDistance)
            {
                nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateAppearanceState>();
            }
        }

        [OnEventFire]
        public void ToAppearanceState(NodeAddedEvent e, NameplateNode nameplate)
        {
            nameplate.nameplate.Alpha = 0f;
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateAppearanceState>();
        }

        [OnEventFire]
        public void ToAppearanceState(TimeUpdateEvent e, NameplateConclealmentNotInvisibilityNode nameplate)
        {
            this.SwitchToAppearanceByDistance(nameplate);
        }

        [OnEventFire]
        public void ToAppearanceState(NodeAddedEvent e, NameplateNode nameplate, [Context, JoinByTank] TankInvisibilityEffectDeactivationStateNode tank)
        {
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateAppearanceState>();
        }

        [OnEventFire]
        public void ToAppearanceState(NodeAddedEvent e, NameplateNode nameplate, [Context, JoinByTank] TankInvisibilityEffectIdleStateNode tank)
        {
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateAppearanceState>();
        }

        [OnEventFire]
        public void ToConcealmentState(TimeUpdateEvent e, NameplateAppearanceNode nameplate)
        {
            if (!nameplate.nameplate.alwaysVisible && (nameplate.nameplatePosition.sqrDistance > nameplate.nameplateOpacity.sqrConcealmentDistance))
            {
                nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateConcealmentState>();
            }
        }

        [OnEventFire]
        public void ToInvisibilityEffectState(NodeAddedEvent e, DMBattleNode dm, [Context, JoinByBattle] SelfBattleUserNode selfBattleUser, [Combine] NameplateNode nameplate, [Context, JoinByTank, Combine] TankInvisibilityEffectActivationStateNode tank)
        {
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateInvisibilityEffectState>();
        }

        [OnEventFire]
        public void ToInvisibilityEffectState(NodeAddedEvent e, DMBattleNode dm, [Context, JoinByBattle] SelfBattleUserNode selfBattleUser, [Combine] NameplateNode nameplate, [Context, JoinByTank, Combine] TankInvisibilityEffectWorkingStateNode tank)
        {
            nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateInvisibilityEffectState>();
        }

        [OnEventFire]
        public void ToInvisibilityEffectState(NodeAddedEvent e, TeamBattleNode teamBattle, [Context, JoinByBattle] SelfBattleUserInTeamModeNode selfBattleUser, [Context, JoinByTeam] TeamNode selfTeam, [Combine] NameplateNode nameplate, [Context, JoinByTank, Combine] TankInTeamInvisibilityEffectActivationStateNode tank, [Context, JoinByTeam, Combine] TeamNode tankTeam)
        {
            if (!selfTeam.Entity.Equals(tankTeam.Entity))
            {
                nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateInvisibilityEffectState>();
            }
        }

        [OnEventFire]
        public void ToInvisibilityEffectState(NodeAddedEvent e, TeamBattleNode teamBattle, [Context, JoinByBattle] SelfBattleUserInTeamModeNode selfBattleUser, [Context, JoinByTeam] TeamNode selfTeam, [Combine] NameplateNode nameplate, [Context, JoinByTank, Combine] TankInTeamInvisibilityEffectWorkingStateNode tank, [Context, JoinByTeam, Combine] TeamNode tankTeam)
        {
            if (!selfTeam.Entity.Equals(tankTeam.Entity))
            {
                nameplate.nameplateEsm.esm.ChangeState<NameplateStates.NameplateInvisibilityEffectState>();
            }
        }

        public class DMBattleNode : Node
        {
            public BattleGroupComponent battleGroup;
            public DMComponent dm;
        }

        public class NameplateAppearanceNode : NameplateOpacitySystem.NameplateNode
        {
            public NameplateAppearanceStateComponent nameplateAppearanceState;
        }

        public class NameplateConclealmentNode : NameplateOpacitySystem.NameplateNode
        {
            public NameplateConcealmentStateComponent nameplateConcealmentState;
        }

        [Not(typeof(NameplateInvisibilityEffectStateComponent))]
        public class NameplateConclealmentNotInvisibilityNode : NameplateOpacitySystem.NameplateConclealmentNode
        {
        }

        public class NameplateDeletionNode : Node
        {
            public NameplateDeletionStateComponent nameplateDeletionState;
            public NameplateComponent nameplate;
        }

        public class NameplateNode : Node
        {
            public NameplateComponent nameplate;
            public NameplatePositionComponent nameplatePosition;
            public NameplateOpacityComponent nameplateOpacity;
            public NameplateESMComponent nameplateEsm;
            public TankGroupComponent tankGroup;
        }

        public class SelfBattleUserInTeamModeNode : NameplateOpacitySystem.SelfBattleUserNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserInBattleAsTankComponent userInBattleAsTank;
            public BattleGroupComponent battleGroup;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankInTeamInvisibilityEffectActivationStateNode : NameplateOpacitySystem.TankInvisibilityEffectActivationStateNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class TankInTeamInvisibilityEffectWorkingStateNode : NameplateOpacitySystem.TankInvisibilityEffectWorkingStateNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class TankInvisibilityEffectActivationStateNode : NameplateOpacitySystem.TankNode
        {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class TankInvisibilityEffectDeactivationStateNode : NameplateOpacitySystem.TankNode
        {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }

        public class TankInvisibilityEffectIdleStateNode : NameplateOpacitySystem.TankNode
        {
            public TankInvisibilityEffectIdleStateComponent tankInvisibilityEffectIdleState;
        }

        public class TankInvisibilityEffectWorkingStateNode : NameplateOpacitySystem.TankNode
        {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }

        public class TankNode : Node
        {
            public RemoteTankComponent remoteTank;
            public TankComponent tank;
            public TankGroupComponent tankGroup;
        }

        public class TeamBattleNode : Node
        {
            public BattleGroupComponent battleGroup;
            public TeamBattleComponent teamBattle;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
        }
    }
}

