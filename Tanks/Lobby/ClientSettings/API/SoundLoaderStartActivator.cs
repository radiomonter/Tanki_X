namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine.Serialization;

    public class SoundLoaderStartActivator : UnityAwareActivator<AutoCompleting>
    {
        [FormerlySerializedAs("sceneListRef")]
        public AssetReference audioResourcesRef;
        public EntityBehaviour entity;

        protected override void Activate()
        {
            this.entity.Entity.AddComponent(new AssetReferenceComponent(this.audioResourcesRef));
            AssetRequestComponent component = new AssetRequestComponent {
                AssetStoreLevel = AssetStoreLevel.MANAGED
            };
            this.entity.Entity.AddComponent(component);
        }
    }
}

