namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class WarmingUpTimerNotificationUIComponent : BehaviourComponent
    {
        [SerializeField]
        private AssetReference voiceReference;

        public void PlaySound(GameObject voice)
        {
            Instantiate<GameObject>(voice);
        }

        public AssetReference VoiceReference =>
            this.voiceReference;
    }
}

