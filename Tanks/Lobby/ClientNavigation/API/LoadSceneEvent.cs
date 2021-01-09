namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LoadSceneEvent : Event
    {
        public LoadSceneEvent(string sceneName, Object sceneAsset)
        {
            this.SceneName = sceneName;
            this.SceneAsset = sceneAsset;
        }

        public string SceneName { get; private set; }

        public Object SceneAsset { get; private set; }
    }
}

