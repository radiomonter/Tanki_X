namespace Tanks.Battle.ClientGraphics.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class SaveLoadCameraSystem : ECSSystem
    {
        private static readonly KeyCode[] saveKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };

        [OnEventComplete]
        public void CheckSaveOrLoadCamera(UpdateEvent evt, SpectatorCameraNode camera)
        {
            for (int i = 0; i < saveKeys.Length; i++)
            {
                if (InputManager.GetKeyDown(saveKeys[i]))
                {
                    camera.spectatorCamera.SaveCameraModificatorKeyHasBeenPressed = false;
                }
                if (InputManager.GetKey(saveKeys[i]) && InputManager.CheckAction(SpectatorCameraActions.SaveCameraModificator))
                {
                    camera.spectatorCamera.SaveCameraModificatorKeyHasBeenPressed = true;
                }
                if (InputManager.GetKeyUp(saveKeys[i]))
                {
                    if (camera.spectatorCamera.SaveCameraModificatorKeyHasBeenPressed)
                    {
                        base.ScheduleEvent(new SaveCameraEvent(i), camera.Entity);
                    }
                    else
                    {
                        base.ScheduleEvent(new LoadCameraEvent(i), camera.Entity);
                    }
                }
            }
        }

        [OnEventFire]
        public void LoadCamera(LoadCameraEvent e, SpectatorCameraNode camera, [JoinAll] Optional<SingleNode<FollowedBattleUserComponent>> followedUser, [JoinAll] ICollection<UserNode> users)
        {
            <LoadCamera>c__AnonStorey0 storey = new <LoadCamera>c__AnonStorey0();
            if (camera.spectatorCamera.savedCameras.ContainsKey(e.slotIndex))
            {
                storey.data = camera.spectatorCamera.savedCameras[e.slotIndex];
                if (storey.data.Type == CameraType.Free)
                {
                    if (followedUser.IsPresent())
                    {
                        followedUser.Get().Entity.RemoveComponent<FollowedBattleUserComponent>();
                    }
                    this.SetCameraLoading(camera, storey.data);
                }
                else
                {
                    UserNode node = users.ToList<UserNode>().SingleOrDefault<UserNode>(new Func<UserNode, bool>(storey.<>m__0));
                    if (node != null)
                    {
                        base.ScheduleEvent(new CameraLoadedSaveValidateEvent(storey.data), node.Entity);
                    }
                }
            }
        }

        [OnEventFire]
        public void RemoveCurrentCameraController(CameraLoadedSaveValidateEvent e, UserNode user, [JoinByUser] SingleNode<UserInBattleAsTankComponent> userAsTank, [JoinByUser] SingleNode<BattleUserComponent> battleUser, [JoinAll] Optional<SingleNode<FollowedBattleUserComponent>> followedUser, [JoinAll] SpectatorCameraNode camera)
        {
            if (followedUser.IsPresent())
            {
                followedUser.Get().Entity.RemoveComponent<FollowedBattleUserComponent>();
            }
            battleUser.Entity.AddComponent<FollowedBattleUserComponent>();
            this.SetCameraLoading(camera, e.SaveData);
        }

        [OnEventFire]
        public void Save(SaveCameraEvent e, FreeCameraNode camera)
        {
            CameraSaveData data = CameraSaveData.CreateFreeData(camera.cameraRootTransform.Root);
            camera.spectatorCamera.savedCameras[e.slotIndex] = data;
        }

        [OnEventFire]
        public void Save(SaveCameraEvent e, FollowCameraNode camera, [JoinAll] SingleNode<FollowedBattleUserComponent> followedUser, [JoinByUser] UserNode user)
        {
            CameraSaveData data = CameraSaveData.CreateFollowData(user.userUid.Uid, camera.bezierPosition.BezierPosition.GetBaseRatio(), camera.bezierPosition.BezierPosition.GetRatioOffset());
            camera.spectatorCamera.savedCameras[e.slotIndex] = data;
        }

        [OnEventFire]
        public void Save(SaveCameraEvent e, MouseOrbitCameraNode camera, [JoinAll] SingleNode<FollowedBattleUserComponent> followedUser, [JoinByUser] UserNode user)
        {
            CameraSaveData data = CameraSaveData.CreateMouseOrbitData(user.userUid.Uid, camera.mouseOrbitCamera.distance, camera.mouseOrbitCamera.targetRotation);
            camera.spectatorCamera.savedCameras[e.slotIndex] = data;
        }

        private void SetCameraLoading(SpectatorCameraNode camera, CameraSaveData data)
        {
            camera.transitionCamera.CameraSaveData = data;
            camera.cameraESM.esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        [CompilerGenerated]
        private sealed class <LoadCamera>c__AnonStorey0
        {
            internal CameraSaveData data;

            internal bool <>m__0(SaveLoadCameraSystem.UserNode userNode) => 
                userNode.userUid.Uid == this.data.UserUid;
        }

        public class CameraLoadedSaveValidateEvent : Event
        {
            public CameraLoadedSaveValidateEvent(CameraSaveData saveData)
            {
                this.SaveData = saveData;
            }

            public CameraSaveData SaveData { get; set; }
        }

        public class CameraTargetNode : Node
        {
            public CameraTargetComponent cameraTarget;
            public UserGroupComponent userGroup;
            public WeaponInstanceComponent weaponInstance;
        }

        public class FollowCameraNode : SaveLoadCameraSystem.SpectatorCameraNode
        {
            public FollowCameraComponent followCamera;
        }

        public class FreeCameraNode : SaveLoadCameraSystem.SpectatorCameraNode
        {
            public FreeCameraComponent freeCamera;
        }

        public class LoadCameraEvent : Event
        {
            public int slotIndex;

            public LoadCameraEvent(int slotIndex)
            {
                this.slotIndex = slotIndex;
            }
        }

        public class MouseOrbitCameraNode : SaveLoadCameraSystem.SpectatorCameraNode
        {
            public MouseOrbitCameraComponent mouseOrbitCamera;
        }

        public class SaveCameraEvent : Event
        {
            public int slotIndex;

            public SaveCameraEvent(int slotIndex)
            {
                this.slotIndex = slotIndex;
            }
        }

        [Not(typeof(TransitionCameraStateComponent))]
        public class SpectatorCameraNode : Node
        {
            public CameraComponent camera;
            public CameraRootTransformComponent cameraRootTransform;
            public CameraTransformDataComponent cameraTransformData;
            public TransitionCameraComponent transitionCamera;
            public SpectatorCameraComponent spectatorCamera;
            public CameraESMComponent cameraESM;
            public BezierPositionComponent bezierPosition;
        }

        public class UserNode : Node
        {
            public UserGroupComponent userGroup;
            public UserUidComponent userUid;
        }

        public class WeaponNode : Node
        {
            public UserGroupComponent userGroup;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

