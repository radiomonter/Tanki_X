namespace Platform.Library.ClientResources.Impl
{
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AssetBundleDatabase
    {
        [SerializeField]
        private List<AssetBundleInfo> bundles = new List<AssetBundleInfo>();
        [SerializeField]
        private List<string> rootGuids;
        private Dictionary<string, AssetInfo> guidToAssetInfo = new Dictionary<string, AssetInfo>();
        private Dictionary<string, AssetBundleInfo> guidToAssetBundleInfo = new Dictionary<string, AssetBundleInfo>();

        public void AddAsset(string assetBundleName, string assetObjectName, string assetGuid, string assetExtension)
        {
            AssetBundleInfo orCreateAssetBundleInfo = this.GetOrCreateAssetBundleInfo(assetBundleName);
            AssetInfo assetInfo = new AssetInfo {
                AssetBundleInfo = orCreateAssetBundleInfo,
                ObjectName = assetObjectName,
                Guid = assetGuid,
                TypeHash = AssetTypeRegistry.GetTypeHashByExtension(assetExtension)
            };
            orCreateAssetBundleInfo.AddAssetInfo(assetInfo);
            this.guidToAssetInfo.Add(assetGuid, assetInfo);
        }

        public void AddDependency(string assetBundleName, string dependencyAssetBundleName)
        {
            int? assetBundleIndex = this.GetAssetBundleIndex(assetBundleName);
            if (assetBundleIndex == null)
            {
                Debug.LogError("Bundle not registred " + assetBundleName);
            }
            else
            {
                AssetBundleInfo info = this.bundles[assetBundleIndex.Value];
                int? nullable2 = this.GetAssetBundleIndex(dependencyAssetBundleName);
                if (nullable2 == null)
                {
                    Debug.Log("Not found dependency assetbundle: " + assetBundleName + " dep:" + dependencyAssetBundleName);
                }
                else
                {
                    AssetBundleInfo item = this.bundles[nullable2.Value];
                    info.Dependencies.Add(item);
                }
            }
        }

        public void Clear()
        {
            this.bundles = new List<AssetBundleInfo>();
            this.rootGuids = null;
            this.guidToAssetInfo = new Dictionary<string, AssetInfo>();
        }

        private AssetBundleInfo CreateAssetBundleInfo(string assetBundleName)
        {
            AssetBundleInfo item = new AssetBundleInfo {
                BundleName = assetBundleName
            };
            this.bundles.Add(item);
            return item;
        }

        public void Deserialize(string data)
        {
            this.Clear();
            JsonUtility.FromJsonOverwrite(data, this);
            this.guidToAssetInfo.Clear();
            foreach (AssetBundleInfo info in this.bundles)
            {
                foreach (AssetInfo info2 in info.Assets)
                {
                    info2.AssetBundleInfo = info;
                    this.guidToAssetInfo.Add(info2.Guid, info2);
                    this.guidToAssetBundleInfo.Add(info2.Guid, info);
                }
            }
            foreach (AssetBundleInfo info3 in this.bundles)
            {
                info3.Dependencies.Clear();
                using (List<string>.Enumerator enumerator4 = info3.DependenciesNames.GetEnumerator())
                {
                    while (enumerator4.MoveNext())
                    {
                        <Deserialize>c__AnonStorey0 storey = new <Deserialize>c__AnonStorey0 {
                            dependencyName = enumerator4.Current
                        };
                        info3.Dependencies.Add(this.bundles.Single<AssetBundleInfo>(new Func<AssetBundleInfo, bool>(storey.<>m__0)));
                    }
                }
            }
        }

        public List<AssetBundleInfo> GetAllAssetBundles() => 
            this.bundles;

        private int? GetAssetBundleIndex(string assetBundleName)
        {
            for (int i = 0; i < this.bundles.Count; i++)
            {
                AssetBundleInfo info = this.bundles[i];
                if (info.BundleName.Equals(assetBundleName))
                {
                    return new int?(i);
                }
            }
            return null;
        }

        public AssetBundleInfo GetAssetBundleInfoByGuid(string guid) => 
            this.guidToAssetBundleInfo[guid];

        public AssetBundleInfo GetAssetBundleInfoByName(string assetBundleName)
        {
            int? assetBundleIndex = this.GetAssetBundleIndex(assetBundleName);
            return ((assetBundleIndex == null) ? null : this.bundles[assetBundleIndex.Value]);
        }

        public AssetInfo GetAssetInfo(string guid)
        {
            AssetInfo info;
            if (!this.guidToAssetInfo.TryGetValue(guid, out info))
            {
                throw new AssetNotFoundException(guid);
            }
            return info;
        }

        private AssetBundleInfo GetOrCreateAssetBundleInfo(string assetBundleName)
        {
            AssetBundleInfo assetBundleInfoByName = this.GetAssetBundleInfoByName(assetBundleName);
            return this.CreateAssetBundleInfo(assetBundleName);
        }

        public List<string> GetRootGuids() => 
            this.rootGuids;

        private bool IsAllDependenciesCached(AssetBundleInfo bundleInfo, string baseUrl)
        {
            <IsAllDependenciesCached>c__AnonStorey1 storey = new <IsAllDependenciesCached>c__AnonStorey1 {
                baseUrl = baseUrl,
                $this = this
            };
            return bundleInfo.AllDependencies.All<AssetBundleInfo>(new Func<AssetBundleInfo, bool>(storey.<>m__0));
        }

        public bool IsAssetRegistered(string guid) => 
            this.guidToAssetInfo.ContainsKey(guid);

        private bool IsBundleCached(AssetBundleInfo bundleInfo, string baseUrl) => 
            Caching.IsVersionCached(AssetBundleNaming.GetAssetBundleUrl(baseUrl, bundleInfo.Filename), bundleInfo.Hash);

        private bool IsCached(string guid, string baseUrl)
        {
            AssetBundleInfo assetBundleInfo = this.GetAssetInfo(guid).AssetBundleInfo;
            return this.IsAllDependenciesCached(assetBundleInfo, baseUrl);
        }

        public string Serialize()
        {
            foreach (AssetBundleInfo info in this.bundles)
            {
                info.DependenciesNames.Clear();
                foreach (AssetBundleInfo info2 in info.Dependencies)
                {
                    info.DependenciesNames.Add(info2.BundleName);
                }
            }
            return JsonUtility.ToJson(this, true);
        }

        public void SetRootGuids(List<string> rootGuids)
        {
            this.rootGuids = rootGuids;
        }

        [CompilerGenerated]
        private sealed class <Deserialize>c__AnonStorey0
        {
            internal string dependencyName;

            internal bool <>m__0(AssetBundleInfo b) => 
                this.dependencyName.Equals(b.BundleName);
        }

        [CompilerGenerated]
        private sealed class <IsAllDependenciesCached>c__AnonStorey1
        {
            internal string baseUrl;
            internal AssetBundleDatabase $this;

            internal bool <>m__0(AssetBundleInfo assetBundleInfo) => 
                this.$this.IsBundleCached(assetBundleInfo, this.baseUrl);
        }
    }
}

