namespace Platform.Library.ClientResources.Impl
{
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class AssetBundleDiskCache : IDisposable
    {
        public static int SIZE_BEFORE_SYSTEM_GC = 0x3200000;
        public static readonly string CACHE_DIRECTORY = "AssetBundlesCache";
        public static AssetBundleDiskCache INSTANCE;
        public static readonly int MINIMUM_PROJECT_BUNDLES = 100;
        private Thread diskWriterThread;
        private DiskCacheWriterThread diskWriter;
        private LinkedList<AssetBundleDiskCacheTask> tasks = new LinkedList<AssetBundleDiskCacheTask>();
        [CompilerGenerated]
        private static Func<string, bool> <>f__am$cache0;

        public AssetBundleDiskCache(AssetBundleDatabase dataBase, string baseUrl)
        {
            this.DataBase = dataBase;
            this.BaseUrl = baseUrl;
            this.CachePath = this.ResolveAssetBundlesCachePath();
            this.diskWriter = new DiskCacheWriterThread();
            this.diskWriterThread = new Thread(new ThreadStart(this.diskWriter.Run));
            this.diskWriterThread.Start();
            INSTANCE = this;
        }

        public static void Clean()
        {
            if (INSTANCE != null)
            {
                INSTANCE.Dispose();
            }
        }

        public bool CleanCache(AssetBundleInfo info)
        {
            try
            {
                string assetBundleCachePath = this.GetAssetBundleCachePath(info);
                File.SetAttributes(assetBundleCachePath, FileAttributes.Archive);
                File.Delete(assetBundleCachePath);
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public void CleanOldBundlesCache()
        {
            HashSet<string> currentVersionBundleNames = this.GetCurrentVersionBundleNames();
            if (currentVersionBundleNames.Count < MINIMUM_PROJECT_BUNDLES)
            {
                Console.WriteLine("AssetBundle database looks incorrect, skip cleaning old bundles");
            }
            else
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = bundleName => bundleName.Contains(".bundle");
                }
                foreach (string str in FileUtils.GetFiles(this.CachePath, <>f__am$cache0))
                {
                    string fileName = Path.GetFileName(str);
                    if (!currentVersionBundleNames.Contains(fileName))
                    {
                        try
                        {
                            File.SetAttributes(str, FileAttributes.Archive);
                            File.Delete(str);
                        }
                        catch (IOException exception1)
                        {
                            LoggerProvider.GetLogger<AssetBundleDiskCache>().ErrorFormat("Can't delete old bundle {0}, IOException: {1}", fileName, exception1.Message);
                        }
                        catch (UnauthorizedAccessException exception3)
                        {
                            LoggerProvider.GetLogger<AssetBundleDiskCache>().ErrorFormat("Can't delete old bundle {0}, UnauthorizedAccessException: {1}", fileName, exception3.Message);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            if (this.diskWriter != null)
            {
                this.diskWriter.Stop();
            }
            foreach (AssetBundleDiskCacheTask task in this.tasks)
            {
                task.Dispose();
            }
            this.tasks.Clear();
        }

        ~AssetBundleDiskCache()
        {
            this.Dispose();
        }

        public string GetAssetBundleCachePath(AssetBundleInfo info) => 
            $"{this.CachePath}/{info.Filename}";

        public HashSet<string> GetCurrentVersionBundleNames()
        {
            HashSet<string> set = new HashSet<string>();
            foreach (AssetBundleInfo info in this.DataBase.GetAllAssetBundles())
            {
                set.Add(info.Filename);
            }
            return set;
        }

        public AssetBundleDiskCacheRequest GetFromCacheOrDownload(AssetBundleInfo info)
        {
            AssetBundleDiskCacheRequest request = new AssetBundleDiskCacheRequest(info, true);
            this.tasks.AddLast(new AssetBundleDiskCacheTask(this).Init(request));
            return request;
        }

        public bool IsCached(AssetBundleInfo info) => 
            File.Exists(this.GetAssetBundleCachePath(info));

        public long RequiredFreeSpaceInBytes()
        {
            long num = 0L;
            foreach (AssetBundleInfo info in this.DataBase.GetAllAssetBundles())
            {
                if (!this.IsCached(info))
                {
                    num += info.Size;
                }
            }
            return num;
        }

        public string ResolveAssetBundlesCachePath()
        {
            string dataPath = Application.dataPath;
            string name = BuildTargetName.GetName();
            string path = $"{dataPath}/{CACHE_DIRECTORY}/{name}/";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string str4 = $"{path}{Random.Range(0, 0x7fffffff):x8}.test";
                using (new FileStream(str4, FileMode.OpenOrCreate))
                {
                }
                File.Delete(str4);
            }
            catch
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                path = $"{folderPath}/TankiX/tankix_Data/{CACHE_DIRECTORY}/{name}/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            return path;
        }

        public void Update()
        {
            LinkedListNode<AssetBundleDiskCacheTask> next;
            for (LinkedListNode<AssetBundleDiskCacheTask> node = this.tasks.First; node != null; node = next)
            {
                next = node.Next;
                AssetBundleDiskCacheTask task = node.Value;
                if (task.Run())
                {
                    int size = task.AssetBundleInfo.Size;
                    task.Dispose();
                    node.Value = null;
                    this.tasks.Remove(node);
                    if (size > SIZE_BEFORE_SYSTEM_GC)
                    {
                        GC.Collect();
                    }
                }
            }
        }

        public DiskCacheWriterRequest WriteToDiskCache(string path, byte[] data) => 
            this.diskWriter.Write(path, data);

        public AssetBundleDatabase DataBase { get; private set; }

        public string CachePath { get; private set; }

        public string BaseUrl { get; private set; }
    }
}

