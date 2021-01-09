namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class CTFSoundsSystem : ECSSystem
    {
        private const string EFFECT_NAME = "Effects";

        private AudioSource CreateSound(GameObject prefab, GameObject effectRoot)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(prefab);
            obj2.transform.parent = effectRoot.transform;
            return obj2.GetComponent<AudioSource>();
        }

        [OnEventFire]
        public void DestroySounds(NodeRemoveEvent e, MapNode map, [JoinAll] SingleNode<CTFSoundsComponent> listener)
        {
            base.ScheduleEvent<PrepareDestroyCTFSoundsEvent>(listener);
            listener.Entity.RemoveComponent<CTFSoundsComponent>();
        }

        [OnEventFire]
        public void FillFlagSounds(NodeAddedEvent e, SelfBattleUserNode battleUser, [Context, JoinByBattle] CTFNode battle, [Context, JoinByMap] MapNode map, [JoinAll] SingleNode<SoundListenerComponent> listener)
        {
            CTFAssetProxyBehaviour behaviour = ((GameObject) battle.resourceData.Data).GetComponent<CTFAssetProxyBehaviour>();
            CTFSoundsComponent component = new CTFSoundsComponent();
            GameObject effectRoot = new GameObject("Effects") {
                transform = { parent = listener.component.transform }
            };
            component.EffectRoot = effectRoot;
            component.FlagLost = this.CreateSound(behaviour.flagLostSound, effectRoot);
            component.FlagReturn = this.CreateSound(behaviour.flagReturnSound, effectRoot);
            component.FlagStole = this.CreateSound(behaviour.flagStoleSound, effectRoot);
            component.FlagWin = this.CreateSound(behaviour.flagWinSound, effectRoot);
            listener.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void PlayDropSound(FlagDropEvent e, SingleNode<FlagComponent> flag, [JoinAll] SingleNode<CTFSoundsComponent> ctfSoundsNode)
        {
            ctfSoundsNode.component.FlagLost.Play();
        }

        [OnEventFire]
        public void PlayReturnSound(FlagDeliveryEvent e, SingleNode<FlagComponent> flag, [JoinAll] SingleNode<CTFSoundsComponent> ctfSoundsNode)
        {
            ctfSoundsNode.component.FlagWin.Play();
        }

        [OnEventFire]
        public void PlayReturnSound(FlagPickupEvent e, SingleNode<FlagComponent> flag, [JoinAll] SingleNode<CTFSoundsComponent> ctfSoundsNode)
        {
            ctfSoundsNode.component.FlagStole.Play();
        }

        [OnEventFire]
        public void PlayReturnSound(FlagReturnEvent e, SingleNode<FlagComponent> flag, [JoinAll] SingleNode<CTFSoundsComponent> ctfSoundsNode)
        {
            ctfSoundsNode.component.FlagReturn.Play();
        }

        public class CTFNode : Node
        {
            public CTFComponent ctf;
            public ResourceDataComponent resourceData;
            public MapGroupComponent mapGroup;
            public BattleGroupComponent battleGroup;
        }

        public class MapNode : Node
        {
            public MapGroupComponent mapGroup;
            public MapInstanceComponent mapInstance;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public BattleGroupComponent battleGroup;
        }
    }
}

