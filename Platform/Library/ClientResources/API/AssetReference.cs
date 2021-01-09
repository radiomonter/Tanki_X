namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Serializable]
    public class AssetReference
    {
        public static readonly string GUID_FIELD_SERIALIZED_NAME = "assetGuid";
        [SerializeField]
        private string assetGuid;
        [SerializeField]
        private Object embededReference;
        private Object hardReference;
        public Action<Object> OnLoaded;

        public AssetReference()
        {
        }

        public AssetReference(string assetGuid) : this()
        {
            this.assetGuid = assetGuid;
        }

        public override bool Equals(object obj) => 
            this.assetGuid == ((AssetReference) obj).assetGuid;

        public override int GetHashCode() => 
            this.assetGuid.GetHashCode();

        public void Load()
        {
            this.Load(0);
        }

        public void Load(int priority)
        {
            Entity entity = EngineService.Engine.CreateEntity("Load " + this.assetGuid);
            entity.AddComponent(new AssetReferenceComponent(this));
            entity.AddComponent(new AssetRequestComponent(priority));
        }

        public void SetReference(Object reference)
        {
            this.hardReference = reference;
            if (this.OnLoaded != null)
            {
                this.OnLoaded(reference);
            }
        }

        public override string ToString() => 
            "AssetReference [assetGuid=" + this.assetGuid + "]";

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public bool IsDefined =>
            !string.IsNullOrEmpty(this.assetGuid);

        public string AssetGuid
        {
            get => 
                this.assetGuid;
            set
            {
                if (this.assetGuid != value)
                {
                    this.assetGuid = value;
                    this.hardReference = null;
                    this.embededReference = null;
                }
            }
        }

        public Object Reference =>
            (this.embededReference == null) ? this.hardReference : this.embededReference;
    }
}

