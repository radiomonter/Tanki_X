namespace Platform.Library.ClientResources.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class AssetBundleInfo
    {
        [SerializeField]
        private string bundleName;
        [SerializeField]
        private string hash;
        [SerializeField]
        private uint crc;
        [SerializeField]
        private uint cacheCrc;
        [SerializeField]
        private int size;
        [SerializeField]
        private List<string> dependenciesNames = new List<string>();
        [SerializeField]
        private List<AssetInfo> assets = new List<AssetInfo>();
        [SerializeField]
        private int modificationHash;
        [NonSerialized]
        private List<AssetBundleInfo> dependencies = new List<AssetBundleInfo>();
        [NonSerialized]
        private bool isCached;

        internal void AddAssetInfo(AssetInfo assetInfo)
        {
            this.Assets.Add(assetInfo);
        }

        private void CollectDependencies(ICollection<AssetBundleInfo> collector)
        {
            foreach (AssetBundleInfo info in this.Dependencies)
            {
                collector.Add(info);
            }
        }

        public override bool Equals(object obj)
        {
            AssetBundleInfo info = obj as AssetBundleInfo;
            return ((info != null) ? (this.Filename == info.Filename) : false);
        }

        public override int GetHashCode() => 
            this.Filename.GetHashCode();

        public override string ToString() => 
            $"[AssetBundleInfo: bundleName={this.bundleName}, hash={this.hash}, size={this.size}, dependenciesNames={this.dependenciesNames}, assets={this.assets}]";

        public bool IsCached
        {
            get => 
                this.isCached;
            set => 
                this.isCached = value;
        }

        public string BundleName
        {
            get => 
                this.bundleName;
            set => 
                this.bundleName = value;
        }

        public Hash128 Hash
        {
            get => 
                Hash128.Parse(this.hash);
            set => 
                this.hash = value.ToString();
        }

        public string HashString =>
            this.hash;

        public int ModificationHash
        {
            get => 
                this.modificationHash;
            set => 
                this.modificationHash = value;
        }

        public int Size
        {
            get => 
                this.size;
            set => 
                this.size = value;
        }

        public List<AssetInfo> Assets
        {
            get => 
                this.assets;
            set => 
                this.assets = value;
        }

        public List<string> DependenciesNames
        {
            get => 
                this.dependenciesNames;
            set => 
                this.dependenciesNames = value;
        }

        public List<AssetBundleInfo> Dependencies
        {
            get => 
                this.dependencies;
            set => 
                this.dependencies = value;
        }

        public ICollection<AssetBundleInfo> AllDependencies
        {
            get
            {
                List<AssetBundleInfo> collector = new List<AssetBundleInfo>();
                this.CollectDependencies(collector);
                return collector;
            }
        }

        public string Filename =>
            AssetBundleNaming.AddCrcToBundleName(this.BundleName, this.CRC);

        public uint CRC
        {
            get => 
                this.crc;
            set => 
                this.crc = value;
        }

        public uint CacheCRC
        {
            get => 
                this.cacheCrc;
            set => 
                this.cacheCrc = value;
        }
    }
}

