namespace Tanks.Battle.ClientCore.Impl
{
    using Assets.tanks.modules.battle.ClientCore.Scripts.Impl.Autopilot;
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankAutopilotControllerSystem : ECSSystem
    {
        private static float NEAR_EQUALS_DOT = 0.99f;
        public const string SURVIVOR_PREFIX = "Survivor ";
        public const string DESERTER_PREFIX = "Deserter ";

        [OnEventFire]
        public void Accept(RequestAutopilotControllerEvent e, SingleNode<TankAutopilotComponent> autopilot, [JoinAll] SelfUserReadyToBattleNode user)
        {
            AcceptAutopilotControllerEvent eventInstance = new AcceptAutopilotControllerEvent {
                Version = autopilot.component.Version
            };
            base.ScheduleEvent(eventInstance, autopilot);
        }

        private void ApplyMissBehaviour(AutopilotTankNode tank, WeaponNode weapon)
        {
            Vector3 vector2 = weapon.weaponInstance.WeaponInstance.transform.InverseTransformPoint(tank.autopilotWeaponController.TargetRigidbody.position);
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;
            weaponRotationControl.Centering = false;
            if (Math.Abs(vector2.x) > 3f)
            {
                tank.autopilotWeaponController.Fire = true;
                weaponRotationControl.Control = 0f;
            }
            else
            {
                tank.autopilotWeaponController.Fire = false;
                weaponRotationControl.Control = -this.ToDiscrete(vector2.x, 0f);
            }
        }

        private void CalculateDirection(AutopilotTankNode tank)
        {
            Vector3 movePosition = tank.navigationData.MovePosition;
            ChassisComponent chassis = tank.chassis;
            chassis.MoveAxis = 0f;
            Vector3 vector2 = tank.rigidbody.Rigidbody.transform.InverseTransformPoint(movePosition);
            if (vector2.z > 0.2)
            {
                chassis.MoveAxis = 1f;
            }
            if (vector2.z < -0.2)
            {
                chassis.MoveAxis = -1f;
            }
        }

        private void CalculateRotation(AutopilotTankNode tank)
        {
            Vector3 movePosition = tank.navigationData.MovePosition;
            ChassisComponent chassis = tank.chassis;
            chassis.TurnAxis = 0f;
            Vector3 vector2 = tank.rigidbody.Rigidbody.transform.InverseTransformPoint(movePosition);
            if (vector2.x > 0.2)
            {
                chassis.TurnAxis = 1f;
            }
            if (vector2.x < -0.2)
            {
                chassis.TurnAxis = -1f;
            }
        }

        private bool CheckAccuracy(AutopilotTankNode tank) => 
            tank.autopilotWeaponController.Accurasy < Random.value;

        private void CheckChassisChange(AutopilotTankNode tank)
        {
            ChassisComponent chassis = tank.chassis;
            if ((chassis.MoveAxis != tank.navigationData.LastMove) || (chassis.TurnAxis != tank.navigationData.LastTurn))
            {
                base.ScheduleEvent<ChassisControlChangedEvent>(tank);
                tank.navigationData.LastMove = chassis.MoveAxis;
                tank.navigationData.LastTurn = chassis.TurnAxis;
            }
        }

        private void CheckIsTargetAchievable(AutopilotTankNode tank, WeaponNode weapon)
        {
            Vector3 position = tank.autopilotWeaponController.TargetRigidbody.position;
            Vector3 origin = weapon.weaponInstance.WeaponInstance.transform.position;
            Vector3 normalized = (position - origin).normalized;
            bool flag = !weapon.targetCollector.Collect(origin, new Vector3(normalized.x, 0f, normalized.z), Vector3.Distance(origin, position), 0).HasTargetHit();
            if (tank.autopilotWeaponController.TragerAchievable != flag)
            {
                tank.autopilotWeaponController.TragerAchievable = flag;
                tank.autopilotWeaponController.OnChange();
            }
        }

        [OnEventComplete]
        public void Control(UpdateEvent e, AutopilotTankNode tank)
        {
            if (tank.rigidbody.Rigidbody && (tank.navigationData.BehavouTree != null))
            {
                Entity target = tank.autopilotMovementController.Target;
                if (!tank.autopilotMovementController.MoveToTarget || (target != null))
                {
                    if (target != null)
                    {
                        if (!target.HasComponent<RigidbodyComponent>())
                        {
                            return;
                        }
                        if (tank.autopilotWeaponController.TargetRigidbody != target.GetComponent<RigidbodyComponent>().Rigidbody)
                        {
                            tank.autopilotWeaponController.TargetRigidbody = target.GetComponent<RigidbodyComponent>().Rigidbody;
                        }
                    }
                    tank.navigationData.BehavouTree.Update();
                    this.CheckChassisChange(tank);
                }
            }
        }

        [OnEventFire]
        public void CreateBehaviourTree(NodeAddedEvent e, AutopilotTankNode tank, [JoinByTank] WeaponNode weapon)
        {
            <CreateBehaviourTree>c__AnonStorey0 storey = new <CreateBehaviourTree>c__AnonStorey0 {
                tank = tank,
                weapon = weapon,
                $this = this
            };
            ConditionNode node2 = new ConditionNode(string.Empty) {
                Condition = new Func<bool>(storey.<>m__0)
            };
            ConditionNode condition = node2;
            ActionNode node4 = new ActionNode {
                Action = new Action(storey.<>m__1)
            };
            ActionNode action = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__2)
            };
            ActionNode node5 = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__3)
            };
            ActionNode node6 = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__4)
            };
            ActionNode node7 = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__5)
            };
            ActionNode node8 = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__6)
            };
            ActionNode node9 = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__7)
            };
            ActionNode node10 = node4;
            node2 = new ConditionNode(string.Empty) {
                Condition = new Func<bool>(storey.<>m__8)
            };
            ConditionNode node11 = node2;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__9)
            };
            ActionNode node12 = node4;
            node2 = new ConditionNode(string.Empty) {
                Condition = new Func<bool>(storey.<>m__A)
            };
            ConditionNode node13 = node2;
            node2 = new ConditionNode(string.Empty) {
                Condition = new Func<bool>(storey.<>m__B)
            };
            ConditionNode node14 = node2;
            node2 = new ConditionNode(string.Empty) {
                Condition = new Func<bool>(storey.<>m__C)
            };
            ConditionNode node15 = node2;
            BehaviourTreeBuilder treePart = new BehaviourTreeBuilder("Drive back when obstacle on critical distance").StartPreconditionSequence().If(node13).ForTime(0.3f).StartParallel().Do(action).Do(node9).End().End();
            BehaviourTreeBuilder builder2 = new BehaviourTreeBuilder("React on tank in front").StartSequence().If(node14).ForTime(0.5f).StartParallel().Do(action).Do(node9).End().ForTime(3f).Do(node5).End();
            BehaviourTreeBuilder builder3 = new BehaviourTreeBuilder("Avoid obstacle").StartPreconditionSequence().If(node15).If(node11).Do(node5).End();
            BehaviourTreeBuilder builder4 = new BehaviourTreeBuilder("Try move to target").StartSelector().ConnectTree(treePart).ConnectTree(builder2).ConnectTree(builder3).Do(node7).End();
            BehaviourTreeBuilder builder5 = new BehaviourTreeBuilder("Enviroment analysis").StartDoOnceIn(0.3f).Do(node12).Do(node10).End();
            BehaviourTreeBuilder builder6 = new BehaviourTreeBuilder("Start movement if allowed").StartPreconditionSequence().If(condition).StartParallel().ConnectTree(builder5).Do(node8).ConnectTree(builder4).End().End();
            BehaviourTreeBuilder builder7 = new BehaviourTreeBuilder("Stop movement").StartParallel().Do(node5).Do(node6).End();
            BehaviourTreeBuilder builder8 = new BehaviourTreeBuilder("movement tree").StartSelector().ConnectTree(builder6).ConnectTree(builder7).End();
            node4 = new ActionNode {
                Action = new Action(storey.<>m__D)
            };
            ActionNode node16 = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__E)
            };
            ActionNode node17 = node4;
            node2 = new ConditionNode(string.Empty) {
                Condition = new Func<bool>(storey.<>m__F)
            };
            ConditionNode node18 = node2;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__10)
            };
            ActionNode node19 = node4;
            node2 = new ConditionNode(string.Empty) {
                Condition = new Func<bool>(storey.<>m__11)
            };
            ConditionNode node20 = node2;
            node2 = new ConditionNode(string.Empty) {
                Condition = new Func<bool>(storey.<>m__12)
            };
            ConditionNode node21 = node2;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__13)
            };
            ActionNode node22 = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__14)
            };
            ActionNode node23 = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__15)
            };
            ActionNode node24 = node4;
            node4 = new ActionNode {
                Action = new Action(storey.<>m__16)
            };
            ActionNode node25 = node4;
            BehaviourTreeBuilder builder9 = new BehaviourTreeBuilder("Missing").StartPreconditionSequence().If(node20).If(node21).Do(node25).End();
            BehaviourTreeBuilder builder10 = new BehaviourTreeBuilder("Rotate and attack").StartSelector().ConnectTree(builder9).StartParallel().Do(node17).Do(node23).End().End();
            BehaviourTreeBuilder builder11 = new BehaviourTreeBuilder("Attack target").StartPreconditionSequence().If(node18).StartParallel().StartDoOnceIn(1f).Do(node19).Do(node22).End().StartDoOnceIn(3f).Do(node24).End().ConnectTree(builder10).End().End();
            BehaviourTreeBuilder builder12 = new BehaviourTreeBuilder("Main targeting tree").StartSelector().ConnectTree(builder11).Do(node16).End();
            BehaviourTreeNode node26 = new BehaviourTreeBuilder("Main tree").StartParallel().ConnectTree(builder8).ConnectTree(builder12).End().Build();
            storey.tank.navigationData.BehavouTree = node26;
        }

        private void FireIfShould(AutopilotTankNode tank)
        {
            tank.autopilotWeaponController.Fire = tank.autopilotWeaponController.IsOnShootingLine;
        }

        [OnEventFire]
        public void FixUid(NodeAddedEvent e, [Combine] UserUidNode uid, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            if (!uid.Entity.Equals(selfUser.Entity))
            {
                bool flag = uid.userUid.Uid.Contains("Deserter ");
                bool flag2 = uid.battleLeaveCounter.NeedGoodBattles > 0;
                if (!flag2 && flag)
                {
                    uid.userUid.Uid = uid.userUid.Uid.Replace("Deserter ", "Survivor ");
                }
                else if (flag2 && !flag)
                {
                    uid.userUid.Uid = uid.userUid.Uid.Insert(0, "Deserter ");
                }
                else if (!flag2)
                {
                    uid.userUid.Uid = uid.userUid.Uid.Insert(0, "Survivor ");
                }
                uid.userUid.Uid = uid.userUid.Uid.Replace("botxz_", string.Empty);
            }
        }

        private void InvertTurnAxisIfReallyMoveBack(AutopilotTankNode tank)
        {
            Vector3 velocity = tank.rigidbody.Rigidbody.velocity;
            if (tank.rigidbody.Rigidbody.transform.InverseTransformDirection(velocity).z <= -1.0)
            {
                tank.chassis.TurnAxis *= -1f;
            }
        }

        private bool IsInShootingRange(AutopilotTankNode tank, WeaponNode weapon)
        {
            Vector3 vector2 = weapon.weaponInstance.WeaponInstance.transform.InverseTransformPoint(tank.autopilotWeaponController.TargetRigidbody.position);
            return ((Math.Abs(vector2.x) < 4f) && (vector2.z > 0f));
        }

        private bool IsOnShootingLine(AutopilotTankNode tank, WeaponNode weapon)
        {
            <IsOnShootingLine>c__AnonStorey1 storey = new <IsOnShootingLine>c__AnonStorey1 {
                target = tank.autopilotMovementController.Target
            };
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            TargetingEvent eventInstance = BattleCache.targetingEvent.GetInstance().Init(targetingData);
            base.ScheduleEvent(eventInstance, weapon);
            return (targetingData.HasTargetHit() && (targetingData.Directions.Any<DirectionData>(new Func<DirectionData, bool>(storey.<>m__0)) && !weapon.Entity.HasComponent<WeaponBlockedComponent>()));
        }

        private void MoveBack(AutopilotTankNode tank)
        {
            tank.chassis.MoveAxis = -1f;
        }

        private void ObstacleAvoidanceRaycasts(AutopilotTankNode tank)
        {
            Transform transform = tank.tankColliders.BoundsCollider.transform;
            BoxCollider boundsCollider = tank.tankColliders.BoundsCollider;
            tank.navigationData.ObstacleOnAvoidanceDistance = false;
            tank.navigationData.ObstacleOnCriticalDistance = false;
            float num = boundsCollider.size.x * 0.5f;
            for (float i = -boundsCollider.size.x * 0.5f; i < num; i += 0.5f)
            {
                RaycastHit hit;
                Vector3 size = boundsCollider.size;
                Vector3 vector5 = boundsCollider.size;
                Vector3 origin = transform.TransformPoint(new Vector3(i, size.y * 0.8f, vector5.z * 0.5f));
                if (Physics.Raycast(new Ray(origin, transform.forward), out hit, 2f, LayerMasks.STATIC) && (Math.Abs(hit.normal.y) < 0.5))
                {
                    tank.navigationData.ObstacleOnAvoidanceDistance = true;
                    if (hit.distance < 0.5)
                    {
                        tank.navigationData.ObstacleOnCriticalDistance = true;
                    }
                    return;
                }
            }
        }

        private void RotateToTarget(AutopilotTankNode tank, WeaponNode weapon)
        {
            Vector3 vector2 = weapon.weaponInstance.WeaponInstance.transform.InverseTransformPoint(tank.autopilotWeaponController.TargetRigidbody.position);
            weapon.weaponRotationControl.Control = this.ToDiscrete(vector2.x, 0f);
        }

        [OnEventFire]
        public void SetTankSync(NodeAddedEvent e, SingleNode<TankAutopilotComponent> autopilot, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            if (session.Entity.Equals(autopilot.component.Session))
            {
                autopilot.Entity.AddComponentIfAbsent<TankSyncComponent>();
            }
        }

        [OnEventFire]
        public void SetTankSync(ChangeAutopilotControllerEvent e, SingleNode<TankAutopilotComponent> autopilot, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            if (session.Entity.Equals(autopilot.component.Session))
            {
                autopilot.Entity.AddComponentIfAbsent<TankSyncComponent>();
            }
            else
            {
                autopilot.Entity.RemoveComponentIfPresent<TankSyncComponent>();
            }
        }

        private void StopFireAndRotation(AutopilotTankNode tank, WeaponNode weapon)
        {
            weapon.weaponRotationControl.Rotation = 0f;
            tank.autopilotWeaponController.Fire = false;
        }

        private void StopMovement(AutopilotTankNode tank)
        {
            tank.chassis.MoveAxis = 0f;
        }

        private void StopRotation(AutopilotTankNode tank)
        {
            tank.chassis.TurnAxis = 0f;
        }

        private void TanksAvoidanceRaycasts(AutopilotTankNode tank)
        {
            Transform transform = tank.tankColliders.BoundsCollider.transform;
            BoxCollider boundsCollider = tank.tankColliders.BoundsCollider;
            tank.navigationData.TankInTheFront = false;
            float num = boundsCollider.size.x * 0.5f;
            for (float i = -boundsCollider.size.x * 0.5f; i < num; i += 0.5f)
            {
                RaycastHit hit;
                Vector3 size = boundsCollider.size;
                Vector3 vector5 = boundsCollider.size;
                Vector3 origin = transform.TransformPoint(new Vector3(i, size.y * 0.8f, vector5.z * 0.5f));
                if (Physics.Raycast(new Ray(origin, transform.forward), out hit, 1f, LayerMasks.TANK_TO_TANK))
                {
                    tank.navigationData.TankInTheFront = true;
                    return;
                }
            }
        }

        private float ToDiscrete(float value, float zeroMinValue) => 
            (value <= zeroMinValue) ? ((value >= -zeroMinValue) ? 0f : -1f) : 1f;

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        [CompilerGenerated]
        private sealed class <CreateBehaviourTree>c__AnonStorey0
        {
            internal TankAutopilotControllerSystem.AutopilotTankNode tank;
            internal TankAutopilotControllerSystem.WeaponNode weapon;
            internal TankAutopilotControllerSystem $this;

            internal bool <>m__0() => 
                this.tank.autopilotMovementController.Moving;

            internal void <>m__1()
            {
                this.$this.MoveBack(this.tank);
            }

            internal void <>m__10()
            {
                this.tank.autopilotWeaponController.ShouldMiss = this.$this.CheckAccuracy(this.tank);
            }

            internal bool <>m__11() => 
                this.tank.autopilotWeaponController.ShouldMiss;

            internal bool <>m__12() => 
                this.$this.IsInShootingRange(this.tank, this.weapon);

            internal void <>m__13()
            {
                this.tank.autopilotWeaponController.IsOnShootingLine = this.$this.IsOnShootingLine(this.tank, this.weapon);
            }

            internal void <>m__14()
            {
                this.$this.FireIfShould(this.tank);
            }

            internal void <>m__15()
            {
                this.$this.CheckIsTargetAchievable(this.tank, this.weapon);
            }

            internal void <>m__16()
            {
                this.$this.ApplyMissBehaviour(this.tank, this.weapon);
            }

            internal void <>m__2()
            {
                this.$this.StopMovement(this.tank);
            }

            internal void <>m__3()
            {
                this.$this.StopRotation(this.tank);
            }

            internal void <>m__4()
            {
                this.$this.CalculateDirection(this.tank);
            }

            internal void <>m__5()
            {
                this.$this.CalculateRotation(this.tank);
            }

            internal void <>m__6()
            {
                this.$this.InvertTurnAxisIfReallyMoveBack(this.tank);
            }

            internal void <>m__7()
            {
                this.$this.ObstacleAvoidanceRaycasts(this.tank);
            }

            internal bool <>m__8() => 
                this.tank.rigidbody.Rigidbody.angularVelocity.magnitude > 0.1;

            internal void <>m__9()
            {
                this.$this.TanksAvoidanceRaycasts(this.tank);
            }

            internal bool <>m__A() => 
                this.tank.navigationData.ObstacleOnCriticalDistance;

            internal bool <>m__B() => 
                this.tank.navigationData.TankInTheFront;

            internal bool <>m__C() => 
                this.tank.navigationData.ObstacleOnAvoidanceDistance;

            internal void <>m__D()
            {
                this.$this.StopFireAndRotation(this.tank, this.weapon);
            }

            internal void <>m__E()
            {
                this.$this.RotateToTarget(this.tank, this.weapon);
            }

            internal bool <>m__F() => 
                this.tank.autopilotWeaponController.Attack;
        }

        [CompilerGenerated]
        private sealed class <IsOnShootingLine>c__AnonStorey1
        {
            internal Entity target;

            internal bool <>m__0(DirectionData direction) => 
                direction.HasTargetHit() && direction.Targets[0].TargetEntity.Equals(this.target);
        }

        public class AutopilotTankNode : Node
        {
            public TankSyncComponent tankSync;
            public TankAutopilotComponent tankAutopilot;
            public TankActiveStateComponent tankActiveState;
            public RigidbodyComponent rigidbody;
            public ChassisComponent chassis;
            public TankCollidersComponent tankColliders;
            public AutopilotMovementControllerComponent autopilotMovementController;
            public AutopilotWeaponControllerComponent autopilotWeaponController;
            public NavigationDataComponent navigationData;
        }

        public class SelfUserReadyToBattleNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class UserUidNode : Node
        {
            public UserUidComponent userUid;
            public BattleLeaveCounterComponent battleLeaveCounter;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
            public WeaponRotationControlComponent weaponRotationControl;
            public WeaponRotationComponent weaponRotation;
            public WeaponGyroscopeRotationComponent weaponGyroscopeRotation;
            public TargetCollectorComponent targetCollector;
        }
    }
}

