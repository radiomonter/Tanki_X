namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ScreenLockComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Animator animator;

        public void Lock()
        {
            this.animator.gameObject.SetActive(false);
            this.animator.gameObject.SetActive(true);
        }

        public void Unlock()
        {
            if (this.animator.gameObject.activeSelf)
            {
                this.animator.SetTrigger("Unlock");
            }
        }
    }
}

