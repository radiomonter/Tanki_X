namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class TimerComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private TimerUIComponent timer;

        public TimerUIComponent Timer =>
            this.timer;
    }
}

