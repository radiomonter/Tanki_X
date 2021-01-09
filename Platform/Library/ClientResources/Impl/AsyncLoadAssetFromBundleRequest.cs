namespace Platform.Library.ClientResources.Impl
{
    using log4net;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AsyncLoadAssetFromBundleRequest : LoadAssetFromBundleRequest
    {
        private readonly AssetBundle bundle;
        private readonly string objectName;
        private readonly Type objectType;
        private AssetBundleRequest assetBundleRequest;
        private static ILog log;
        private static List<AssetBundleRequest> assetBundleRequestList = new List<AssetBundleRequest>();
        private bool isStreamedSceneAssetBundle;

        public AsyncLoadAssetFromBundleRequest(AssetBundle bundle, string objectName, Type objectType)
        {
            this.bundle = bundle;
            this.objectName = objectName;
            this.objectType = objectType;
        }

        private ILog GetLogger()
        {
            log ??= LoggerProvider.GetLogger(this);
            return log;
        }

        public void StartExecute()
        {
            this.IsStarted = true;
            this.GetLogger().InfoFormat("LoadAssetAsync objectName={0} objectType={1}", this.objectName, this.objectType);
            if (this.bundle.isStreamedSceneAssetBundle)
            {
                this.isStreamedSceneAssetBundle = true;
            }
            else
            {
                this.assetBundleRequest = this.bundle.LoadAssetAsync(this.objectName, this.objectType);
                assetBundleRequestList.Add(this.assetBundleRequest);
            }
        }

        public AssetBundle Bundle =>
            this.bundle;

        public string ObjectName =>
            this.objectName;

        public Type ObjectType =>
            this.objectType;

        public bool IsStarted { get; private set; }

        public bool IsDone =>
            this.IsStarted ? (!this.isStreamedSceneAssetBundle ? ((this.assetBundleRequest != null) ? this.assetBundleRequest.isDone : false) : true) : false;

        public Object Asset
        {
            get
            {
                if (this.isStreamedSceneAssetBundle)
                {
                    return null;
                }
                if (!this.IsDone)
                {
                    return null;
                }
                assetBundleRequestList.Remove(this.assetBundleRequest);
                return this.assetBundleRequest.asset;
            }
        }
    }
}

