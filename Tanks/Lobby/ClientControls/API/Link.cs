namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class Link : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Button button;

        public void Awake()
        {
            this.button.onClick.AddListener(new UnityAction(this.OnClick));
        }

        public void OnClick()
        {
            this.button.onClick.RemoveListener(new UnityAction(this.OnClick));
            this.animator.SetBool("Activated", true);
        }
    }
}

