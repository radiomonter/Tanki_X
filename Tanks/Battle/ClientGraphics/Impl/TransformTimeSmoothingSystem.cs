namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class TransformTimeSmoothingSystem : ECSSystem
    {
        private const float LOW_FPS_FRAME_TIME = 0.03333334f;
        private const float SMOOTH_SPEED = 1.2f;
        private const float MIN_LERP_FACTOR = 0.4f;
        private const float MAX_LERP_FACTOR = 1.2f;
        public static readonly float ROTATION_CORRECTION_LERP_COEFF = 0.95f;
        private int lastFrame;
        private float frameLeaderDeltaAngle;
        private float frameLeaderDeltaPosition;

        [OnEventFire]
        public void Destroy(NodeRemoveEvent e, SingleNode<TransformTimeSmoothingComponent> node)
        {
            node.Entity.RemoveComponent<TransformTimeSmoothingDataComponent>();
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<TransformTimeSmoothingComponent> node)
        {
            Transform transform = node.component.Transform;
            TransformTimeSmoothingDataComponent component = new TransformTimeSmoothingDataComponent {
                LastPosition = transform.position,
                LastRotation = transform.rotation,
                LerpFactor = 1f
            };
            node.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void TransformSmoothCalculation(TransformTimeSmoothingEvent e, TimeSmoothingNode node)
        {
            bool flag = node.transformTimeSmoothing.UseCorrectionByFrameLeader && (Time.frameCount > this.lastFrame);
            Transform transform = node.transformTimeSmoothing.Transform;
            TransformTimeSmoothingDataComponent transformTimeSmoothingData = node.transformTimeSmoothingData;
            float smoothDeltaTime = Time.smoothDeltaTime;
            float num2 = Mathf.Clamp((float) (smoothDeltaTime / Time.deltaTime), (float) 0.4f, (float) 1f);
            if (num2 < transformTimeSmoothingData.LerpFactor)
            {
                transformTimeSmoothingData.LerpFactor = num2;
            }
            else
            {
                float num3 = (1.2f * smoothDeltaTime) * (1f - Mathf.Sqrt(transformTimeSmoothingData.LerpFactor));
                transformTimeSmoothingData.LerpFactor = Mathf.Clamp((float) (transformTimeSmoothingData.LerpFactor + num3), (float) 0.4f, (float) 1f);
            }
            Vector3 v = Vector3.SlerpUnclamped(transformTimeSmoothingData.LastPosition, transform.position, transformTimeSmoothingData.LerpFactor);
            Quaternion rot = Quaternion.SlerpUnclamped(transformTimeSmoothingData.LastRotation, transform.rotation, transformTimeSmoothingData.LerpFactor);
            if (PhysicsUtil.IsValidVector3(v) && PhysicsUtil.IsValidQuaternion(rot))
            {
                transform.SetPositionSafe(v);
                transform.SetRotationSafe(rot);
            }
            transformTimeSmoothingData.LastRotationDeltaAngle = Quaternion.Angle(transformTimeSmoothingData.LastRotation, transform.rotation);
            this.frameLeaderDeltaPosition = (transformTimeSmoothingData.LastPosition - transform.position).magnitude;
            float num4 = 1f;
            float num5 = 1f;
            if (node.transformTimeSmoothing.UseCorrectionByFrameLeader && !flag)
            {
                float num6 = 0.1f;
                if ((this.frameLeaderDeltaAngle > num6) && (transformTimeSmoothingData.LastRotationDeltaAngle > num6))
                {
                    num4 = Mathf.Abs((float) (((transformTimeSmoothingData.LastRotationDeltaAngle * (1f - transformTimeSmoothingData.LerpFactor)) + (this.frameLeaderDeltaAngle * transformTimeSmoothingData.LerpFactor)) / this.frameLeaderDeltaAngle));
                }
                if (this.frameLeaderDeltaPosition > num6)
                {
                    num5 = Mathf.Abs((float) (((this.frameLeaderDeltaPosition * (1f - transformTimeSmoothingData.LerpFactor)) + (this.frameLeaderDeltaPosition * transformTimeSmoothingData.LerpFactor)) / this.frameLeaderDeltaPosition));
                }
            }
            float t = Mathf.Clamp((float) (transformTimeSmoothingData.LerpFactor * num5), (float) 0.4f, (float) 1.2f);
            float num8 = Mathf.Clamp((float) (transformTimeSmoothingData.LerpFactor * num4), (float) 0.4f, (float) 1.2f);
            v = Vector3.SlerpUnclamped(transformTimeSmoothingData.LastPosition, transform.position, t);
            rot = Quaternion.SlerpUnclamped(transformTimeSmoothingData.LastRotation, transform.rotation, num8);
            if (PhysicsUtil.IsValidVector3(v) && PhysicsUtil.IsValidQuaternion(rot))
            {
                transform.SetPositionSafe(v);
                transform.SetRotationSafe(rot);
            }
            transformTimeSmoothingData.LastRotationDeltaAngle = Quaternion.Angle(transformTimeSmoothingData.LastRotation, transform.rotation);
            if (flag)
            {
                this.lastFrame = Time.frameCount;
                this.frameLeaderDeltaAngle = transformTimeSmoothingData.LastRotationDeltaAngle;
                this.frameLeaderDeltaPosition = (transformTimeSmoothingData.LastPosition - transform.position).magnitude;
            }
            transformTimeSmoothingData.LastPosition = transform.position;
            transformTimeSmoothingData.LastRotation = transform.rotation;
        }

        public class TimeSmoothingNode : Node
        {
            public TransformTimeSmoothingComponent transformTimeSmoothing;
            public TransformTimeSmoothingDataComponent transformTimeSmoothingData;
        }
    }
}

