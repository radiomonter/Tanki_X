namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using TMPro;
    using UnityEngine;

    public class DamageInfoComponent : BehaviourComponent
    {
        public Material criticalMaterialPreset;
        public TextMeshProUGUI text;
        private Camera _cachedCamera;

        public Camera CachedCamera
        {
            get
            {
                if (!this._cachedCamera)
                {
                    this._cachedCamera = Camera.main;
                }
                return this._cachedCamera;
            }
        }
    }
}

