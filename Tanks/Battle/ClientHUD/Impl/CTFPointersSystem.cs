namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class CTFPointersSystem : ECSSystem
    {
        private const float UP_OFFSET = 0.125f;
        private const float DOWN_OFFSET = 0.14f;
        private const float SIDE_OFFSET = 0.03f;
        private const float FLAG_HEIGHT = 5.5f;
        private const float DISTANCE = 20f;

        private Vector2 CalculateBaseScreenPosition(Camera camera, Vector3 worldPos, Vector2 selfRect, out bool onScreen)
        {
            Vector3 position = new Vector3(worldPos.x, worldPos.y + 5.5f, worldPos.z);
            Vector3 topPoint = camera.WorldToScreenPoint(position);
            onScreen = this.FlagOnScreen(topPoint, camera.WorldToScreenPoint(worldPos), Vector3.Distance(camera.transform.position, worldPos));
            if (topPoint.z < 0f)
            {
                topPoint *= -1f;
            }
            topPoint = (Vector3) this.GetBehindPosition(topPoint);
            if (!onScreen)
            {
                topPoint = (Vector3) this.ClampScreenPosToScreenSize(topPoint, selfRect);
            }
            return topPoint;
        }

        private Vector2 CalculateFlagScreenPosition(FlagPointerComponent enemyFlag, Camera camera, Vector3 worldPos, Vector2 selfRect, out bool onScreen)
        {
            Vector3 position = new Vector3(worldPos.x, (worldPos.y + 5.5f) - 2f, worldPos.z);
            Vector3 topPoint = camera.WorldToScreenPoint(position);
            Vector2 vector4 = new Vector2(((float) Screen.width) / 2f, ((float) Screen.height) / 2f);
            Vector2 normalized = (topPoint - vector4).normalized;
            float z = (Mathf.Atan2(normalized.y, normalized.x) - 5156.62f) * 57.29578f;
            onScreen = this.FlagOnScreen(topPoint, camera.WorldToScreenPoint(worldPos), Vector3.Distance(camera.transform.position, worldPos));
            if (topPoint.z < 0f)
            {
                topPoint *= -1f;
                z -= 10313.24f;
            }
            topPoint = (Vector3) this.GetBehindPosition(topPoint);
            if (onScreen)
            {
                enemyFlag.pointer.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                enemyFlag.pointer.transform.localRotation = Quaternion.Euler(0f, 0f, z);
                topPoint = (Vector3) this.ClampScreenPosToScreenSize(topPoint, selfRect);
            }
            return topPoint;
        }

        [OnEventFire]
        public void ChangeBaseIconWhenFlagNotHome(NodeAddedEvent e, [Combine] FlagNotHomeNode flag, [JoinByTeam] FlagPedestalNode flagPedestal, SelfBattleUser user, [Context] SingleNode<AlliesBasePointerComponent> alliesPointer, [Context] EnemyBasePointer enemyPointer)
        {
            this.SetFlagNotHomeIcon(flag, user, alliesPointer, enemyPointer);
        }

        [OnEventFire]
        public void ChangeBaseIconWhenFlagNotHome(NodeRemoveEvent e, [Combine] FlagHomeNode flag, [JoinByTeam] FlagPedestalNode flagPedestal, [JoinAll] SelfBattleUser user, [JoinAll] SingleNode<AlliesBasePointerComponent> alliesPointer, [JoinAll] EnemyBasePointer enemyPointer)
        {
            this.SetFlagNotHomeIcon(flag, user, alliesPointer, enemyPointer);
        }

        [OnEventFire]
        public void ChangeFlagTextWhenCaptured(NodeRemoveEvent e, [Combine] FlagGroundedNode flag, [Context] SelfBattleUser user, [Context] SingleNode<AlliesFlagPointerComponent> alliesPointer, [Context] EnemyFlagPointerNode enemyPointer)
        {
            if (user.teamGroup.Key == flag.teamGroup.Key)
            {
                alliesPointer.component.SetFlagCapturedState();
            }
            else
            {
                enemyPointer.enemyFlagPointer.SetFlagCapturedState();
            }
        }

        [OnEventFire]
        public void ChangeFlagTextWhenGrounded(NodeAddedEvent e, [Combine] FlagGroundedNode flag, [Context] SelfBattleUser user, [Context] SingleNode<AlliesFlagPointerComponent> alliesPointer, [Context] EnemyFlagPointerNode enemyPointer)
        {
            if (user.teamGroup.Key == flag.teamGroup.Key)
            {
                alliesPointer.component.SetFlagOnTheGroundState();
            }
            else
            {
                enemyPointer.enemyFlagPointer.SetFlagOnTheGroundState();
            }
        }

        private Vector2 ClampScreenPosToScreenSize(Vector3 screenPos, Vector2 selfSize)
        {
            float min = (selfSize.x / 2f) + (Screen.width * 0.03f);
            float num4 = (selfSize.y / 2f) + (Screen.height * 0.14f);
            return new Vector2(Mathf.Clamp(screenPos.x, min, (Screen.width - (selfSize.x / 2f)) - (Screen.width * 0.03f)), Mathf.Clamp(screenPos.y, num4, (Screen.height - (selfSize.y / 2f)) - (Screen.height * 0.125f)));
        }

        private bool FlagOnScreen(Vector3 topPoint, Vector3 downPoint, float distance) => 
            this.RangeWithinScreen(topPoint, downPoint);

        private Vector2 GetBehindPosition(Vector3 currentPosition)
        {
            Vector2 vector2;
            Vector2 q = new Vector2((float) Screen.width, (float) Screen.height) / 2f;
            float x = 0.0285f;
            float y = 0.11875f;
            return (!DepenetrationForce.LineSegementsIntersect(new Vector2(x, y), new Vector2(Screen.width - x, y), q, currentPosition, out vector2, false) ? (!DepenetrationForce.LineSegementsIntersect(new Vector2(x, y), new Vector2(x, Screen.height - y), q, currentPosition, out vector2, false) ? (!DepenetrationForce.LineSegementsIntersect(new Vector2(x, Screen.height - y), new Vector2(Screen.width - x, Screen.height - y), q, currentPosition, out vector2, false) ? (!DepenetrationForce.LineSegementsIntersect(new Vector2(Screen.width - x, y), new Vector2(Screen.width - x, Screen.height - y), q, currentPosition, out vector2, false) ? currentPosition : vector2) : vector2) : vector2) : vector2);
        }

        private Vector2 GetCanvasPosition(Vector3 screenPosition, RectTransform canvasRect)
        {
            Vector2 vector = new Vector2((screenPosition.x / ((float) Screen.width)) * canvasRect.rect.size.x, (screenPosition.y / ((float) Screen.height)) * canvasRect.rect.size.y);
            return (vector - (canvasRect.rect.size / 2f));
        }

        private FlagNotHomeNode GetOppositeTeamFlag(ICollection<FlagNotHomeNode> flags, SelfBattleUser user)
        {
            for (int i = 0; i < flags.Count; i++)
            {
                if (flags.ElementAt<FlagNotHomeNode>(i).teamGroup.Key != user.teamGroup.Key)
                {
                    return flags.ElementAt<FlagNotHomeNode>(i);
                }
            }
            return null;
        }

        private FlagPedestalNode GetOppositeTeamPedestal(ICollection<FlagPedestalNode> pedestals, SelfBattleUser user)
        {
            for (int i = 0; i < pedestals.Count; i++)
            {
                if (pedestals.ElementAt<FlagPedestalNode>(i).teamGroup.Key != user.teamGroup.Key)
                {
                    return pedestals.ElementAt<FlagPedestalNode>(i);
                }
            }
            return pedestals.First<FlagPedestalNode>();
        }

        [OnEventFire]
        public void HidePointerWhenFlagHome(NodeAddedEvent e, [Combine] FlagHomeNode flag, SelfBattleUser user, [Context] SingleNode<AlliesFlagPointerComponent> alliesPointer, [Context] EnemyFlagPointerNode enemyPointer, [Context] SingleNode<AlliesBasePointerComponent> alliesBasePointer, [Context] EnemyBasePointer enemyBasePointer)
        {
            if (flag.teamGroup.Key == user.teamGroup.Key)
            {
                alliesPointer.component.Hide();
                alliesBasePointer.component.SetFlagAtHomeState();
            }
            else
            {
                enemyPointer.enemyFlagPointer.Hide();
                enemyBasePointer.enemyBasePointer.SetFlagAtHomeState();
            }
        }

        private bool NotFlagCarrier(HUDNodes.SelfTankNode selfTank, FlagNotHomeNode flag) => 
            !(flag.Entity.HasComponent<TankGroupComponent>() && (selfTank.tankGroup.Key == flag.Entity.GetComponent<TankGroupComponent>().Key));

        private bool PointWithinScreen(Vector3 point) => 
            ((point.z > 0f) && ((point.x > 0f) && ((point.x < Screen.width) && (point.y > 0f)))) && (point.y < Screen.height);

        private bool RangeWithinScreen(Vector3 topPoint, Vector3 downPoint) => 
            ((topPoint.z > 0f) && ((downPoint.z > 0f) && ((topPoint.x > 0f) && ((topPoint.x < Screen.width) && ((downPoint.x > 0f) && ((downPoint.x < Screen.width) && (topPoint.y > 0f))))))) && (downPoint.y < Screen.height);

        private void SetBasePosition(CTFPointerComponent basePointer, Vector3 flagPedestalPosition, Camera camera)
        {
            bool flag;
            Vector2 size = basePointer.selfRect.rect.size;
            Vector3 localScale = basePointer.parentCanvasRect.localScale;
            Vector2 selfRect = new Vector2(size.x * localScale.x, size.y * localScale.y);
            Vector2 vector4 = this.CalculateBaseScreenPosition(camera, flagPedestalPosition, selfRect, out flag);
            RectTransform parentCanvasRect = basePointer.parentCanvasRect;
            basePointer.transform.localPosition = (Vector3) (this.GetCanvasPosition((Vector3) vector4, parentCanvasRect) + new Vector2(0f, !flag ? 0f : (size.y / 2f)));
        }

        private void SetFlagNotHomeIcon(FlagNode flag, SelfBattleUser user, SingleNode<AlliesBasePointerComponent> alliesPointer, EnemyBasePointer enemyPointer)
        {
            if (flag.teamGroup.Key == user.teamGroup.Key)
            {
                alliesPointer.component.SetFlagStolenState();
            }
            else
            {
                enemyPointer.enemyBasePointer.SetFlagStolenState();
            }
        }

        private void SetFlagPointerPosition(FlagNotHomeNode flag, FlagPointerComponent pointer, Camera battleCamera)
        {
            bool flag2;
            pointer.Show();
            BoxCollider boxCollider = flag.flagCollider.boxCollider;
            Vector2 size = pointer.selfRect.rect.size;
            Vector3 localScale = pointer.parentCanvasRect.localScale;
            Vector2 selfRect = new Vector2(size.x * localScale.x, size.y * localScale.y);
            Vector3 worldPos = boxCollider.transform.TransformPoint(boxCollider.center);
            Vector2 vector5 = this.CalculateFlagScreenPosition(pointer, battleCamera, worldPos, selfRect, out flag2);
            RectTransform parentCanvasRect = pointer.parentCanvasRect;
            pointer.transform.localPosition = (Vector3) (this.GetCanvasPosition((Vector3) vector5, parentCanvasRect) + new Vector2(0f, !flag2 ? 0f : (size.y / 2f)));
        }

        [OnEventFire]
        public void ShowPointersInCTF(NodeAddedEvent e, SingleNode<CTFComponent> ctfGameNode, [Context] SingleNode<AlliesBasePointerComponent> alliesPointer, [Context] EnemyBasePointer enemyPointer, [Context] SelfBattleUser user)
        {
            alliesPointer.component.Show();
            enemyPointer.enemyBasePointer.Show();
        }

        [OnEventFire]
        public void UpdateAlliasFlagPointer(UpdateEvent e, SingleNode<AlliesFlagPointerComponent> pointer, [JoinAll] HUDNodes.SelfTankNode selfTank, [JoinByTeam] FlagNotHomeNode flag, [JoinAll] BattleCameraNode battleCamera, [JoinAll] SingleNode<CTFComponent> ctfGameNode)
        {
            if (this.NotFlagCarrier(selfTank, flag))
            {
                this.SetFlagPointerPosition(flag, pointer.component, battleCamera.camera.UnityCamera);
            }
            else
            {
                pointer.component.Hide();
            }
        }

        [OnEventFire]
        public void UpdateAlliesBasePointer(UpdateEvent e, SingleNode<AlliesBasePointerComponent> pointer, [JoinAll] SelfBattleUser user, [JoinByTeam] FlagPedestalNode pedestal, [JoinAll] BattleCameraNode battleCamera, [JoinAll] SingleNode<CTFComponent> ctfGameNode)
        {
            this.SetBasePosition(pointer.component, pedestal.flagPedestal.Position, battleCamera.camera.UnityCamera);
        }

        [OnEventFire]
        public void UpdateEnemyBasePointer(UpdateEvent e, EnemyBasePointer pointer, [JoinAll] SelfBattleUser user, [JoinAll] ICollection<FlagPedestalNode> pedestals, [JoinAll] BattleCameraNode battleCamera, [JoinAll] SingleNode<CTFComponent> ctfGameNode)
        {
            if (pedestals.Count >= 2)
            {
                FlagPedestalNode oppositeTeamPedestal = this.GetOppositeTeamPedestal(pedestals, user);
                this.SetBasePosition(pointer.enemyBasePointer, oppositeTeamPedestal.flagPedestal.Position, battleCamera.camera.UnityCamera);
            }
        }

        [OnEventFire]
        public void UpdateEnemyFlagPointer(UpdateEvent e, EnemyFlagPointerNode pointer, [JoinAll] SelfBattleUser user, [JoinByUser] HUDNodes.SelfTankNode selfTank, [JoinByBattle] ICollection<FlagNotHomeNode> flags, [JoinAll] BattleCameraNode battleCamera, [JoinAll] SingleNode<CTFComponent> ctfGameNode)
        {
            if (flags.Count >= 2)
            {
                FlagNotHomeNode oppositeTeamFlag = this.GetOppositeTeamFlag(flags, user);
                if ((oppositeTeamFlag != null) && this.NotFlagCarrier(selfTank, oppositeTeamFlag))
                {
                    this.SetFlagPointerPosition(oppositeTeamFlag, pointer.enemyFlagPointer, battleCamera.camera.UnityCamera);
                }
                else
                {
                    pointer.enemyFlagPointer.Hide();
                }
            }
        }

        public class BattleCameraNode : Node
        {
            public CameraComponent camera;
            public BattleCameraComponent battleCamera;
        }

        public class EnemyBasePointer : Node
        {
            public EnemyBasePointerComponent enemyBasePointer;
        }

        public class EnemyFlagPointerNode : Node
        {
            public EnemyFlagPointerComponent enemyFlagPointer;
        }

        public class FlagGroundedNode : CTFPointersSystem.FlagNode
        {
            public FlagGroundedStateComponent flagGroundedState;
        }

        public class FlagHomeNode : CTFPointersSystem.FlagNode
        {
            public FlagHomeStateComponent flagHomeState;
        }

        public class FlagNode : Node
        {
            public TeamGroupComponent teamGroup;
            public BattleGroupComponent battleGroup;
            public FlagInstanceComponent flagInstance;
            public FlagColliderComponent flagCollider;
        }

        [Not(typeof(FlagHomeStateComponent))]
        public class FlagNotHomeNode : CTFPointersSystem.FlagNode
        {
        }

        public class FlagPedestalNode : Node
        {
            public FlagPedestalComponent flagPedestal;
            public TeamGroupComponent teamGroup;
        }

        public class SelfBattleUser : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
            public TeamGroupComponent teamGroup;
        }
    }
}

