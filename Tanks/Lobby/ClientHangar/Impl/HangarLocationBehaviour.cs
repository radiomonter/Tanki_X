namespace Tanks.Lobby.ClientHangar.Impl
{
    using Tanks.Lobby.ClientHangar.API;
    using UnityEngine;

    public class HangarLocationBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Tanks.Lobby.ClientHangar.API.HangarLocation hangarLocation;

        public Tanks.Lobby.ClientHangar.API.HangarLocation HangarLocation =>
            this.hangarLocation;
    }
}

