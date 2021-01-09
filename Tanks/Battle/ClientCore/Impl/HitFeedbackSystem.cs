namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;

    public class HitFeedbackSystem : ECSSystem
    {
        [OnEventFire]
        public void AddStreamWeaponHitFeedback(SelfHitEvent e, StreamWeaponWorkingFeedbackControllerNode weapon, [JoinByTank] SelfTankNode tank, [JoinByBattle] BattleNode battle)
        {
            if (this.ValidateSelfHit(e, tank, battle))
            {
                weapon.Entity.AddComponentIfAbsent<StreamHitEnemyFeedbackComponent>();
            }
            else
            {
                weapon.Entity.RemoveComponentIfPresent<StreamHitEnemyFeedbackComponent>();
            }
        }

        [OnEventFire]
        public void BeginStreamWeaponHitFeedback(NodeAddedEvent e, StreamHitNode weapon, [Context, JoinByTank] SelfTankNode tank, [Context, JoinByBattle] DMBattleNode battle)
        {
            weapon.Entity.AddComponentIfAbsent<StreamHitEnemyFeedbackComponent>();
        }

        [OnEventFire]
        public void BeginStreamWeaponHitFeedback(NodeAddedEvent e, StreamHitNode weapon, [Context, JoinByTank] SelfTankTeamNode tank, [Context, JoinByBattle] TeamBattleNode battle)
        {
            this.UpdateStreamWeaponHitFeedback(weapon, tank);
        }

        [OnEventFire]
        public void FinishStreamWeaponHitFeedback(NodeRemoveEvent e, StreamHitNode node, [JoinSelf] SingleNode<StreamHitEnemyFeedbackComponent> weapon)
        {
            weapon.Entity.RemoveComponent<StreamHitEnemyFeedbackComponent>();
        }

        [OnEventFire]
        public void FinishStreamWeaponHitFeedback(NodeRemoveEvent e, StreamHitNode node, [JoinSelf] SingleNode<StreamHitTeammateFeedbackComponent> weapon)
        {
            weapon.Entity.RemoveComponent<StreamHitTeammateFeedbackComponent>();
        }

        [OnEventFire]
        public void RemoveStreamWeaponHitFeedback(NodeAddedEvent e, SingleNode<StreamHitEnemyFeedbackComponent> weapon, [Context, JoinSelf] StreamWeaponIdleFeedbackControllerNode idle)
        {
            weapon.Entity.RemoveComponent<StreamHitEnemyFeedbackComponent>();
        }

        [OnEventFire]
        public void RemoveStreamWeaponHitFeedback(SelfHitSkipEvent e, SingleNode<StreamHitEnemyFeedbackComponent> weapon, [JoinSelf] StreamWeaponWorkingFeedbackControllerNode streamWeapon)
        {
            weapon.Entity.RemoveComponent<StreamHitEnemyFeedbackComponent>();
        }

        [OnEventFire]
        public void ScheduleHitFeedbackOnSelfHitEvent(SelfHitEvent e, DiscreteWeaponNode weapon, [JoinByTank] SelfTankNode tank, [JoinByBattle] BattleNode battle)
        {
            if (this.ValidateSelfHit(e, tank, battle))
            {
                base.ScheduleEvent<HitFeedbackEvent>(tank);
            }
        }

        [OnEventFire]
        public void ScheduleHitFeedbackOnSelfSplashHitEvent(SelfSplashHitEvent e, SplashWeaponNode weapon, [JoinByTank] SelfTankNode tank, [JoinByBattle] BattleNode battle)
        {
            if (this.ValidateSelfHit(e, tank, battle))
            {
                base.ScheduleEvent<HitFeedbackEvent>(tank);
            }
            else if (((e.SplashTargets != null) && (e.SplashTargets.Count != 0)) && this.ValidateTargets(e.SplashTargets, tank, battle))
            {
                base.ScheduleEvent<HitFeedbackEvent>(tank);
            }
        }

        private void UpdateStreamWeaponHitFeedback(StreamHitNode weapon, SelfTankTeamNode tank)
        {
            if (weapon.streamHit.TankHit == null)
            {
                weapon.Entity.RemoveComponentIfPresent<StreamHitEnemyFeedbackComponent>();
                weapon.Entity.RemoveComponentIfPresent<StreamHitTeammateFeedbackComponent>();
            }
            else if (tank.teamGroup.Key == weapon.streamHit.TankHit.Entity.GetComponent<TeamGroupComponent>().Key)
            {
                weapon.Entity.AddComponentIfAbsent<StreamHitTeammateFeedbackComponent>();
                weapon.Entity.RemoveComponentIfPresent<StreamHitEnemyFeedbackComponent>();
            }
            else
            {
                weapon.Entity.AddComponentIfAbsent<StreamHitEnemyFeedbackComponent>();
                weapon.Entity.RemoveComponentIfPresent<StreamHitTeammateFeedbackComponent>();
            }
        }

        private bool ValidateSelfHit(SelfHitEvent e, SelfTankNode tank, BattleNode battle) => 
            (e.Targets != null) ? ((e.Targets.Count != 0) ? this.ValidateTargets(e.Targets, tank, battle) : false) : false;

        private bool ValidateTarget(Entity targetEntity, SelfTankNode tank, BattleNode battle) => 
            !targetEntity.Equals(tank.Entity) ? (targetEntity.HasComponent<TankActiveStateComponent>() ? (!battle.Entity.HasComponent<TeamBattleComponent>() || (targetEntity.GetComponent<TeamGroupComponent>().Key != tank.Entity.GetComponent<TeamGroupComponent>().Key)) : false) : false;

        private bool ValidateTargets(List<HitTarget> targets, SelfTankNode tank, BattleNode battle)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                HitTarget target = targets[i];
                if (this.ValidateTarget(target.Entity, tank, battle))
                {
                    return true;
                }
            }
            return false;
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
        }

        [Not(typeof(SplashWeaponComponent))]
        public class DiscreteWeaponNode : HitFeedbackSystem.WeaponNode
        {
            public DiscreteWeaponComponent discreteWeapon;
        }

        public class DMBattleNode : HitFeedbackSystem.BattleNode
        {
            public DMComponent dm;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
            public BattleGroupComponent battleGroup;
        }

        public class SelfTankTeamNode : HitFeedbackSystem.SelfTankNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class SplashWeaponNode : HitFeedbackSystem.WeaponNode
        {
            public DiscreteWeaponComponent discreteWeapon;
            public SplashWeaponComponent splashWeapon;
        }

        public class StreamHitNode : HitFeedbackSystem.WeaponNode
        {
            public StreamHitComponent streamHit;
            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
        }

        public class StreamWeaponFeedbackControllerNode : HitFeedbackSystem.WeaponNode
        {
            public StreamWeaponComponent streamWeapon;
            public StreamWeaponSimpleFeedbackControllerComponent streamWeaponSimpleFeedbackController;
        }

        public class StreamWeaponIdleFeedbackControllerNode : HitFeedbackSystem.StreamWeaponFeedbackControllerNode
        {
            public StreamWeaponIdleComponent streamWeaponIdle;
        }

        public class StreamWeaponWorkingFeedbackControllerNode : HitFeedbackSystem.StreamWeaponFeedbackControllerNode
        {
            public StreamWeaponWorkingComponent streamWeaponWorking;
        }

        public class TeamBattleNode : HitFeedbackSystem.BattleNode
        {
            public TeamBattleComponent teamBattle;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public TankGroupComponent tankGroup;
        }
    }
}

