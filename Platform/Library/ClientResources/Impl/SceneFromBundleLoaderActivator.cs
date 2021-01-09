namespace Platform.Library.ClientResources.Impl
{
    using log4net;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.Library.ClientUnityIntegration.Impl;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SceneFromBundleLoaderActivator : UnityAwareActivator<ManuallyCompleting>, ECSActivator, Activator
    {
        public GameObject progressBar;
        public AssetReference sceneListRef;
        private ILog logger = LoggerProvider.GetLogger<SceneLoaderActivator>();
        private bool startedLoading;
        private int loadingCount;
        private bool instantiating;

        protected override void Activate()
        {
            this.StartLoading();
        }

        public void RegisterSystemsAndTemplates()
        {
            EngineService.SystemRegistry.RegisterNode<LoadedSceneNode>();
            EngineService.SystemRegistry.RegisterNode<SceneLoaderNode>();
        }

        private void StartLoading()
        {
            this.startedLoading = true;
            if (this.progressBar != null)
            {
                this.progressBar.SetActive(true);
            }
            Entity entity = EngineService.Engine.CreateEntity("ScenesLoader");
            entity.AddComponent(new AssetReferenceComponent(this.sceneListRef));
            AssetRequestComponent component = new AssetRequestComponent {
                AssetStoreLevel = AssetStoreLevel.MANAGED
            };
            entity.AddComponent(component);
            SceneLoaderComponent component2 = new SceneLoaderComponent {
                sceneName = string.Empty + base.GetInstanceID()
            };
            entity.AddComponent(component2);
        }

        private void Update()
        {
            SceneList data = null;
            if (this.startedLoading)
            {
                IEnumerable<LoadedSceneNode> source = from n in EngineService.Engine.SelectAll<LoadedSceneNode>()
                    where n.sceneLoader.sceneName.Equals(base.GetInstanceID().ToString())
                    select n;
                if (source.Any<LoadedSceneNode>())
                {
                    data = (SceneList) source.First<LoadedSceneNode>().resourceData.Data;
                }
            }
            if (this.instantiating)
            {
                base.enabled = false;
                this.logger.Info("Complete");
                base.Complete();
            }
            else if (this.startedLoading && (data != null))
            {
                this.logger.InfoFormat("Finished downloading scenes, instantiating...", new object[0]);
                this.instantiating = true;
                for (int i = 0; i < data.scenes.Length; i++)
                {
                    if (data.scenes[i].initAfterLoading)
                    {
                        string sceneName = data.scenes[i].sceneName;
                        this.logger.InfoFormat("LoadScene {0}", sceneName);
                        UnityUtil.LoadScene(data.scenes[i].scene, sceneName, true);
                    }
                }
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public class LoadedSceneNode : Node
        {
            public SceneFromBundleLoaderActivator.SceneLoaderComponent sceneLoader;
            public ResourceDataComponent resourceData;
        }

        public class SceneLoaderComponent : Component
        {
            public string sceneName;
        }

        public class SceneLoaderNode : Node
        {
            public SceneFromBundleLoaderActivator.SceneLoaderComponent sceneLoader;
            public ResourceLoadStatComponent resourceLoadStat;
        }
    }
}

