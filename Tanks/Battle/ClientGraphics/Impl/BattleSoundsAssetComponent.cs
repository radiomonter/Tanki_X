namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class BattleSoundsAssetComponent : BehaviourComponent
    {
        [SerializeField]
        private Tanks.Battle.ClientGraphics.Impl.BattleSoundsBehaviour battleSoundsBehaviour;

        public Tanks.Battle.ClientGraphics.Impl.BattleSoundsBehaviour BattleSoundsBehaviour =>
            this.battleSoundsBehaviour;
    }
}

