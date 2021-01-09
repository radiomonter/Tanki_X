namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ScreenBackgroundAnimationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private int layerId;
        [SerializeField]
        private string state = "Show";
        [SerializeField]
        private string speedMultiplicatorName = "showSpeed";
        [SerializeField]
        private UnityEngine.Animator animator;

        public ScreenBackgroundAnimationComponent()
        {
            this.State = UnityEngine.Animator.StringToHash(this.state);
            this.SpeedMultiplicatorId = UnityEngine.Animator.StringToHash(this.speedMultiplicatorName);
        }

        public int LayerId =>
            this.layerId;

        public int State { get; private set; }

        public int SpeedMultiplicatorId { get; private set; }

        public UnityEngine.Animator Animator =>
            this.animator;
    }
}

