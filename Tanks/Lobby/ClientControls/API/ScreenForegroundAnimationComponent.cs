namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ScreenForegroundAnimationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private UnityEngine.Animator animator;

        public UnityEngine.Animator Animator =>
            this.animator;

        public float Alpha
        {
            get => 
                base.GetComponent<CanvasGroup>().alpha;
            set => 
                base.GetComponent<CanvasGroup>().alpha = value;
        }
    }
}

