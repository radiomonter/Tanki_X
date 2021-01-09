namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientResources.Impl;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.ClientLauncher.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class SceneSwitcher
    {
        public static void Clean()
        {
            AssetBundleDiskCache.Clean();
            AssetBundlesStorage.Clean();
            ServiceRegistry.Reset();
            InjectionUtils.ClearInjectionPoints(typeof(InjectAttribute));
            BaseElementCanvasScaler.MarkNeedInitialize();
            FatalErrorHandler.IsErrorScreenWasShown = false;
        }

        public static void CleanAndRestart()
        {
            ApplicationUtils.Restart();
        }

        public static void CleanAndSwitch(string sceneName)
        {
            CleanPreviousScene();
            SceneManager.LoadScene(sceneName);
        }

        private static void CleanPreviousScene()
        {
            InitConfigurationActivator.LauncherPassed = false;
            DisposeUrlLoaders();
            if ((NetworkService != null) && NetworkService.IsConnected)
            {
                NetworkService.Disconnect();
            }
            DestroyAllGameObjects();
        }

        private static void DestroyAllGameObjects()
        {
            Transform[] transformArray = Object.FindObjectsOfType<Transform>();
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < transformArray.Length; i++)
            {
                if ((transformArray[i].parent == null) && (transformArray[i].GetComponent<SkipRemoveOnSceneSwitch>() == null))
                {
                    list.Add(transformArray[i].gameObject);
                }
            }
            for (int j = 0; j < list.Count; j++)
            {
                list[j].gameObject.SetActive(false);
            }
            for (int k = 0; k < list.Count; k++)
            {
                Object.Destroy(list[k]);
            }
        }

        public static void DisposeUrlLoaders()
        {
            EngineService.Engine.ScheduleEvent<DisposeUrlLoadersEvent>(EngineService.EntityStub);
        }

        [Inject]
        public static Platform.System.Data.Exchange.ClientNetwork.API.NetworkService NetworkService { get; set; }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

