namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AssetStorageComponent : Component
    {
        public AssetStorageComponent()
        {
            this.ManagedReferencies = new Dictionary<string, ResourceStorageEntry>();
            this.StaticReferencies = new Dictionary<string, Object>();
        }

        public bool Contains(string guid)
        {
            if (!this.StaticReferencies.ContainsKey(guid))
            {
                ResourceStorageEntry entry;
                if (!this.ManagedReferencies.TryGetValue(guid, out entry))
                {
                    return false;
                }
                entry.LastAccessTime = Time.time;
            }
            return true;
        }

        public Object Get(string guid)
        {
            Object obj2;
            ResourceStorageEntry entry;
            if (this.StaticReferencies.TryGetValue(guid, out obj2))
            {
                return obj2;
            }
            if (!this.ManagedReferencies.TryGetValue(guid, out entry))
            {
                throw new ResourceNotInStorageException(guid);
            }
            entry.LastAccessTime = Time.time;
            return entry.Asset;
        }

        public void Put(string guid, Object asset, AssetStoreLevel level)
        {
            if (level == AssetStoreLevel.STATIC)
            {
                if (!this.StaticReferencies.ContainsKey(guid))
                {
                    this.StaticReferencies.Add(guid, asset);
                }
            }
            else if ((level == AssetStoreLevel.MANAGED) && !this.ManagedReferencies.ContainsKey(guid))
            {
                ResourceStorageEntry entry = new ResourceStorageEntry {
                    Asset = asset,
                    LastAccessTime = Time.time
                };
                this.ManagedReferencies.Add(guid, entry);
            }
        }

        public void Remove(string guid, AssetStoreLevel level)
        {
            if (level == AssetStoreLevel.STATIC)
            {
                this.ManagedReferencies.Remove(guid);
                this.StaticReferencies.Remove(guid);
            }
            else if (level == AssetStoreLevel.MANAGED)
            {
                this.ManagedReferencies.Remove(guid);
            }
        }

        public Dictionary<string, ResourceStorageEntry> ManagedReferencies { get; set; }

        public Dictionary<string, Object> StaticReferencies { get; set; }
    }
}

