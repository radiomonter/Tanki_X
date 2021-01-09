namespace Tanks.Battle.ClientGraphics.API
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class GraffitiDecalSystem : ECSSystem
    {
        private const float SPRAY_DELAY = 5.1f;
        private const float GRAFFITI_DELAY = 2f;
        private const float ADDITIONAL_GUN_LENGTH = 10f;

        private void AddEmitEffect(GraffitiSimpleNode graffiti, BurningTargetBloomComponent effect)
        {
            if (graffiti.dynamicDecalProjector.Emmit)
            {
                Renderer component = graffiti.graffitiInstance.GraffitiDecalObject.GetComponent<Renderer>();
                graffiti.graffitiInstance.EmitRenderer = component;
                effect.burningTargetBloom.targets.Add(component);
            }
        }

        [OnEventFire]
        public void CheckSpraySelf(TimeUpdateEvent e, GraffitiSimpleNode graffiti, [JoinByUser] SingleNode<SelfBattleUserComponent> self, [JoinByUser] SingleNode<TankActiveStateComponent> tank, [JoinByBattle] SingleNode<RoundActiveStateComponent> round)
        {
            if (InputManager.GetActionKeyDown(BattleActions.GRAFFITI) && (graffiti.graffitiAntiSpamTimer.SprayDelay < Time.realtimeSinceStartup))
            {
                base.ScheduleEvent<SprayEvent>(graffiti);
                graffiti.graffitiAntiSpamTimer.SprayDelay = Time.realtimeSinceStartup + 5.1f;
            }
        }

        [OnEventFire]
        public void DestroyGraffiti(NodeRemoveEvent e, GraffitiNode graffitiNode, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode, [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect)
        {
            if (graffitiNode.dynamicDecalProjector.Emmit)
            {
                bloomPostEffect.component.burningTargetBloom.targets.Remove(graffitiNode.graffitiInstance.EmitRenderer);
            }
            decalManagerNode.component.GraffitiDynamicDecalManager.RemoveDecal(graffitiNode.graffitiInstance.GraffitiDecalObject);
        }

        [OnEventFire]
        public void DestroyHolder(NodeRemoveEvent e, [Combine] SingleNode<GraffitiAntiSpamTimerComponent> graffitiHolder)
        {
            Object.Destroy(graffitiHolder.component.gameObject);
        }

        private void DrawEffect(GraffitiVisualEffect prefab, float length, Transform parent, string spriteUid, ItemRarityType rarity)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = prefab.gameObject,
                AutoRecycleTime = length
            };
            base.ScheduleEvent(eventInstance, new EntityStub());
            Transform instance = eventInstance.Instance;
            instance.parent = parent;
            instance.localPosition = Vector3.zero;
            instance.localRotation = Quaternion.identity;
            GraffitiVisualEffect component = instance.GetComponent<GraffitiVisualEffect>();
            component.Image.SpriteUid = spriteUid;
            component.Rarity = rarity;
            component.gameObject.SetActive(true);
        }

        protected GameObject DrawGraffiti(DecalManagerComponent managerComponent, DynamicDecalProjectorComponent decalProjector, Vector3 position, Vector3 direction, Vector3 up)
        {
            DecalProjection decalProjection = new DecalProjection {
                AtlasHTilesCount = decalProjector.AtlasHTilesCount,
                AtlasVTilesCount = decalProjector.AtlasVTilesCount,
                SurfaceAtlasPositions = decalProjector.SurfaceAtlasPositions,
                HalfSize = decalProjector.HalfSize,
                Up = up,
                Distantion = decalProjector.Distance,
                Ray = new Ray(position, direction)
            };
            Mesh mesh = null;
            return (!managerComponent.DecalMeshBuilder.Build(decalProjection, ref mesh) ? null : managerComponent.GraffitiDynamicDecalManager.AddGraffiti(mesh, decalProjector.Material, decalProjector.Color, decalProjector.LifeTime));
        }

        [OnEventFire]
        public void DrawGraffiti(CreateGraffitiEvent e, FirstGraffitiNode graffitiNode, [JoinByUser] SingleNode<SelfBattleUserComponent> self, [JoinByUser] TankWithGraffitiNode tank, [JoinByUser] Optional<SingleNode<PremiumAccountBoostComponent>> premium, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode, [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect)
        {
            GameObject obj2 = this.DrawGraffiti(decalManagerNode.component, graffitiNode.dynamicDecalProjector, e.Position, e.Direction, e.Up);
            if (obj2)
            {
                graffitiNode.graffitiInstance.GraffitiDecalObject = obj2;
                graffitiNode.Entity.AddComponent(new GraffitiDecalComponent(e.Position, e.Direction, e.Up));
                this.PlaySound(graffitiNode.graffitiSound.Sound, e.Position);
                if (graffitiNode.dynamicDecalProjector.Emmit)
                {
                    Renderer component = graffitiNode.graffitiInstance.GraffitiDecalObject.GetComponent<Renderer>();
                    graffitiNode.graffitiInstance.EmitRenderer = component;
                    bloomPostEffect.component.burningTargetBloom.targets.Add(component);
                }
                if (premium.IsPresent())
                {
                    GraffitiVisualEffectComponent graffitiVisualEffect = tank.graffitiVisualEffect;
                    GraffitiVisualEffect visualEffectPrefab = graffitiVisualEffect.VisualEffectPrefab;
                    this.DrawEffect(visualEffectPrefab, graffitiVisualEffect.TimeToDestroy, tank.tankCommonInstance.TankCommonInstance.transform, graffitiNode.imageItem.SpriteUid, graffitiNode.itemRarity.RarityType);
                }
            }
        }

        [OnEventFire]
        public void DrawGraffiti(CreateGraffitiEvent e, GraffitiNode graffitiNode, [JoinByUser] SingleNode<SelfBattleUserComponent> self, [JoinByUser] TankWithGraffitiNode tank, [JoinByUser] Optional<SingleNode<PremiumAccountBoostComponent>> premium, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode, [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect)
        {
            GameObject obj2 = this.DrawGraffiti(decalManagerNode.component, graffitiNode.dynamicDecalProjector, e.Position, e.Direction, e.Up);
            if (obj2)
            {
                graffitiNode.Entity.RemoveComponent(typeof(GraffitiDecalComponent));
                graffitiNode.graffitiInstance.GraffitiDecalObject = obj2;
                graffitiNode.Entity.AddComponent(new GraffitiDecalComponent(e.Position, e.Direction, e.Up));
                this.PlaySound(graffitiNode.graffitiSound.Sound, e.Position);
                this.AddEmitEffect(graffitiNode, bloomPostEffect.component);
                if (premium.IsPresent())
                {
                    GraffitiVisualEffectComponent graffitiVisualEffect = tank.graffitiVisualEffect;
                    GraffitiVisualEffect visualEffectPrefab = graffitiVisualEffect.VisualEffectPrefab;
                    this.DrawEffect(visualEffectPrefab, graffitiVisualEffect.TimeToDestroy, tank.tankCommonInstance.TankCommonInstance.transform, graffitiNode.imageItem.SpriteUid, graffitiNode.itemRarity.RarityType);
                }
            }
        }

        [OnEventFire]
        public void DrawGraffiti(CreateGraffitiEvent e, GraffitiSimpleNode graffitiNode, [JoinByUser] RemoteUserNode user, [JoinByUser] TankWithGraffitiNode tank, [JoinByUser] Optional<SingleNode<PremiumAccountBoostComponent>> premium, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode, [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect)
        {
            GameObject obj2 = this.DrawGraffiti(decalManagerNode.component, graffitiNode.dynamicDecalProjector, e.Position, e.Direction, e.Up);
            if (obj2)
            {
                graffitiNode.graffitiInstance.GraffitiDecalObject = obj2;
                this.PlaySound(graffitiNode.graffitiSound.Sound, e.Position);
                this.AddEmitEffect(graffitiNode, bloomPostEffect.component);
                if (premium.IsPresent())
                {
                    GraffitiVisualEffectComponent graffitiVisualEffect = tank.graffitiVisualEffect;
                    GraffitiVisualEffect visualEffectPrefab = graffitiVisualEffect.VisualEffectPrefab;
                    this.DrawEffect(visualEffectPrefab, graffitiVisualEffect.TimeToDestroy, tank.tankCommonInstance.TankCommonInstance.transform, graffitiNode.imageItem.SpriteUid, graffitiNode.itemRarity.RarityType);
                }
            }
        }

        [OnEventFire]
        public void Init(NodeAddedEvent evt, SelfBattleUserNode battleUser, [Context, JoinByBattle] BattleNode battle, [Context, JoinByMap] MapInstanceNode mapInstance, SingleNode<DecalManagerComponent> managerComponent)
        {
            managerComponent.component.GraffitiDynamicDecalManager = new GraffitiDynamicDecalManager(mapInstance.mapInstance.SceneRoot, battle.userLimit.UserLimit, (float) battle.timeLimit.TimeLimitSec, managerComponent.component.DecalsQueue);
        }

        [OnEventComplete]
        public void InstantiateGraffitiSettings(NodeAddedEvent e, SingleNode<DecalManagerComponent> decalManager, [JoinAll, Combine] GraffitiBattleItemNode graffiti)
        {
            GameObject graffitiInstance = (GameObject) Object.Instantiate(graffiti.resourceData.Data);
            graffitiInstance.AddComponent<GraffitiAntiSpamTimerComponent>();
            graffitiInstance.AddComponent<EntityBehaviour>().BuildEntity(graffiti.Entity);
            graffiti.Entity.AddComponent(new GraffitiInstanceComponent(graffitiInstance));
        }

        [OnEventComplete]
        public void InstantiateGraffitiSettings(NodeAddedEvent e, GraffitiBattleItemNode graffiti, [JoinAll] SingleNode<DecalManagerComponent> mapInstance)
        {
            GameObject graffitiInstance = (GameObject) Object.Instantiate(graffiti.resourceData.Data);
            graffitiInstance.AddComponent<GraffitiAntiSpamTimerComponent>();
            graffitiInstance.AddComponent<EntityBehaviour>().BuildEntity(graffiti.Entity);
            graffiti.Entity.AddComponent(new GraffitiInstanceComponent(graffitiInstance));
        }

        [OnEventFire]
        public void OnRemoteGraffiti(NodeAddedEvent e, GraffitiNode node, [JoinByUser] SingleNode<UserUidComponent> UidNode, [JoinByUser] RemoteUserNode user)
        {
            string uid = UidNode.component.Uid;
            Vector3 sprayPosition = node.graffitiDecal.SprayPosition;
            Vector3 sprayDirection = node.graffitiDecal.SprayDirection;
            Vector3 sprayUpDirection = node.graffitiDecal.SprayUpDirection;
            GraffitiAntiSpamTimerComponent graffitiAntiSpamTimer = node.graffitiAntiSpamTimer;
            if (!graffitiAntiSpamTimer.GraffitiDelayDictionary.ContainsKey(uid))
            {
                base.ScheduleEvent(new CreateGraffitiEvent(sprayPosition, sprayDirection, sprayUpDirection), node.Entity);
                GraffitiAntiSpamTimerComponent.GraffityInfo info2 = new GraffitiAntiSpamTimerComponent.GraffityInfo {
                    Time = Time.realtimeSinceStartup
                };
                graffitiAntiSpamTimer.GraffitiDelayDictionary.Add(uid, info2);
            }
            else
            {
                float realtimeSinceStartup = Time.realtimeSinceStartup;
                GraffitiAntiSpamTimerComponent.GraffityInfo info3 = graffitiAntiSpamTimer.GraffitiDelayDictionary[uid];
                float timeInSec = (info3.Time + 2f) - realtimeSinceStartup;
                if (timeInSec <= 0f)
                {
                    info3.CreateGraffitiEvent = new CreateGraffitiEvent(sprayPosition, sprayDirection, sprayUpDirection);
                    info3.Time = Time.realtimeSinceStartup;
                    base.NewEvent(info3.CreateGraffitiEvent).Attach(node.Entity).Schedule();
                }
                else if (info3.Time > realtimeSinceStartup)
                {
                    info3.CreateGraffitiEvent.Position = sprayPosition;
                    info3.CreateGraffitiEvent.Direction = sprayDirection;
                    info3.CreateGraffitiEvent.Up = sprayUpDirection;
                }
                else
                {
                    info3.CreateGraffitiEvent = new CreateGraffitiEvent(sprayPosition, sprayDirection, sprayUpDirection);
                    info3.Time += 2f;
                    base.NewEvent(info3.CreateGraffitiEvent).Attach(node.Entity).ScheduleDelayed(timeInSec);
                }
            }
        }

        private void PlaySound(AudioSource soundPrefab, Vector3 position)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = soundPrefab.gameObject,
                AutoRecycleTime = soundPrefab.clip.length
            };
            base.ScheduleEvent(eventInstance, new EntityStub());
            Transform instance = eventInstance.Instance;
            instance.position = position;
            AudioSource component = instance.GetComponent<AudioSource>();
            component.gameObject.SetActive(true);
            component.Play();
        }

        [OnEventFire]
        public void Spray(SprayEvent e, SingleNode<GraffitiInstanceComponent> graffitiInstanceNode, [JoinByUser] WeaponNode weaponNode)
        {
            RaycastHit hit;
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance);
            Vector3 worldPosition = accessor.GetWorldPosition();
            Vector3 barrelOriginWorld = accessor.GetBarrelOriginWorld();
            Vector3 dir = worldPosition - barrelOriginWorld;
            if (PhysicsUtil.RaycastWithExclusion(barrelOriginWorld, dir, out hit, (worldPosition - barrelOriginWorld).magnitude + 10f, LayerMasks.VISUAL_STATIC, null))
            {
                base.ScheduleEvent(new CreateGraffitiEvent(barrelOriginWorld, dir, weaponNode.weaponInstance.WeaponInstance.transform.up), graffitiInstanceNode);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public UserLimitComponent userLimit;
            public TimeLimitComponent timeLimit;
            public BattleGroupComponent battleGroup;
            public MapGroupComponent mapGroup;
        }

        [Not(typeof(GraffitiDecalComponent))]
        public class FirstGraffitiNode : Node
        {
            public GraffitiInstanceComponent graffitiInstance;
            public DynamicDecalProjectorComponent dynamicDecalProjector;
            public GraffitiSoundComponent graffitiSound;
            public ImageItemComponent imageItem;
            public ItemRarityComponent itemRarity;
        }

        public class GraffitiBattleItemNode : Node
        {
            public GraffitiBattleItemComponent graffitiBattleItem;
            public ResourceDataComponent resourceData;
        }

        public class GraffitiNode : GraffitiDecalSystem.GraffitiSimpleNode
        {
            public GraffitiDecalComponent graffitiDecal;
        }

        public class GraffitiSimpleNode : Node
        {
            public GraffitiInstanceComponent graffitiInstance;
            public DynamicDecalProjectorComponent dynamicDecalProjector;
            public GraffitiAntiSpamTimerComponent graffitiAntiSpamTimer;
            public GraffitiSoundComponent graffitiSound;
            public ImageItemComponent imageItem;
            public ItemRarityComponent itemRarity;
        }

        public class MapInstanceNode : Node
        {
            public MapInstanceComponent mapInstance;
            public MapGroupComponent mapGroup;
        }

        [Not(typeof(SelfBattleUserComponent))]
        public class RemoteUserNode : Node
        {
            public BattleUserComponent battleUser;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public BattleGroupComponent battleGroup;
        }

        public class TankWithGraffitiNode : Node
        {
            public TankCommonInstanceComponent tankCommonInstance;
            public GraffitiVisualEffectComponent graffitiVisualEffect;
        }

        public class WeaponNode : Node
        {
            public WeaponInstanceComponent weaponInstance;
            public MuzzlePointComponent muzzlePoint;
            public WeaponComponent weapon;
            public TankGroupComponent tankGroup;
        }
    }
}

