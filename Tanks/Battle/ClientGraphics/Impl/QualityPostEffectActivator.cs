namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class QualityPostEffectActivator : MonoBehaviour
    {
        public Quality.QualityLevel quality;
        public DepthTextureMode depthTextureMode;
        [SerializeField]
        private List<MonoBehaviour> postEffects;

        public void Start()
        {
            if (QualitySettings.GetQualityLevel() == this.quality)
            {
                Camera.main.depthTextureMode = this.depthTextureMode;
                foreach (MonoBehaviour behaviour in this.postEffects)
                {
                    behaviour.enabled = true;
                }
            }
        }
    }
}

