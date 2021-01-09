namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class AssetBundleDiskCacheSystem : ECSSystem
    {
        private bool EnoughFreeSpaceForCache(AssetBundleDiskCache cache)
        {
            long num = cache.RequiredFreeSpaceInBytes();
            Console.WriteLine("AssetBundleDiskCache: requiredFreeSpace: " + num + " bytes.");
            int num2 = (int) (num / 0x12c00000L);
            long num3 = (num - (0x12c00000L * num2)) + 0x3200000L;
            num2++;
            List<FileStream> list = new List<FileStream>();
            try
            {
                for (int i = 0; i < num2; i++)
                {
                    string testFileName = this.GetTestFileName(cache.CachePath, i);
                    list.Add(File.Open(testFileName, FileMode.OpenOrCreate));
                }
            }
            catch (Exception)
            {
                return true;
            }
            bool flag2 = true;
            try
            {
                int num5 = 0;
                while (true)
                {
                    if (num5 >= (num2 - 1))
                    {
                        list[num2 - 1].SetLength(num3);
                        break;
                    }
                    list[num5].SetLength(0x12c00000L);
                    num5++;
                }
            }
            catch (IOException)
            {
                flag2 = false;
            }
            finally
            {
                for (int i = 0; i < num2; i++)
                {
                    list[i].Close();
                    File.Delete(this.GetTestFileName(cache.CachePath, i));
                }
            }
            return flag2;
        }

        private string GetTestFileName(string cachePath, int index) => 
            cachePath + "/testFreeSpace.test" + index;

        private void HandleError(Entity entity, AssetBundleDiskCacheRequest request, string errorMessage)
        {
            if ((request.Progress > 0f) && (request.Progress < 1f))
            {
                this.SheduleErrorEvent<ServerDisconnectedEvent>(entity, errorMessage);
            }
            else
            {
                this.SheduleErrorEvent<NoServerConnectionEvent>(entity, errorMessage);
            }
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<AssetBundleDatabaseComponent> dbNode, SingleNode<BaseUrlComponent> baseUrlNode)
        {
            bool flag;
            AssetBundleDiskCache cache = new AssetBundleDiskCache(dbNode.component.AssetBundleDatabase, baseUrlNode.component.Url);
            try
            {
                cache.CleanOldBundlesCache();
                flag = this.EnoughFreeSpaceForCache(cache);
            }
            catch (Exception exception)
            {
                this.SheduleErrorEvent<InvalidGameDataErrorEvent>(dbNode.Entity, "AssetBundleDiskCacheSystem: " + exception.Message);
                return;
            }
            if (!flag)
            {
                this.SheduleErrorEvent<NotEnoughDiskSpaceForCacheErrorEvent>(dbNode.Entity, "Not enough disk space for cache");
            }
            else
            {
                AssetBundleDiskCacheComponent component = new AssetBundleDiskCacheComponent {
                    AssetBundleDiskCache = cache
                };
                baseUrlNode.Entity.AddComponent(component);
            }
        }

        private void SheduleErrorEvent<T>(Entity entity, string errorMessage) where T: Event, new()
        {
            base.Log.ErrorFormat("AssetBundleDiskCacheSystem Error: {0}", errorMessage);
            base.ScheduleEvent<T>(entity);
        }

        [OnEventFire]
        public void StartLoad(NodeAddedEvent e, SingleNode<AssetBundleLoadingComponent> loadingNode, [JoinAll] SingleNode<AssetBundleDiskCacheComponent> cacheNode)
        {
            AssetBundleLoadingComponent component = loadingNode.component;
            component.AssetBundleDiskCacheRequest = cacheNode.component.AssetBundleDiskCache.GetFromCacheOrDownload(component.Info);
            component.StartTime = Time.realtimeSinceStartup;
            base.Log.InfoFormat("LoadStart {0}", component.AssetBundleDiskCacheRequest.AssetBundleInfo.Filename);
        }

        [OnEventFire]
        public void Update(UpdateEvent e, SingleNode<AssetBundleDiskCacheComponent> cacheNode, [JoinAll] ICollection<SingleNode<AssetBundleLoadingComponent>> loadingBundleNodes)
        {
            cacheNode.component.AssetBundleDiskCache.Update();
            foreach (SingleNode<AssetBundleLoadingComponent> node in loadingBundleNodes)
            {
                AssetBundleDiskCacheRequest assetBundleDiskCacheRequest = node.component.AssetBundleDiskCacheRequest;
                if (assetBundleDiskCacheRequest.IsDone)
                {
                    base.Log.InfoFormat("LoadComplete {0}", assetBundleDiskCacheRequest.AssetBundleInfo.Filename);
                    if (!string.IsNullOrEmpty(assetBundleDiskCacheRequest.Error))
                    {
                        this.HandleError(node.Entity, assetBundleDiskCacheRequest, assetBundleDiskCacheRequest.Error);
                        break;
                    }
                    base.ScheduleEvent<LoadCompleteEvent>(node.Entity);
                }
            }
        }
    }
}

