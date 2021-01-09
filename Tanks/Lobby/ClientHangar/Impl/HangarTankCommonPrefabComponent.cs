namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class HangarTankCommonPrefabComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject tankCommonPrefab;

        public GameObject TankCommonPrefab =>
            this.tankCommonPrefab;
    }
}

