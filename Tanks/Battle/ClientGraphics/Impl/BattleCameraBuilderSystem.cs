namespace Tanks.Battle.ClientGraphics.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class BattleCameraBuilderSystem : ECSSystem
    {
        private const string MAP_CONFIG_PATH = "camera";

        [OnEventFire]
        public void AddCameraTarget(NodeAddedEvent e, WeaponNode weapon, [Context, JoinByTank] SelfTankReadyForCameraNode tank)
        {
            if (!weapon.Entity.HasComponent<CameraTargetComponent>())
            {
                CameraTargetComponent component = new CameraTargetComponent {
                    TargetObject = weapon.weaponInstance.WeaponInstance.gameObject
                };
                weapon.Entity.AddComponent(component);
            }
        }

        [OnEventFire]
        public void Clean(NodeRemoveEvent e, TankIncarnationNode tankIncarnation, [JoinByTank] SingleNode<FollowedTankReadyToCameraComponent> tank)
        {
            tank.Entity.RemoveComponent<FollowedTankReadyToCameraComponent>();
        }

        [OnEventFire]
        public void Clean(NodeRemoveEvent e, TankIncarnationNode tankIncarnation, [JoinByTank] SingleNode<SelfTankReadyForCameraComponent> tank)
        {
            tank.Entity.RemoveComponent<SelfTankReadyForCameraComponent>();
        }

        [OnEventFire]
        public void CreateBattleCamera(NodeAddedEvent evt, SingleNode<MapInstanceComponent> node)
        {
            Transform transform = new GameObject("BattleCameraRoot").transform;
            GameObject obj3 = GameObject.Find(ClientGraphicsConstants.MAIN_CAMERA_NAME);
            Transform transform2 = obj3.transform;
            transform.SetPositionSafe(transform2.position);
            transform.SetRotationSafe(transform2.rotation);
            transform.SetParent(node.component.SceneRoot.transform, true);
            transform2.SetParent(transform, true);
            Entity entity = base.CreateEntity(typeof(CameraTemplate), "camera");
            EntityBehaviour component = obj3.GetComponent<EntityBehaviour>();
            if (component.Entity != null)
            {
                component.DestroyEntity();
            }
            component.BuildEntity(entity);
            entity.AddComponent(new CameraRootTransformComponent(transform));
            entity.AddComponent<BattleCameraComponent>();
            Camera unityCamera = obj3.GetComponent<Camera>();
            entity.AddComponent(new CameraComponent(unityCamera));
            CameraTransformDataComponent component2 = new CameraTransformDataComponent();
            TransformData data = new TransformData {
                Position = transform2.position,
                Rotation = transform2.rotation
            };
            component2.Data = data;
            entity.AddComponent(component2);
            entity.AddComponent<CameraFOVUpdateComponent>();
            entity.AddComponent<BezierPositionComponent>();
            entity.AddComponent<ApplyCameraTransformComponent>();
            entity.AddComponent<TransitionCameraComponent>();
            entity.AddComponent(new CameraShakerComponent(obj3.AddComponent<CameraShaker>()));
            BurningTargetBloomComponent component4 = new BurningTargetBloomComponent {
                burningTargetBloom = unityCamera.GetComponent<BurningTargetBloom>()
            };
            entity.AddComponent(component4);
            this.SetupCameraESM(entity);
        }

        [OnEventFire]
        public void DeleteCamera(NodeRemoveEvent evt, SingleNode<MapInstanceComponent> node, [JoinAll] SingleNode<BattleCameraComponent> camera)
        {
            base.DeleteEntity(camera.Entity);
        }

        [OnEventFire]
        public void FollowNewUser(NodeAddedEvent e, WeaponNode weapon, [JoinByUser] FollowedBattleUserNode followedBattleUser)
        {
            if (!weapon.Entity.HasComponent<CameraTargetComponent>())
            {
                CameraTargetComponent component = new CameraTargetComponent(weapon.weaponInstance.WeaponInstance);
                weapon.Entity.AddComponent(component);
            }
        }

        [OnEventFire]
        public void ResetTransitionCamera(NodeRemoveEvent e, CameraNode camera)
        {
            camera.transitionCamera.Reset();
        }

        [OnEventComplete]
        public void SetTankAsReadyForCameraJoining(TankMovementInitEvent evt, SelfTankNode tank)
        {
            tank.Entity.AddComponent<SelfTankReadyForCameraComponent>();
        }

        [OnEventComplete]
        public void SetTankAsReadyForCameraJoining(TankMovementInitEvent evt, RemoteTankNode tank, [JoinByUser] FollowedBattleUserNode followedBattleUser)
        {
            tank.Entity.AddComponent<FollowedTankReadyToCameraComponent>();
        }

        private void SetupCameraESM(Entity cameraEntity)
        {
            CameraESMComponent component = new CameraESMComponent();
            cameraEntity.AddComponent(component);
            EntityStateMachine esm = component.Esm;
            esm.AddState<CameraStates.CameraFollowState>();
            esm.AddState<CameraStates.CameraFreeState>();
            esm.AddState<CameraStates.CameraGoState>();
            esm.AddState<CameraStates.CameraOrbitState>();
            esm.AddState<CameraStates.CameraTransitionState>();
        }

        [OnEventFire]
        public void SwitchCameraToSpawnState(NodeAddedEvent evt, SingleNode<FollowedTankReadyToCameraComponent> tank, [JoinByUser] SingleNode<UserUidComponent> userUidNode, [JoinAll] FollowESMNode camera)
        {
            TransitionCameraComponent transitionCamera = camera.transitionCamera;
            transitionCamera.CameraSaveData = CameraSaveData.CreateFollowData(userUidNode.component.Uid, camera.bezierPosition.BezierPosition.GetBaseRatio(), camera.bezierPosition.BezierPosition.GetRatioOffset());
            transitionCamera.Spawn = true;
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        [OnEventFire]
        public void SwitchCameraToSpawnState(NodeAddedEvent evt, SingleNode<FollowedTankReadyToCameraComponent> tank, [JoinByUser] SingleNode<UserUidComponent> userUidNode, [JoinAll] MouseOrbitESMNode camera)
        {
            TransitionCameraComponent transitionCamera = camera.transitionCamera;
            transitionCamera.CameraSaveData = CameraSaveData.CreateMouseOrbitData(userUidNode.component.Uid, camera.mouseOrbitCamera.distance, camera.mouseOrbitCamera.targetRotation);
            transitionCamera.Spawn = true;
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        [OnEventFire]
        public void SwitchCameraToSpawnState(NodeAddedEvent evt, SelfTankReadyForCameraNode tank, [JoinByUser] SingleNode<UserUidComponent> userUidNode, [Context, JoinAll] ESMNode camera, [JoinAll] Optional<SingleNode<FollowCameraComponent>> followCameraOptional)
        {
            TransitionCameraComponent transitionCamera = camera.transitionCamera;
            transitionCamera.CameraSaveData = CameraSaveData.CreateFollowData(userUidNode.component.Uid, camera.bezierPosition.BezierPosition.GetBaseRatio(), camera.bezierPosition.BezierPosition.GetRatioOffset());
            transitionCamera.Spawn = true;
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        public class CameraNode : Node
        {
            public TransitionCameraComponent transitionCamera;
            public TransitionCameraStateComponent transitionCameraState;
        }

        public class ESMNode : Node
        {
            public CameraESMComponent cameraESM;
            public BezierPositionComponent bezierPosition;
            public TransitionCameraComponent transitionCamera;
        }

        public class FollowedBattleUserNode : Node
        {
            public FollowedBattleUserComponent followedBattleUser;
            public UserGroupComponent userGroup;
        }

        public class FollowESMNode : BattleCameraBuilderSystem.ESMNode
        {
            public FollowCameraComponent followCamera;
        }

        public class MouseOrbitESMNode : BattleCameraBuilderSystem.ESMNode
        {
            public MouseOrbitCameraComponent mouseOrbitCamera;
        }

        public class RemoteTankNode : Node
        {
            public UserGroupComponent userGroup;
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public SelfTankComponent selfTank;
        }

        public class SelfTankReadyForCameraNode : Node
        {
            public TankGroupComponent tankGroup;
            public SelfTankComponent selfTank;
            public SelfTankReadyForCameraComponent selfTankReadyForCamera;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

