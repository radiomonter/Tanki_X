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
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class SpectatorCameraManagerSystem : ECSSystem
    {
        public static readonly string SCREEN_SHOT_FILE_NAME = "TankiX";
        [CompilerGenerated]
        private static Func<UserUidNode, string> <>f__am$cache0;

        [OnEventFire]
        public void ChangeCameraState(UpdateEvent e, FollowCameraNode camera)
        {
            if (InputManager.GetActionKeyDown(SpectatorCameraActions.MouseOrbitMode))
            {
                camera.cameraESM.Esm.ChangeState<CameraStates.CameraOrbitState>().mouseOrbitCamera.targetRotation = camera.cameraTransformData.Data.Rotation;
            }
        }

        [OnEventFire]
        public void ChangeCameraState(UpdateEvent e, MouseOrbitCameraNode camera)
        {
            if (InputManager.GetActionKeyDown(SpectatorCameraActions.MouseOrbitMode))
            {
                camera.cameraESM.Esm.ChangeState<CameraStates.CameraFollowState>();
            }
        }

        [OnEventFire]
        public void ChangeCameraState(SpectatorGoBackRequestEvent e, Node anyNode, [JoinAll] NotFreeCameraNode camera)
        {
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraFreeState>();
        }

        private List<UserUidNode> FilterUsers(ICollection<UserUidNode> users, ICollection<ReadyBattleUserAsTankNode> tanks, UserUidNode currentUser, FollowedBattleUserNode currentBattleUser)
        {
            <FilterUsers>c__AnonStorey1 storey = new <FilterUsers>c__AnonStorey1 {
                currentUser = currentUser,
                tanksList = tanks.ToList<ReadyBattleUserAsTankNode>()
            };
            if (currentBattleUser.Entity.HasComponent<TeamGroupComponent>())
            {
                <FilterUsers>c__AnonStorey0 storey2 = new <FilterUsers>c__AnonStorey0 {
                    currentTeamKey = currentBattleUser.Entity.GetComponent<TeamGroupComponent>().Key
                };
                List<ReadyBattleUserAsTankNode> list = storey.tanksList.Where<ReadyBattleUserAsTankNode>(new Func<ReadyBattleUserAsTankNode, bool>(storey2.<>m__0)).ToList<ReadyBattleUserAsTankNode>();
                if (list.Count > 0)
                {
                    storey.tanksList = list;
                }
            }
            List<UserUidNode> list2 = users.Where<UserUidNode>(new Func<UserUidNode, bool>(storey.<>m__0)).ToList<UserUidNode>();
            if (!list2.Exists(new Predicate<UserUidNode>(storey.<>m__1)))
            {
                list2.Add(storey.currentUser);
            }
            return list2;
        }

        [OnEventFire]
        public void FollowNewUser(NodeAddedEvent e, FollowedBattleUserNode followedBattleUser, [JoinByUser] WeaponInstanceNode weaponInstanceNode, [JoinAll, Combine] Optional<SingleNode<TeleportCameraIntentComponent>> moveIntent)
        {
            CameraTargetComponent component = new CameraTargetComponent {
                TargetObject = weaponInstanceNode.weaponInstance.WeaponInstance
            };
            weaponInstanceNode.Entity.AddComponent(component);
            if (moveIntent.IsPresent())
            {
                base.ScheduleEvent<CameraFollowEvent>(weaponInstanceNode.Entity);
                moveIntent.Get().Entity.RemoveComponent<TeleportCameraIntentComponent>();
            }
        }

        [OnEventFire]
        public void FollowUser(CameraFollowEvent e, BattleUserNode battleUser, [JoinAll] Optional<FollowedBattleUserNode> prevFollowedBattleUser, [JoinAll] SingleNode<CameraESMComponent> camera)
        {
            if (prevFollowedBattleUser.IsPresent())
            {
                prevFollowedBattleUser.Get().Entity.RemoveComponent<FollowedBattleUserComponent>();
            }
            battleUser.Entity.AddComponent<TeleportCameraIntentComponent>();
            battleUser.Entity.AddComponent<FollowedBattleUserComponent>();
            camera.component.Esm.ChangeState<CameraStates.CameraFollowState>();
        }

        private UserUidNode GetUserUidForFollowingTank(List<UserUidNode> userUids, UserUidNode currentTargetUserUid, SwitchUserDirection switchUserDirection)
        {
            <GetUserUidForFollowingTank>c__AnonStorey3 storey = new <GetUserUidForFollowingTank>c__AnonStorey3();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => x.userUid.Uid;
            }
            List<string> list = userUids.Select<UserUidNode, string>(<>f__am$cache0).ToList<string>();
            storey.uid = currentTargetUserUid.userUid.Uid;
            list.Sort();
            int index = list.IndexOf(storey.uid);
            if (switchUserDirection == SwitchUserDirection.PrevUser)
            {
                index--;
            }
            else if (switchUserDirection == SwitchUserDirection.NextUser)
            {
                index++;
            }
            storey.uid = list[(index + list.Count) % list.Count];
            return userUids.Single<UserUidNode>(new Func<UserUidNode, bool>(storey.<>m__0));
        }

        [OnEventFire]
        public void InitSpectatorCamera(NodeAddedEvent e, SpectatorNode spectator, [JoinAll] CameraNode camera)
        {
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraFreeState>();
            camera.Entity.AddComponent<SpectatorCameraComponent>();
        }

        [OnEventComplete]
        public void ReturnToFreeCamera(NodeRemoveEvent e, BattleUserNode battleUser, [JoinByUser] FollowedBattleUserNode followedBattleUser, [JoinAll] SingleNode<CameraESMComponent> camera)
        {
            followedBattleUser.followedBattleUser.UserHasLeftBattle = true;
            camera.component.Esm.ChangeState<CameraStates.CameraFreeState>();
        }

        [OnEventFire]
        public void Screenshot(UpdateEvent evt, SingleNode<SpectatorCameraComponent> cameraNode)
        {
            if (InputManager.CheckAction(SpectatorCameraActions.Screenshot))
            {
                object[] objArray1 = new object[] { SCREEN_SHOT_FILE_NAME, "_", cameraNode.component.SequenceScreenshot, ".png" };
                Application.CaptureScreenshot(string.Concat(objArray1));
            }
        }

        [OnEventFire]
        public void SetNewTargetToCamera(SetNewTargetCameraEvent e, UserUidNode user, [JoinByUser] BattleUserNode battleUser, [JoinAll] FollowedBattleUserNode prevFollowedUser, [JoinAll] FollowCameraNode camera)
        {
            prevFollowedUser.Entity.RemoveComponent<FollowedBattleUserComponent>();
            battleUser.Entity.AddComponent<FollowedBattleUserComponent>();
            CameraOffsetConfigComponent cameraOffsetConfig = camera.cameraOffsetConfig;
            Vector3 vector = new Vector3(cameraOffsetConfig.XOffset, cameraOffsetConfig.YOffset, cameraOffsetConfig.ZOffset);
            BezierPosition bezierPosition = camera.bezierPosition.BezierPosition;
            camera.transitionCamera.CameraSaveData = CameraSaveData.CreateFollowData(user.userUid.Uid, bezierPosition.GetBaseRatio(), bezierPosition.GetRatioOffset());
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        [OnEventFire]
        public void SetNewTargetToCamera(SetNewTargetCameraEvent e, UserUidNode user, [JoinByUser] BattleUserNode battleUser, [JoinAll] FollowedBattleUserNode prevFollowedUser, [JoinAll] MouseOrbitCameraNode camera)
        {
            prevFollowedUser.Entity.RemoveComponent<FollowedBattleUserComponent>();
            battleUser.Entity.AddComponent<FollowedBattleUserComponent>();
            camera.transitionCamera.CameraSaveData = CameraSaveData.CreateMouseOrbitData(user.userUid.Uid, camera.mouseOrbitCamera.distance, camera.mouseOrbitCamera.targetRotation);
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        [OnEventFire]
        public void StopFollowUser(NodeRemoveEvent e, FollowedBattleUserNode followedBattleUser, [JoinByUser] SingleNode<CameraTargetComponent> cameraTarget)
        {
            cameraTarget.Entity.RemoveComponent<CameraTargetComponent>();
        }

        [OnEventFire]
        public void StopFollowUser(NodeRemoveEvent e, FollowedBattleUserNode followedBattleUser, [JoinAll] SingleNode<TeleportCameraIntentComponent> moveCameraIntent)
        {
            followedBattleUser.Entity.RemoveComponent<TeleportCameraIntentComponent>();
        }

        [OnEventFire]
        public void SwitchFollowedTank(UpdateEvent e, SpectatorCameraNode camera, [JoinAll] FollowedBattleUserNode followedUser, [JoinByUser] UserUidNode userUid)
        {
            if (InputManager.GetActionKeyDown(SpectatorCameraActions.PrevTank) || Input.GetMouseButtonDown(UnityInputConstants.MOUSE_BUTTON_RIGHT))
            {
                base.ScheduleEvent(new SwitchFollowedUserEvent(SwitchUserDirection.PrevUser), userUid.Entity);
            }
            else if (InputManager.GetActionKeyDown(SpectatorCameraActions.NextTank) || Input.GetMouseButtonDown(UnityInputConstants.MOUSE_BUTTON_LEFT))
            {
                base.ScheduleEvent(new SwitchFollowedUserEvent(SwitchUserDirection.NextUser), userUid.Entity);
            }
        }

        [OnEventFire]
        public void SwitchFollowedUser(SwitchFollowedUserEvent e, UserUidNode userNode1, [JoinByUser] FollowedBattleUserNode followedUser, [JoinByBattle] ICollection<UserUidNode> allUserUids, UserUidNode userNode2, [JoinByBattle] ICollection<ReadyBattleUserAsTankNode> tankUsers)
        {
            List<UserUidNode> userUids = this.FilterUsers(allUserUids, tankUsers, userNode1, followedUser);
            UserUidNode node = this.GetUserUidForFollowingTank(userUids, userNode1, e.switchUserDirection);
            if (!userNode1.Equals(node))
            {
                base.ScheduleEvent<SetNewTargetCameraEvent>(node.Entity);
            }
        }

        [OnEventFire]
        public void UnlinkCamera(NodeAddedEvent e, SingleNode<FreeCameraComponent> freeCamera, [JoinAll] SingleNode<FollowedBattleUserComponent> followedBattleUser)
        {
            if (!followedBattleUser.component.UserHasLeftBattle)
            {
                followedBattleUser.Entity.RemoveComponent<FollowedBattleUserComponent>();
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        [CompilerGenerated]
        private sealed class <FilterUsers>c__AnonStorey0
        {
            internal long currentTeamKey;

            internal bool <>m__0(SpectatorCameraManagerSystem.ReadyBattleUserAsTankNode tank) => 
                tank.Entity.GetComponent<TeamGroupComponent>().Key != this.currentTeamKey;
        }

        [CompilerGenerated]
        private sealed class <FilterUsers>c__AnonStorey1
        {
            internal List<SpectatorCameraManagerSystem.ReadyBattleUserAsTankNode> tanksList;
            internal SpectatorCameraManagerSystem.UserUidNode currentUser;

            internal bool <>m__0(SpectatorCameraManagerSystem.UserUidNode user)
            {
                <FilterUsers>c__AnonStorey2 storey = new <FilterUsers>c__AnonStorey2 {
                    <>f__ref$1 = this,
                    user = user
                };
                return this.tanksList.Exists(new Predicate<SpectatorCameraManagerSystem.ReadyBattleUserAsTankNode>(storey.<>m__0));
            }

            internal bool <>m__1(SpectatorCameraManagerSystem.UserUidNode user) => 
                user.Equals(this.currentUser);

            private sealed class <FilterUsers>c__AnonStorey2
            {
                internal SpectatorCameraManagerSystem.UserUidNode user;
                internal SpectatorCameraManagerSystem.<FilterUsers>c__AnonStorey1 <>f__ref$1;

                internal bool <>m__0(SpectatorCameraManagerSystem.ReadyBattleUserAsTankNode tank) => 
                    this.user.userGroup.Key == tank.userGroup.Key;
            }
        }

        [CompilerGenerated]
        private sealed class <GetUserUidForFollowingTank>c__AnonStorey3
        {
            internal string uid;

            internal bool <>m__0(SpectatorCameraManagerSystem.UserUidNode x) => 
                x.userUid.Uid == this.uid;
        }

        public class BattleUserNode : Node
        {
            public BattleUserComponent battleUser;
            public UserGroupComponent userGroup;
        }

        public class CameraNode : Node
        {
            public CameraComponent camera;
            public CameraTransformDataComponent cameraTransformData;
            public CameraESMComponent cameraESM;
            public BattleCameraComponent battleCamera;
        }

        public class FollowCameraNode : SpectatorCameraManagerSystem.SpectatorCameraNode
        {
            public FollowCameraComponent followCamera;
            public BezierPositionComponent bezierPosition;
            public CameraOffsetConfigComponent cameraOffsetConfig;
            public TransitionCameraComponent transitionCamera;
        }

        public class FollowedBattleUserNode : Node
        {
            public FollowedBattleUserComponent followedBattleUser;
            public UserGroupComponent userGroup;
        }

        public class FreeCameraNode : SpectatorCameraManagerSystem.SpectatorCameraNode
        {
            public FreeCameraComponent freeCamera;
        }

        public class MouseOrbitCameraNode : SpectatorCameraManagerSystem.SpectatorCameraNode
        {
            public MouseOrbitCameraComponent mouseOrbitCamera;
            public TransitionCameraComponent transitionCamera;
        }

        [Not(typeof(FreeCameraComponent))]
        public class NotFreeCameraNode : SpectatorCameraManagerSystem.SpectatorCameraNode
        {
        }

        public class ReadyBattleUserAsTankNode : Node
        {
            public UserReadyToBattleComponent userReadyToBattle;
            public UserInBattleAsTankComponent userInBattleAsTank;
            public UserGroupComponent userGroup;
        }

        public class SetNewTargetCameraEvent : Event
        {
        }

        public class SpectatorCameraNode : SpectatorCameraManagerSystem.CameraNode
        {
            public SpectatorCameraComponent spectatorCamera;
        }

        public class SpectatorNode : Node
        {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
            public UserGroupComponent userGroup;
        }

        public class SwitchFollowedUserEvent : Event
        {
            public SpectatorCameraManagerSystem.SwitchUserDirection switchUserDirection;

            public SwitchFollowedUserEvent(SpectatorCameraManagerSystem.SwitchUserDirection switchUserDirection)
            {
                this.switchUserDirection = switchUserDirection;
            }
        }

        public enum SwitchUserDirection
        {
            PrevUser,
            NextUser
        }

        public class UserUidNode : Node
        {
            public UserGroupComponent userGroup;
            public UserUidComponent userUid;
            public BattleGroupComponent battleGroup;
        }

        public class WeaponInstanceNode : Node
        {
            public WeaponInstanceComponent weaponInstance;
            public TankPartComponent tankPart;
            public UserGroupComponent userGroup;
        }
    }
}

