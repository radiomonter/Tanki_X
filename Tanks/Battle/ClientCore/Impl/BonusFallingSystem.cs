namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class BonusFallingSystem : ECSSystem
    {
        [OnEventFire]
        public void PrepareFalling(NodeAddedEvent e, BonusFallingNode bonus)
        {
            this.UpdateBonusPosition(bonus);
        }

        private void ToAlignmentToGroundState(Entity entity)
        {
            entity.RemoveComponent<BonusFallingStateComponent>();
            entity.AddComponent<BonusAlignmentToGroundStateComponent>();
            entity.AddComponent<BonusGroundedStateComponent>();
        }

        private void UpdateBonusPosition(BonusFallingNode bonus)
        {
            GameObject bonusBoxInstance = bonus.bonusBoxInstance.BonusBoxInstance;
            if (bonusBoxInstance)
            {
                BonusConfigComponent bonusConfig = bonus.bonusConfig;
                BonusDataComponent bonusData = bonus.bonusData;
                float num = Mathf.Clamp((float) (Date.Now - bonus.bonusDropTime.DropTime), 0f, bonusData.FallDuration);
                Vector3 euler = new Vector3(bonusConfig.SwingAngle * Mathf.Sin(bonusConfig.SwingFreq * (bonusData.FallDuration - num)), bonus.rotation.RotationEuler.y + (bonusConfig.AngularSpeed * num), bonusBoxInstance.transform.eulerAngles.z);
                bonusBoxInstance.transform.rotation = Quaternion.Euler(euler);
                Vector3 vector4 = MathUtil.SetRotationMatrix(bonusBoxInstance.transform.eulerAngles * 0.01745329f).MultiplyPoint3x4(Vector3.down);
                bonusBoxInstance.transform.position = new Vector3(bonus.position.Position.x + (bonusData.SwingPivotY * vector4.x), bonus.position.Position.y - (bonusConfig.FallSpeed * num), bonus.position.Position.z + (bonusData.SwingPivotY * vector4.z));
                if (num.Equals(bonusData.FallDuration))
                {
                    this.ToAlignmentToGroundState(bonus.Entity);
                }
            }
        }

        [OnEventComplete]
        public void UpdateBonusPosition(UpdateEvent e, BonusFallingNode bonus)
        {
            this.UpdateBonusPosition(bonus);
        }

        public class BonusFallingNode : Node
        {
            public BonusComponent bonus;
            public BonusDropTimeComponent bonusDropTime;
            public BonusFallingStateComponent bonusFallingState;
            public BonusDataComponent bonusData;
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BonusParachuteInstanceComponent bonusParachuteInstance;
            public BonusConfigComponent bonusConfig;
            public PositionComponent position;
            public RotationComponent rotation;
        }
    }
}

