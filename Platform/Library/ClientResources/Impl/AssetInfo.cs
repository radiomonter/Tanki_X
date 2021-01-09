namespace Platform.Library.ClientResources.Impl
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AssetInfo
    {
        [SerializeField]
        private string guid;
        [SerializeField]
        private string objectName;
        [SerializeField]
        private int typeHash;
        [NonSerialized]
        private Platform.Library.ClientResources.Impl.AssetBundleInfo assetBundleInfo;

        public override string ToString() => 
            $"[AssetInfo: guid={this.guid}, objectName={this.objectName}, Type={this.AssetType}, assetBundleName={this.assetBundleInfo.BundleName}]";

        public Type AssetType =>
            AssetTypeRegistry.GetType(this.typeHash);

        public string Guid
        {
            get => 
                this.guid;
            set => 
                this.guid = value;
        }

        public string ObjectName
        {
            get => 
                this.objectName;
            set => 
                this.objectName = value;
        }

        public Platform.Library.ClientResources.Impl.AssetBundleInfo AssetBundleInfo
        {
            get => 
                this.assetBundleInfo;
            set => 
                this.assetBundleInfo = value;
        }

        internal int TypeHash
        {
            get => 
                this.typeHash;
            set => 
                this.typeHash = value;
        }
    }
}

