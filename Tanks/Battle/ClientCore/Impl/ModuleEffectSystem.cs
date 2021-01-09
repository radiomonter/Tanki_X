namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class ModuleEffectSystem : ECSSystem
    {
        private static readonly float EFFECT_UNITYOBJECT_DELETION_DELAY = 1.5f;
        [CompilerGenerated]
        private static Action<Renderer> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<Collider> <>f__am$cache1;

        [OnEventFire]
        public void CleanPreloadingEffects(NodeRemoveEvent evt, SingleNode<PreloadingModuleEffectsComponent> mapEffect)
        {
            mapEffect.component.EntitiesForEffectsLoading.ForEach(e => base.DeleteEntity(e));
            mapEffect.component.EntitiesForEffectsLoading.Clear();
        }

        [OnEventFire]
        public void Destroy(NodeRemoveEvent e, SingleNode<EffectInstanceComponent> effect)
        {
            base.ScheduleEvent<PrepareDestroyModuleEffectEvent>(effect);
            if (effect.Entity.HasComponent<EffectInstanceRemovableComponent>())
            {
                GameObject gameObject = effect.component.GameObject;
                gameObject.AddComponent<DelayedSelfDestroyBehaviour>().Delay = EFFECT_UNITYOBJECT_DELETION_DELAY;
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = r => r.enabled = false;
                }
                gameObject.GetComponentsInChildren<Renderer>(true).ForEach<Renderer>(<>f__am$cache0);
            }
        }

        [OnEventFire]
        public void Destroy(RemoveEffectEvent e, SingleNode<EffectInstanceComponent> effect)
        {
            effect.component.GameObject.transform.SetParent(null, true);
        }

        [OnEventFire]
        public void PreloadModuleEffects(NodeAddedEvent e, SingleNode<PreloadingModuleEffectsComponent> mapEffect)
        {
            <PreloadModuleEffects>c__AnonStorey0 storey = new <PreloadModuleEffects>c__AnonStorey0 {
                mapEffect = mapEffect,
                $this = this
            };
            PreloadingModuleEffectData[] preloadingModuleEffects = storey.mapEffect.component.PreloadingModuleEffects;
            storey.mapEffect.component.PreloadingBuffer = new Dictionary<string, GameObject>();
            storey.mapEffect.component.EntitiesForEffectsLoading = new List<Entity>();
            preloadingModuleEffects.ForEach<PreloadingModuleEffectData>(new Action<PreloadingModuleEffectData>(storey.<>m__0));
        }

        [OnEventFire]
        public void WarmUpModuleEffects(NodeAddedEvent e, SingleNode<PreloadingModuleEffectsComponent> mapEffect, [Combine] PreloadedModuleEffectNode preloadedModuleEffect)
        {
            Transform preloadedModuleEffectsRoot = mapEffect.component.PreloadedModuleEffectsRoot;
            GameObject obj2 = Object.Instantiate<GameObject>((GameObject) preloadedModuleEffect.resourceData.Data, preloadedModuleEffectsRoot.position, preloadedModuleEffectsRoot.rotation, preloadedModuleEffectsRoot);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate (Collider c) {
                    if (!c.enabled)
                    {
                        c.enabled = true;
                        c.enabled = false;
                    }
                };
            }
            obj2.GetComponentsInChildren<Collider>(true).ForEach<Collider>(<>f__am$cache1);
            obj2.SetActive(false);
            mapEffect.component.PreloadingBuffer.Add(preloadedModuleEffect.preloadingModuleEffectKey.Key, obj2);
            if (mapEffect.component.PreloadingBuffer.Count == mapEffect.component.PreloadingModuleEffects.Length)
            {
                mapEffect.component.EntitiesForEffectsLoading.ForEach(entity => base.DeleteEntity(entity));
                mapEffect.component.EntitiesForEffectsLoading.Clear();
                mapEffect.Entity.AddComponent(new PreloadedModuleEffectsComponent(mapEffect.component.PreloadingBuffer));
            }
        }

        [CompilerGenerated]
        private sealed class <PreloadModuleEffects>c__AnonStorey0
        {
            internal SingleNode<PreloadingModuleEffectsComponent> mapEffect;
            internal ModuleEffectSystem $this;

            internal void <>m__0(PreloadingModuleEffectData i)
            {
                Entity item = this.$this.CreateEntity($"Preloading {i.key}");
                this.mapEffect.component.EntitiesForEffectsLoading.Add(item);
                item.AddComponent(new AssetReferenceComponent(i.asset));
                item.AddComponent<AssetRequestComponent>();
                item.AddComponent(new ModuleEffectSystem.PreloadingModuleEffectKeyComponent(i.key));
            }
        }

        public class PreloadedModuleEffectNode : Node
        {
            public ModuleEffectSystem.PreloadingModuleEffectKeyComponent preloadingModuleEffectKey;
            public ResourceDataComponent resourceData;
        }

        public class PreloadingModuleEffectKeyComponent : Component
        {
            public PreloadingModuleEffectKeyComponent(string key)
            {
                this.Key = key;
            }

            public string Key { get; set; }
        }
    }
}

