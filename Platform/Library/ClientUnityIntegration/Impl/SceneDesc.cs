namespace Platform.Library.ClientUnityIntegration.Impl
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SceneDesc
    {
        public bool initAfterLoading = true;
        public string sceneName;
        public Object scene;
        [NonSerialized]
        public Object sceneAsset;
    }
}

