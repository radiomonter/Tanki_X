namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class BonusParachuteMapEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject parachute;

        public GameObject Parachute =>
            this.parachute;
    }
}

