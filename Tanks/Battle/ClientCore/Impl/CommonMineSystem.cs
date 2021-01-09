namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class CommonMineSystem : ECSSystem
    {
        private static readonly float TANK_MINE_RAYCAST_DISTANCE = 10000f;

        [OnEventFire]
        public void InitMinePlacingTransform(InitMinePlacingTransformEvent e, SingleNode<MineConfigComponent> mine, SingleNode<MapInstanceComponent> map)
        {
            if (!mine.Entity.HasComponent<MinePlacingTransformComponent>())
            {
                RaycastHit hit;
                MinePlacingTransformComponent component = new MinePlacingTransformComponent();
                if (!Physics.Raycast(e.Position + (Vector3.up * 3f), Vector3.down, out hit, TANK_MINE_RAYCAST_DISTANCE, LayerMasks.STATIC))
                {
                    component.HasPlacingTransform = false;
                }
                else
                {
                    component.PlacingData = hit;
                    component.HasPlacingTransform = true;
                }
                mine.Entity.AddComponent(component);
            }
        }

        [OnEventFire]
        public void Instantiate(NodeAddedEvent e, [Combine] AnyMineNode mine, [JoinByUser] Optional<MountedGraffityItemNode> graffity, [JoinByUser] Optional<SingleNode<UserAvatarComponent>> avatar, SingleNode<MapInstanceComponent> map, SingleNode<PreloadedModuleEffectsComponent> mapEffect)
        {
            string key = mine.preloadingMineKey.Key;
            if (avatar.IsPresent())
            {
                key = this.TryCrutchRemapByAvatar(key, avatar.Get().component.Id);
            }
            if (graffity.IsPresent())
            {
                key = this.TryCrutchRemapByGraffiti(key, graffity.Get());
            }
            GameObject original = mapEffect.component.PreloadedEffects[key];
            if (original)
            {
                GameObject gameObject = Object.Instantiate<GameObject>(original, null);
                gameObject.SetActive(true);
                mine.Entity.AddComponent(new EffectInstanceComponent(gameObject));
                gameObject.GetComponent<EntityBehaviour>().BuildEntity(mine.Entity);
            }
        }

        private string TryCrutchRemapByAvatar(string existingKey, string avatarId) => 
            ((existingKey != "spider") || (avatarId != "457e8f5f-953a-424c-bd97-67d9e116ab7a")) ? (((existingKey != "mine") || (avatarId != "457e8f5f-953a-424c-bd97-67d9e116ab7a")) ? existingKey : "mineHolo") : "spiderHolo";

        private string TryCrutchRemapByGraffiti(string existingKey, MountedGraffityItemNode graffiti) => 
            ((existingKey != "spider") || (graffiti.assetReference.Reference.AssetGuid != "7997b10cf40900d4f968f6d04619723d")) ? existingKey : "hellSpider";

        [OnEventFire]
        public void TryExplosion(MineTryExplosionEvent evt, AnyActiveMineInstantiatedNode mine, [JoinByTank] SingleNode<SelfTankComponent> tank)
        {
            MineUtil.ExecuteSplashExplosion(mine.Entity, tank.Entity, mine.effectInstance.GameObject.transform.position);
        }

        public class AnyActiveMineInstantiatedNode : CommonMineSystem.AnyMineNode
        {
            public EffectActiveComponent effectActive;
            public EffectInstanceComponent effectInstance;
        }

        public class AnyMineNode : Node
        {
            public TankGroupComponent tankGroup;
            public PreloadingMineKeyComponent preloadingMineKey;
            public MineConfigComponent mineConfig;
            public UserGroupComponent userGroup;
        }

        public class MountedGraffityItemNode : Node
        {
            public GraffitiBattleItemComponent graffitiBattleItem;
            public AssetReferenceComponent assetReference;
        }
    }
}

