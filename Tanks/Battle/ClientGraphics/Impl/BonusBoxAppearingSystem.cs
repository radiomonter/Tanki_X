namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class BonusBoxAppearingSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateBrokenBonusBox(BonusTakenEvent e, BonusWithResourceNode bonus, [JoinAll] SingleNode<BonusClientConfigComponent> bonusConfig)
        {
            Transform transform = bonus.bonusBoxInstance.BonusBoxInstance.transform;
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = bonus.bonusBoxInstance.BonusBoxInstance.GetComponent<BrokenBonusBoxBehavior>().BrokenBonusGameObject
            };
            base.ScheduleEvent(eventInstance, bonus);
            Transform instance = eventInstance.Instance;
            GameObject gameObject = instance.gameObject;
            instance.position = transform.position;
            instance.rotation = transform.rotation;
            Material[] allMaterials = MaterialAlphaUtils.GetAllMaterials(gameObject);
            allMaterials.SetOverrideTag("RenderType", "Transparent");
            Entity entity = base.CreateEntity("brokenBonusBox");
            entity.AddComponent(new MaterialArrayComponent(allMaterials));
            entity.AddComponent(new BrokenBonusBoxInstanceComponent(gameObject));
            entity.AddComponent<BonusTakingStateComponent>();
            entity.AddComponent(new LocalDurationComponent(bonusConfig.component.disappearingDuration));
            gameObject.SetActive(true);
        }

        [OnEventFire]
        public void SetFullOpacity(NodeRemoveEvent e, BonusBoxSpawnAppearingNode node)
        {
            node.material.Material.SetFullOpacity();
        }

        [OnEventFire]
        public void SetFullTransparent(NodeAddedEvent e, BonusBoxSpawnAppearingNode node)
        {
            node.material.Material.SetFullTransparent();
        }

        [OnEventFire]
        public void UpdateBonusBoxAlpha(TimeUpdateEvent e, BonusBoxSpawnAppearingNode node)
        {
            float progress = Date.Now.GetProgress(node.bonusDropTime.DropTime, node.bonusConfig.SpawnDuration);
            node.material.Material.SetAlpha(progress);
        }

        public class BonusBoxSpawnAppearingNode : Node
        {
            public BonusSpawnStateComponent bonusSpawnState;
            public BonusComponent bonus;
            public BonusDropTimeComponent bonusDropTime;
            public BonusConfigComponent bonusConfig;
            public BonusBoxInstanceComponent bonusBoxInstance;
            public MaterialComponent material;
        }

        public class BonusWithResourceNode : Node
        {
            public BonusBoxInstanceComponent bonusBoxInstance;
        }
    }
}

