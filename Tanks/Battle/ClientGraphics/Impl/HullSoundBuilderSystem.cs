namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics;
    using UnityEngine;

    public class HullSoundBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateCommonTankSounds(NodeAddedEvent evt, TankNode tank)
        {
            Transform soundRoot = this.CreateTankSoundRoot(tank);
            this.CreateTankExplosionSound(tank, soundRoot);
        }

        private void CreateTankExplosionSound(TankNode tank, Transform soundRoot)
        {
            GameObject obj3 = Object.Instantiate<GameObject>(tank.tankDeathExplosionPrefabs.SoundPrefab);
            Transform transform = obj3.transform;
            transform.SetParent(soundRoot);
            transform.localPosition = Vector3.zero;
            tank.Entity.AddComponent(new TankExplosionSoundComponent(obj3.GetComponent<AudioSource>()));
        }

        private Transform CreateTankSoundRoot(TankNode tank)
        {
            Transform transform = tank.tankVisualRoot.transform.gameObject.GetComponentsInChildren<TankSoundRootBehaviour>(true)[0].gameObject.transform;
            tank.Entity.AddComponent(new TankSoundRootComponent(transform));
            return transform;
        }

        public class TankNode : Node
        {
            public TankVisualRootComponent tankVisualRoot;
            public TankCommonInstanceComponent tankCommonInstance;
            public TankDeathExplosionPrefabsComponent tankDeathExplosionPrefabs;
            public AssembledTankComponent assembledTank;
        }
    }
}

