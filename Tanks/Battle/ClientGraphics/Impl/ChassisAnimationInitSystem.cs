namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class ChassisAnimationInitSystem : ECSSystem
    {
        private TrackController ConstructController(TrackBindingData data, Transform root)
        {
            TrackController controller = new TrackController();
            TrackPoint[] pointArray = new TrackPoint[data.trackPointsJoints.Length];
            for (int i = 0; i < data.trackPointsJoints.Length; i++)
            {
                Transform trackPoint = data.trackPointsJoints[i];
                Vector3 position = data.trackPointsPositions[i];
                Vector3 trackPointOffset = root.InverseTransformPoint(trackPoint.position) - position;
                pointArray[i] = new TrackPoint(position, trackPoint, trackPointOffset);
            }
            controller.trackPoints = pointArray;
            MovingWheel[] wheelArray = new MovingWheel[data.movingWheelsJoints.Length];
            for (int j = 0; j < data.movingWheelsJoints.Length; j++)
            {
                MovingWheel wheel;
                Transform transform2 = data.movingWheelsJoints[j];
                int index = data.movingWheelsNearestTrackPointsIndices[j];
                if (index < 0)
                {
                    wheel = new MovingWheel(transform2);
                }
                else
                {
                    Vector3 vector3 = data.trackPointsPositions[index];
                    wheel = new MovingWheel(transform2, pointArray[index], root.InverseTransformPoint(transform2.position) - vector3);
                }
                if (data.movingWheelsRadiuses != null)
                {
                    wheel.radius = data.movingWheelsRadiuses[j];
                }
                wheelArray[j] = wheel;
            }
            controller.movingWheels = wheelArray;
            Wheel[] wheelArray2 = new Wheel[data.rotatingWheelsJoints.Length];
            for (int k = 0; k < data.rotatingWheelsJoints.Length; k++)
            {
                Wheel wheel2 = new Wheel(data.rotatingWheelsJoints[k]);
                if (data.rotatingWheelsRadiuses != null)
                {
                    wheel2.radius = data.rotatingWheelsRadiuses[k];
                }
                wheelArray2[k] = wheel2;
            }
            controller.rotatingWheels = wheelArray2;
            if (data.trackPointsJoints.Length <= 1)
            {
                controller.segments = new TrackSegment[0];
            }
            else
            {
                TrackSegment[] segmentArray = new TrackSegment[data.trackPointsJoints.Length - 1];
                int index = 1;
                while (true)
                {
                    if (index >= data.trackPointsJoints.Length)
                    {
                        controller.segments = segmentArray;
                        break;
                    }
                    TrackPoint a = pointArray[index - 1];
                    TrackPoint b = pointArray[index];
                    Vector3 vector5 = data.trackPointsPositions[index - 1];
                    Vector3 vector6 = data.trackPointsPositions[index];
                    Vector3 vector7 = vector6 - vector5;
                    segmentArray[index - 1] = new TrackSegment(a, b, vector7.magnitude);
                    index++;
                }
            }
            controller.centerX = data.centerX;
            return controller;
        }

        [OnEventFire]
        public void OnInit(ChassisInitEvent e, ChassisAnimationInitNode node)
        {
            Transform root = node.tankVisualRoot.transform;
            ChassisAnimationComponent chassisAnimation = node.chassisAnimation;
            ChassisTrackControllerComponent component = new ChassisTrackControllerComponent {
                LeftTrack = this.ConstructController(chassisAnimation.leftTrackData, root),
                RightTrack = this.ConstructController(chassisAnimation.rightTrackData, root)
            };
            chassisAnimation.TracksMaterial = TankMaterialsUtil.GetTrackMaterial(node.trackRenderer.Renderer);
            node.Entity.AddComponent(component);
        }

        public class ChassisAnimationInitNode : Node
        {
            public TankVisualRootComponent tankVisualRoot;
            public ChassisAnimationComponent chassisAnimation;
            public TrackRendererComponent trackRenderer;
        }
    }
}

