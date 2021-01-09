namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class IsisGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateEffect(NodeAddedEvent e, IsisRayEffectInitNode node)
        {
            GameObject gameObject = Object.Instantiate<GameObject>(node.isisGraphics.RayPrefab);
            CustomRenderQueue.SetQueue(gameObject, 0xc4e);
            UnityUtil.InheritAndEmplace(gameObject.transform, node.muzzlePoint.Current);
            node.isisGraphics.Ray = gameObject.GetComponent<IsisRayEffectBehaviour>();
            node.isisGraphics.Ray.Init();
            node.Entity.AddComponent<IsisGraphicsReadyComponent>();
        }

        [OnEventFire]
        public void DisableTarget(NodeRemoveEvent e, DisableEffectNode node)
        {
            node.Entity.RemoveComponentIfPresent<IsisGraphicsDamagingStateComponent>();
            node.isisGraphics.Ray.DisableTarget();
        }

        [OnEventFire]
        public void DisableTarget(NodeRemoveEvent e, TankActiveStateNode activeTank, [JoinByTank] DisableEffectNode node)
        {
            node.Entity.RemoveComponentIfPresent<IsisGraphicsDamagingStateComponent>();
            node.isisGraphics.Ray.DisableTarget();
        }

        [OnEventFire]
        public void EnableTarget(NodeAddedEvent e, TargetEffectNode node, [Context, JoinByBattle] SingleNode<DMComponent> dm)
        {
            node.Entity.AddComponentIfAbsent<IsisGraphicsDamagingStateComponent>();
            node.isisGraphics.Ray.EnableTargetForDamaging();
            UpdateRayEffectUpdateEvent eventInstance = new UpdateRayEffectUpdateEvent();
            eventInstance.speedMultipliers = new float[] { float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity };
            base.NewEvent(eventInstance).Attach(node.streamHit.TankHit.Entity).Attach(node).Schedule();
        }

        [OnEventFire]
        public void EnableTarget(NodeAddedEvent e, [Combine] TargetEffectNode node, [Context, JoinByTeam] TeamNode team)
        {
            StreamHitComponent streamHit = node.streamHit;
            base.NewEvent<UpdateIsisRayModeEvent>().Attach(team).Attach(streamHit.TankHit.Entity).Attach(node).Schedule();
            UpdateRayEffectUpdateEvent eventInstance = new UpdateRayEffectUpdateEvent();
            eventInstance.speedMultipliers = new float[] { float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity };
            base.NewEvent(eventInstance).Attach(node).Attach(streamHit.TankHit.Entity).Schedule();
        }

        [OnEventFire]
        public void HideEffect(NodeRemoveEvent e, WorkingEffectNode node)
        {
            node.isisGraphics.Ray.Hide();
            node.Entity.RemoveComponentIfPresent<IsisGraphicsDamagingStateComponent>();
            node.Entity.RemoveComponent<IsisRayEffectShownComponent>();
        }

        [OnEventComplete]
        public void ResendUpdateWithTarget(UpdateEvent e, TargetEffectNode node)
        {
            UpdateRayEffectUpdateEvent eventInstance = new UpdateRayEffectUpdateEvent();
            eventInstance.speedMultipliers = new float[] { 1f, 2f, 1f };
            float[] singleArray2 = new float[3];
            singleArray2[1] = 4f;
            singleArray2[2] = 1f;
            eventInstance.bezierPointsRandomness = singleArray2;
            base.NewEvent(eventInstance).Attach(node).Attach(node.streamHit.TankHit.Entity).Schedule();
        }

        [OnEventFire]
        public void ShowEffect(NodeAddedEvent e, WorkingEffectNode node)
        {
            node.isisGraphics.Ray.Show();
            node.Entity.AddComponent<IsisRayEffectShownComponent>();
        }

        [OnEventFire]
        public void UpdateEffectWithTarget(UpdateRayEffectUpdateEvent e, TargetEffectNode node, SingleNode<TankVisualRootComponent> targetTank, [JoinAll] CameraNode cameraNode)
        {
            node.isisGraphics.Ray.UpdateTargetPosition(targetTank.component.transform, node.streamHit.TankHit.LocalHitPoint, e.speedMultipliers, e.bezierPointsRandomness);
        }

        [OnEventFire]
        public void UpdateIsisRayMode(UpdateIsisRayModeEvent evt, SingleNode<TeamComponent> weaponTeam, SingleNode<IsisGraphicsComponent> effectNode, TankNode tank)
        {
            if (weaponTeam.Entity.Id == tank.teamGroup.Key)
            {
                effectNode.Entity.RemoveComponentIfPresent<IsisGraphicsDamagingStateComponent>();
                effectNode.component.Ray.EnableTargetForHealing();
            }
            else
            {
                effectNode.Entity.AddComponentIfAbsent<IsisGraphicsDamagingStateComponent>();
                effectNode.component.Ray.EnableTargetForDamaging();
            }
        }

        public class CameraNode : Node
        {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
        }

        public class DisableEffectNode : IsisGraphicsSystem.WorkingEffectNode
        {
            public StreamHitComponent streamHit;
        }

        public class IsisRayEffectInitNode : Node
        {
            public IsisGraphicsComponent isisGraphics;
            public MuzzlePointComponent muzzlePoint;
        }

        public class TankActiveStateNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TeamGroupComponent teamGroup;
            public TankComponent tank;
        }

        public class TargetEffectNode : IsisGraphicsSystem.DisableEffectNode
        {
            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
            public IsisRayEffectShownComponent isisRayEffectShown;
        }

        public class TeamNode : Node
        {
            public TeamGroupComponent teamGroup;
            public TeamComponent team;
        }

        public class WorkingEffectNode : IsisGraphicsSystem.IsisRayEffectInitNode
        {
            public IsisGraphicsReadyComponent isisGraphicsReady;
            public StreamWeaponWorkingComponent streamWeaponWorking;
        }
    }
}

