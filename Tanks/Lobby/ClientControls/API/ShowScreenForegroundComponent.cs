namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using UnityEngine;

    [SerialVersionUID(0x8d32d55551e4462L)]
    public class ShowScreenForegroundComponent : MonoBehaviour, Component
    {
        [SerializeField, Range(0f, 1f)]
        private float alpha = 1f;

        public float Alpha =>
            this.alpha;
    }
}

