namespace Platform.Library.ClientResources.Impl
{
    using log4net;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.Library.ClientUnityIntegration.Impl;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AssetBundleDiskCacheTask : IDisposable
    {
        public static readonly int RELOAD_FROM_HTTP_ATTEMPTS = 2;
        public static readonly int CRC_RELOAD_ATTEMPS = 2;
        public static readonly int BUNDLE_RECRATION_ATTEMPS = 2;
        public static Crc32 CRC32 = new Crc32();
        private AssetBundleDiskCache assetBundleDiskCache;
        private Dictionary<AssetBundleDiskCacheState, Action> state2action = new Dictionary<AssetBundleDiskCacheState, Action>(10);
        private WWWLoader wwwLoader;
        private AssetBundleCreateRequest createRequest;
        private DiskCacheWriterRequest writeRequest;
        private int loadFromHttpAttempts = RELOAD_FROM_HTTP_ATTEMPTS;
        private int crcReloadAttempts = CRC_RELOAD_ATTEMPS;
        private int bundleCreationAttempts = BUNDLE_RECRATION_ATTEMPS;
        private float taskNextRunTime;
        private byte[] buffer;
        private string url;

        public AssetBundleDiskCacheTask(AssetBundleDiskCache _assetBundleDiskCache)
        {
            this.assetBundleDiskCache = _assetBundleDiskCache;
            this.state2action.Add(AssetBundleDiskCacheState.INIT, new Action(this.Initialize));
            this.state2action.Add(AssetBundleDiskCacheState.START_LOAD_FROM_HTTP, new Action(this.StartLoadFromHTTP));
            this.state2action.Add(AssetBundleDiskCacheState.LOADING_FROM_HTTP, new Action(this.LoadingFromHTTP));
            this.state2action.Add(AssetBundleDiskCacheState.START_LOAD_FROM_DISK, new Action(this.StartLoadFromDisk));
            this.state2action.Add(AssetBundleDiskCacheState.START_WRITE_TO_DISK, new Action(this.StartWriteToDisk));
            this.state2action.Add(AssetBundleDiskCacheState.WRITE_TO_DISK, new Action(this.WriteToDisk));
            this.state2action.Add(AssetBundleDiskCacheState.CREATE_ASSETBUNDLE, new Action(this.CreateAssetBundle));
            this.state2action.Add(AssetBundleDiskCacheState.COMPLETE, new Action(this.Complete));
            this.Log = LoggerProvider.GetLogger(this);
        }

        private bool CheckCrc()
        {
            int num;
            if (CRC32.Compute(this.buffer) == this.AssetBundleInfo.CacheCRC)
            {
                return true;
            }
            this.crcReloadAttempts = (num = this.crcReloadAttempts) - 1;
            if (num <= 0)
            {
                this.Error = $"Crc mismatch while loading {this.AssetBundleInfo.BundleName}";
                this.State = AssetBundleDiskCacheState.COMPLETE;
                return false;
            }
            LoggerProvider.GetLogger(this).WarnFormat("Crc mismatch while loading {0}, try to download it agan ", this.AssetBundleInfo.BundleName);
            this.buffer = null;
            this.State = AssetBundleDiskCacheState.START_LOAD_FROM_HTTP;
            return false;
        }

        private bool CheckFileIsValid(string path)
        {
            bool flag;
            try
            {
                if (new FileInfo(path).Length == this.AssetBundleInfo.Size)
                {
                    return true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (IOException)
            {
                flag = false;
            }
            return flag;
        }

        public void Complete()
        {
            if (this.wwwLoader != null)
            {
                this.wwwLoader.Dispose();
                this.wwwLoader = null;
            }
            this.Progress = 1f;
            this.IsDone = true;
        }

        private void CreateAssetBundle()
        {
            if (this.createRequest.isDone)
            {
                this.AssetBundle = this.createRequest.assetBundle;
                if (this.AssetBundle == null)
                {
                    if (this.HandleRestartOnBundleCreationFail())
                    {
                        return;
                    }
                    this.Error = $"failed to create assetbundle {this.AssetBundleInfo.BundleName}";
                }
                this.State = AssetBundleDiskCacheState.COMPLETE;
            }
        }

        public void Dispose()
        {
            if (this.wwwLoader != null)
            {
                this.wwwLoader.Dispose();
                this.wwwLoader = null;
            }
            this.buffer = null;
        }

        private bool HandleRestartOnBundleCreationFail()
        {
            int num;
            this.bundleCreationAttempts = (num = this.bundleCreationAttempts) - 1;
            if (num > 0)
            {
                this.Sleep(0.5f);
                LoggerProvider.GetLogger(this).WarnFormat("Failed to create assetBundle {0}, try to create it agan ", this.AssetBundleInfo.BundleName);
                this.State = AssetBundleDiskCacheState.START_LOAD_FROM_DISK;
                return true;
            }
            this.loadFromHttpAttempts = (num = this.loadFromHttpAttempts) - 1;
            if (num > 0)
            {
                LoggerProvider.GetLogger(this).WarnFormat("Failed to create assetBundle {0}, try to download it agan ", this.AssetBundleInfo.BundleName);
                if (this.assetBundleDiskCache.CleanCache(this.AssetBundleInfo))
                {
                    this.State = AssetBundleDiskCacheState.START_LOAD_FROM_HTTP;
                    return true;
                }
            }
            return false;
        }

        public AssetBundleDiskCacheTask Init(Platform.Library.ClientResources.API.AssetBundleDiskCacheRequest request)
        {
            this.AssetBundleInfo = request.AssetBundleInfo;
            this.AssetBundleDiskCacheRequest = request;
            return this;
        }

        private void Initialize()
        {
            this.State = !this.assetBundleDiskCache.IsCached(this.AssetBundleInfo) ? AssetBundleDiskCacheState.START_LOAD_FROM_HTTP : AssetBundleDiskCacheState.START_LOAD_FROM_DISK;
        }

        private void LoadingFromHTTP()
        {
            if (!string.IsNullOrEmpty(this.wwwLoader.Error))
            {
                int num;
                this.loadFromHttpAttempts = (num = this.loadFromHttpAttempts) - 1;
                if (num <= 0)
                {
                    this.Error = $"{this.wwwLoader.Error}, url: {this.url}";
                    this.State = AssetBundleDiskCacheState.COMPLETE;
                }
                else
                {
                    int num2 = (RELOAD_FROM_HTTP_ATTEMPTS - this.loadFromHttpAttempts) + 2;
                    LoggerProvider.GetLogger(this).WarnFormat("AssetBundle download failed, try attempt {0}, URL: {1}, ERROR: {2}", num2, this.url, this.wwwLoader.Error);
                    this.wwwLoader.Dispose();
                    this.State = AssetBundleDiskCacheState.START_LOAD_FROM_HTTP;
                }
            }
            else
            {
                this.Progress = this.wwwLoader.Progress;
                if (this.wwwLoader.IsDone)
                {
                    this.buffer = this.wwwLoader.Bytes;
                    if (!this.AssetBundleDiskCacheRequest.UseCrcCheck || this.CheckCrc())
                    {
                        this.wwwLoader.Dispose();
                        this.State = AssetBundleDiskCacheState.START_WRITE_TO_DISK;
                    }
                }
            }
        }

        public bool Run()
        {
            while (Time.realtimeSinceStartup >= this.taskNextRunTime)
            {
                AssetBundleDiskCacheState state = this.State;
                UnityProfiler.OnBeginSample("Invoke " + this.State);
                this.state2action[this.State]();
                UnityProfiler.OnEndSample();
                if (state == this.State)
                {
                    this.UpdateRequest();
                    return this.IsDone;
                }
            }
            return this.IsDone;
        }

        public void Sleep(float seconds)
        {
            this.taskNextRunTime = Time.realtimeSinceStartup + seconds;
        }

        private void StartLoadFromDisk()
        {
            string assetBundleCachePath = this.assetBundleDiskCache.GetAssetBundleCachePath(this.AssetBundleInfo);
            if (this.CheckFileIsValid(assetBundleCachePath))
            {
                this.createRequest = UnityEngine.AssetBundle.LoadFromFileAsync(assetBundleCachePath);
                this.State = AssetBundleDiskCacheState.CREATE_ASSETBUNDLE;
            }
            else if (!this.HandleRestartOnBundleCreationFail())
            {
                this.Error = $"Can't load assetbundle {this.AssetBundleInfo.BundleName}, file is corrupted {assetBundleCachePath}";
                this.State = AssetBundleDiskCacheState.COMPLETE;
            }
        }

        private void StartLoadFromHTTP()
        {
            this.url = AssetBundleNaming.GetAssetBundleUrl(this.assetBundleDiskCache.BaseUrl, this.AssetBundleInfo.Filename);
            if (this.loadFromHttpAttempts < RELOAD_FROM_HTTP_ATTEMPTS)
            {
                this.url = $"{this.url}?rnd={Random.value}";
            }
            Console.WriteLine("Start download url: {0}", this.url);
            this.wwwLoader = new WWWLoader(new WWW(this.url));
            this.wwwLoader.MaxRestartAttempts = 0;
            this.State = AssetBundleDiskCacheState.LOADING_FROM_HTTP;
        }

        private void StartWriteToDisk()
        {
            this.writeRequest = this.assetBundleDiskCache.WriteToDiskCache(this.assetBundleDiskCache.GetAssetBundleCachePath(this.AssetBundleInfo), this.buffer);
            this.State = AssetBundleDiskCacheState.WRITE_TO_DISK;
        }

        private void UpdateRequest()
        {
            this.AssetBundleDiskCacheRequest.IsDone = this.IsDone;
            this.AssetBundleDiskCacheRequest.AssetBundle = this.AssetBundle;
            this.AssetBundleDiskCacheRequest.Progress = this.Progress;
            this.AssetBundleDiskCacheRequest.Error = this.Error;
        }

        private void WriteToDisk()
        {
            if (this.writeRequest.IsDone)
            {
                if (!string.IsNullOrEmpty(this.writeRequest.Error))
                {
                    this.Error = this.writeRequest.Error;
                    this.State = AssetBundleDiskCacheState.COMPLETE;
                }
                else
                {
                    this.buffer = null;
                    this.State = AssetBundleDiskCacheState.START_LOAD_FROM_DISK;
                }
            }
        }

        public Platform.Library.ClientResources.API.AssetBundleDiskCacheRequest AssetBundleDiskCacheRequest { get; private set; }

        public bool IsDone { get; private set; }

        public Platform.Library.ClientResources.Impl.AssetBundleInfo AssetBundleInfo { get; private set; }

        public UnityEngine.AssetBundle AssetBundle { get; private set; }

        public string Error { get; private set; }

        public float Progress { get; private set; }

        public AssetBundleDiskCacheState State { get; private set; }

        private ILog Log { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        private struct TaskRunner
        {
            public Action runner;
            public Action error;
            public Action timeOut;
            public float timeOutValue;
        }
    }
}

