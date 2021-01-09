namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class BonusSpawnSystem : ECSSystem
    {
        [OnEventFire]
        public void BonusOnGroundAnimation(UpdateEvent e, BonusBoxSpawnOnGroundLocalDurationNode bonus)
        {
            float num2 = 0.5f + (Date.Now.GetProgress(bonus.localDuration.StartedTime, bonus.localDuration.Duration) * 0.5f);
            bonus.bonusBoxInstance.BonusBoxInstance.transform.localScale = Vector3.one * num2;
        }

        [OnEventFire]
        public void RemoveOnGroundState(LocalDurationExpireEvent e, BonusBoxSpawnOnGroundNode bonus)
        {
            bonus.bonusBoxInstance.BonusBoxInstance.transform.localScale = Vector3.one;
            bonus.Entity.RemoveComponent<BonusSpawnOnGroundStateComponent>();
        }

        [OnEventComplete]
        public void SetActiveState(LocalDurationExpireEvent e, BonusBoxSpawnNode bonus)
        {
            bonus.Entity.RemoveComponent<BonusSpawnStateComponent>();
            bonus.Entity.AddComponent<BonusActiveStateComponent>();
        }

        [OnEventFire]
        public void SetBonusPosition(SetBonusPositionEvent e, BonusBoxBaseNode bonus)
        {
            bonus.bonusBoxInstance.BonusBoxInstance.transform.position = bonus.position.Position;
            bonus.bonusBoxInstance.BonusBoxInstance.transform.rotation = Quaternion.Euler(bonus.rotation.RotationEuler);
        }

        [OnEventFire]
        public void SetBonusPositionOnSpawn(NodeAddedEvent e, BonusBoxSpawnNode bonus)
        {
            float progress = Date.Now.GetProgress(bonus.bonusDropTime.DropTime, bonus.bonusConfig.SpawnDuration);
            bonus.Entity.AddComponent(new LocalDurationComponent(bonus.bonusConfig.SpawnDuration * (1f - progress)));
            base.ScheduleEvent<SetBonusPositionEvent>(bonus);
        }

        [OnEventFire]
        public void SetFallingState(NodeAddedEvent e, BonusParachuteSpawnNode bonus)
        {
            bonus.Entity.AddComponent<BonusFallingStateComponent>();
        }

        public class BonusBoxBaseNode : Node
        {
            public BonusConfigComponent bonusConfig;
            public BonusComponent bonus;
            public BonusDropTimeComponent bonusDropTime;
            public PositionComponent position;
            public RotationComponent rotation;
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BattleGroupComponent battleGroup;
        }

        public class BonusBoxSpawnNode : BonusSpawnSystem.BonusBoxBaseNode
        {
            public BonusSpawnStateComponent bonusSpawnState;
        }

        public class BonusBoxSpawnOnGroundLocalDurationNode : BonusSpawnSystem.BonusBoxSpawnOnGroundNode
        {
            public LocalDurationComponent localDuration;
        }

        public class BonusBoxSpawnOnGroundNode : BonusSpawnSystem.BonusBoxSpawnNode
        {
            public BonusSpawnOnGroundStateComponent bonusSpawnOnGroundState;
        }

        public class BonusParachuteSpawnNode : BonusSpawnSystem.BonusBoxSpawnNode
        {
            public BonusParachuteInstanceComponent bonusParachuteInstance;
            public LocalDurationComponent localDuration;
        }
    }
}

