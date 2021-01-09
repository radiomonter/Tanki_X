namespace Platform.Library.ClientResources.API
{
    using Platform.Library.ClientResources.Impl;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class AssetBundlesStorage
    {
        public static int STORAGE_PREFARE_SIZE = 0x6400000;
        public static int EXPIRATION_TIME_SEC = 60;
        private static LinkedList<AssetBundleStorageEntry> entryQueue = new LinkedList<AssetBundleStorageEntry>();
        private static Dictionary<AssetBundleInfo, AssetBundleStorageEntry> bundle2entry = new Dictionary<AssetBundleInfo, AssetBundleStorageEntry>();
        private static HashSet<AssetBundleInfo> loadingBundles = new HashSet<AssetBundleInfo>();
        private static int size;

        private static AssetBundleStorageEntry Access(AssetBundleInfo info)
        {
            AssetBundleStorageEntry entry = bundle2entry[info];
            entry.LastAccessTime = Time.time;
            return entry;
        }

        public static void Clean()
        {
            foreach (AssetBundleStorageEntry entry in entryQueue)
            {
                if (entry.Bundle != null)
                {
                    entry.Bundle.Unload(true);
                }
            }
            InternalClean();
        }

        public static AssetBundle Get(AssetBundleInfo info) => 
            Access(info).Bundle;

        public static void InternalClean()
        {
            entryQueue.Clear();
            bundle2entry.Clear();
            loadingBundles.Clear();
            size = 0;
        }

        public static bool IsExpired(AssetBundleStorageEntry entry) => 
            (Time.time - entry.LastAccessTime) > EXPIRATION_TIME_SEC;

        public static bool IsLoading(AssetBundleInfo info) => 
            loadingBundles.Contains(info);

        public static bool IsNeedFreeSpace() => 
            size > STORAGE_PREFARE_SIZE;

        public static bool IsStored(AssetBundleInfo info)
        {
            if (!bundle2entry.ContainsKey(info))
            {
                return false;
            }
            Access(info);
            return true;
        }

        public static void MarkLoading(AssetBundleInfo info)
        {
            loadingBundles.Add(info);
        }

        public static void Refresh(AssetBundleInfo info)
        {
            IsStored(info);
        }

        public static void Store(AssetBundleInfo info, AssetBundle bundle)
        {
            loadingBundles.Remove(info);
            AssetBundleStorageEntry entry = new AssetBundleStorageEntry {
                Info = info,
                Bundle = bundle,
                LastAccessTime = Time.time
            };
            entryQueue.AddLast(entry);
            bundle2entry.Add(info, entry);
            size += info.Size;
        }

        public static void StoreAsStatic(AssetBundleInfo info, AssetBundle bundle)
        {
            AssetBundleStorageEntry entry = new AssetBundleStorageEntry {
                Info = info,
                Bundle = bundle,
                LastAccessTime = Time.time
            };
            bundle2entry.Add(info, entry);
        }

        public static bool Unload(AssetBundleStorageEntry entry)
        {
            entry.Bundle.Unload(false);
            entryQueue.Remove(entry);
            bundle2entry.Remove(entry.Info);
            size -= entry.Info.Size;
            return true;
        }

        public static LinkedList<AssetBundleStorageEntry> EntryQueue =>
            entryQueue;

        public static int Size =>
            size;
    }
}

