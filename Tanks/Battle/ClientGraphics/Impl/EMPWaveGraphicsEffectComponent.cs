namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class EMPWaveGraphicsEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private EMPWaveGraphicsBehaviour empWaveAsset;

        public EMPWaveGraphicsBehaviour EMPWaveAsset =>
            this.empWaveAsset;
    }
}

