namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class BonusAlignmentToGroundSystem : ECSSystem
    {
        [OnEventFire]
        public void BoxAlignmentToGround(NodeRemoveEvent e, BonusBoxAlignmentToGroundNode bonus)
        {
            GameObject bonusBoxInstance = bonus.bonusBoxInstance.BonusBoxInstance;
            if (bonusBoxInstance)
            {
                BonusDataComponent bonusData = bonus.bonusData;
                Transform transform = bonusBoxInstance.transform;
                float angle = Vector3.Angle(bonusData.GroundPointNormal, transform.TransformDirection(Vector3.up));
                transform.RotateAround(bonusData.LandingPoint, bonusData.LandingAxis, angle);
                Vector3 eulerAngles = transform.eulerAngles;
                eulerAngles.y = bonus.rotation.RotationEuler.y + (bonusData.FallDuration * bonus.bonusConfig.AngularSpeed);
                transform.eulerAngles = eulerAngles;
            }
        }

        [OnEventFire]
        public void BoxAlignmentToGround(UpdateEvent e, BonusBoxAlignmentToGroundNode bonus)
        {
            BonusDataComponent bonusData = bonus.bonusData;
            Transform transform = bonus.bonusBoxInstance.BonusBoxInstance.transform;
            float angle = (bonus.bonusConfig.AlignmentToGroundAngularSpeed * ((Date.Now - bonus.bonusDropTime.DropTime) - bonus.bonusData.FallDuration)) - Vector3.Angle(transform.TransformDirection(Vector3.up), Vector3.up);
            transform.RotateAround(bonusData.LandingPoint, bonusData.LandingAxis, angle);
            if (Vector3.Angle(bonus.bonusData.GroundPointNormal, transform.TransformDirection(Vector3.up)) <= angle)
            {
                bonus.Entity.RemoveComponent<BonusAlignmentToGroundStateComponent>();
            }
        }

        [OnEventFire]
        public void PrepareAlignmentToGround(NodeAddedEvent e, BonusAlignmentToGroundParachuteNode bonus, [JoinAll] SingleNode<BonusClientConfigComponent> bonusConfig)
        {
            bonus.Entity.AddComponent<BonusParachuteFoldingStateComponent>();
            if (Date.Now.GetProgress(bonus.bonusDropTime.DropTime + bonus.bonusData.FallDuration, bonus.bonusData.AlignmentToGroundDuration).Equals((float) 1f))
            {
                bonus.Entity.RemoveComponent<BonusAlignmentToGroundStateComponent>();
            }
            else if (bonus.bonusData.GroundPointNormal == Vector3.up)
            {
                bonus.Entity.RemoveComponent<BonusAlignmentToGroundStateComponent>();
            }
        }

        public class BonusAlignmentToGroundParachuteNode : BonusAlignmentToGroundSystem.BonusBoxAlignmentToGroundNode
        {
            public BonusParachuteInstanceComponent bonusParachuteInstance;
            public TopParachuteMarkerComponent topParachuteMarker;
        }

        public class BonusBoxAlignmentToGroundNode : BonusAlignmentToGroundSystem.BonusBoxBaseNode
        {
            public BonusAlignmentToGroundStateComponent bonusAlignmentToGroundState;
        }

        public class BonusBoxBaseNode : Node
        {
            public BonusConfigComponent bonusConfig;
            public BonusComponent bonus;
            public BonusDropTimeComponent bonusDropTime;
            public PositionComponent position;
            public RotationComponent rotation;
            public BonusDataComponent bonusData;
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BattleGroupComponent battleGroup;
        }
    }
}

