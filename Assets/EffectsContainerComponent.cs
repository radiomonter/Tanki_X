namespace Assets
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class EffectsContainerComponent : BehaviourComponent
    {
        [SerializeField]
        private RectTransform buffContainer;
        [SerializeField]
        private RectTransform debuffContainer;
        [SerializeField]
        private EntityBehaviour effectPrefab;

        public void SpawnBuff(Entity entity)
        {
            this.SpawnEffect(this.buffContainer, entity);
        }

        public void SpawnDebuff(Entity entity)
        {
            this.SpawnEffect(this.debuffContainer, entity);
        }

        private void SpawnEffect(RectTransform container, Entity entity)
        {
            EntityBehaviour behaviour = Instantiate<EntityBehaviour>(this.effectPrefab);
            behaviour.BuildEntity(entity);
            behaviour.transform.SetParent(container, false);
            behaviour.transform.SetAsFirstSibling();
            base.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
        }
    }
}

