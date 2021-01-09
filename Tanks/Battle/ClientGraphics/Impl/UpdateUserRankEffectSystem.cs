namespace Tanks.Battle.ClientGraphics.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class UpdateUserRankEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void PlayUpdateRankEffect(UpdateUserRankEffectEvent evt, ReadyTankNode tank, UserRankNode user, [JoinByUser] BattleUserNode battleUser)
        {
            GameObject effectPrefab = tank.updateUserRankEffect.EffectPrefab;
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = effectPrefab,
                AutoRecycleTime = effectPrefab.GetComponent<UpdateRankEffectSettings>().DestroyTimeDelay
            };
            base.ScheduleEvent(eventInstance, tank);
            Transform instance = eventInstance.Instance;
            GameObject gameObject = instance.gameObject;
            Transform transform = new GameObject("RankEffectRoot").transform;
            transform.parent = tank.tankVisualRoot.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            transform.gameObject.AddComponent<UpdateUserRankTransformBehaviour>().Init();
            instance.parent = transform;
            instance.localPosition = Vector3.zero;
            instance.localRotation = Quaternion.identity;
            instance.localScale = Vector3.one;
            foreach (UpdateRankEffectParticleMovement movement in gameObject.GetComponentsInChildren<UpdateRankEffectParticleMovement>(true))
            {
                movement.parent = transform;
            }
            transform.GetComponentInChildren<UpdateRankEffectSettings>(true).icon.SetRank(user.userRank.Rank);
            gameObject.SetActive(true);
            base.NewEvent<UpdateRankEffectFinishedEvent>().Attach(battleUser).ScheduleDelayed(tank.updateUserRankEffect.FinishEventTime);
            if (!tank.Entity.HasComponent<UpdateUserRankEffectInstantiatedComponent>())
            {
                tank.Entity.AddComponent<UpdateUserRankEffectInstantiatedComponent>();
            }
            tank.Entity.RemoveComponent<UpdateUserRankEffectReadyComponent>();
        }

        [OnEventFire]
        public void PlayUpdateUserRankEffect(UpdateRankEvent e, UserRankNode user, [JoinByUser] NotReadyTankNode tank)
        {
            tank.Entity.AddComponent<UpdateUserRankEffectReadyComponent>();
        }

        [OnEventFire]
        public void ReleaseEffectsOnDeath(NodeRemoveEvent e, TankIncarnationNode tankIncarnation, [JoinByTank] TankWithEffectsNode tank, [JoinAll] SingleNode<MapInstanceComponent> map)
        {
            <ReleaseEffectsOnDeath>c__AnonStorey0 storey = new <ReleaseEffectsOnDeath>c__AnonStorey0 {
                map = map
            };
            tank.tankVisualRoot.GetComponentsInChildren<UpdateUserRankTransformBehaviour>(true).ForEach<UpdateUserRankTransformBehaviour>(new Action<UpdateUserRankTransformBehaviour>(storey.<>m__0));
        }

        private void ScheduleUpdateUserRankEffect(ReadyTankNode tank, UserRankNode user)
        {
            Node[] nodes = new Node[] { tank, user };
            base.NewEvent<UpdateUserRankEffectEvent>().AttachAll(nodes).Schedule();
        }

        [OnEventFire]
        public void ScheduleUpdateUserRankEffect(NodeAddedEvent evt, ActiveTankNode tank, [JoinByUser] UserRankNode user)
        {
            this.ScheduleUpdateUserRankEffect(tank, user);
        }

        [OnEventFire]
        public void ScheduleUpdateUserRankEffect(NodeAddedEvent evt, DeadTankNode tank, [JoinByUser] UserRankNode user)
        {
            this.ScheduleUpdateUserRankEffect(tank, user);
        }

        [OnEventFire]
        public void ScheduleUpdateUserRankEffect(NodeAddedEvent evt, SemiActiveTankNode tank, [JoinByUser] UserRankNode user)
        {
            this.ScheduleUpdateUserRankEffect(tank, user);
        }

        [CompilerGenerated]
        private sealed class <ReleaseEffectsOnDeath>c__AnonStorey0
        {
            internal SingleNode<MapInstanceComponent> map;

            internal void <>m__0(UpdateUserRankTransformBehaviour c)
            {
                c.transform.SetParent(this.map.component.SceneRoot.transform, true);
            }
        }

        public class ActiveTankNode : UpdateUserRankEffectSystem.ReadyTankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class BattleUserNode : Node
        {
            public BattleUserComponent battleUser;
            public UserGroupComponent userGroup;
        }

        public class DeadTankNode : UpdateUserRankEffectSystem.ReadyTankNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        [Not(typeof(UpdateUserRankEffectReadyComponent))]
        public class NotReadyTankNode : UpdateUserRankEffectSystem.TankNode
        {
        }

        public class ReadyTankNode : UpdateUserRankEffectSystem.TankNode
        {
            public UpdateUserRankEffectReadyComponent updateUserRankEffectReady;
        }

        public class SemiActiveTankNode : UpdateUserRankEffectSystem.ReadyTankNode
        {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankVisualRootComponent tankVisualRoot;
            public UpdateUserRankEffectComponent updateUserRankEffect;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public UserGroupComponent userGroup;
            public TankGroupComponent tankGroup;
        }

        public class TankWithEffectsNode : UpdateUserRankEffectSystem.TankNode
        {
            public UpdateUserRankEffectInstantiatedComponent updateUserRankEffectInstantiated;
        }

        public class UserRankNode : Node
        {
            public UserComponent user;
            public UserRankComponent userRank;
            public UserGroupComponent userGroup;
        }
    }
}

