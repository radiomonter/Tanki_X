namespace Tanks.Lobby.ClientControls.API
{
    using UnityEngine;

    public class WorldSpaceScaleControllerProvider : MonoBehaviour, BaseElementScaleControllerProvider
    {
        [SerializeField]
        private Tanks.Lobby.ClientControls.API.BaseElementScaleController baseElementScaleController;

        public Tanks.Lobby.ClientControls.API.BaseElementScaleController BaseElementScaleController =>
            this.baseElementScaleController;
    }
}

