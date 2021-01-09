namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class LoadGearComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private UnityEngine.Animator animator;
        [SerializeField]
        private ProgressBar gearProgressBar;

        private void Hide()
        {
            base.gameObject.SetActive(false);
        }

        public UnityEngine.Animator Animator =>
            this.animator;

        public ProgressBar GearProgressBar =>
            this.gearProgressBar;
    }
}

