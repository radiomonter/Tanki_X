namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PreloadingModuleEffectsComponent : BehaviourComponent
    {
        [SerializeField]
        private Transform preloadedModuleEffectsRoot;
        [SerializeField]
        private PreloadingModuleEffectData[] preloadingModuleEffects;

        public Transform PreloadedModuleEffectsRoot =>
            this.preloadedModuleEffectsRoot;

        public PreloadingModuleEffectData[] PreloadingModuleEffects =>
            this.preloadingModuleEffects;

        public Dictionary<string, GameObject> PreloadingBuffer { get; set; }

        public List<Entity> EntitiesForEffectsLoading { get; set; }
    }
}

