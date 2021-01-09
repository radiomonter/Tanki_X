namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class LoginRewardScrollTip : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect scrollRect;
        private Animator _animator;

        private void Start()
        {
            this._animator = base.GetComponent<Animator>();
        }

        private void Update()
        {
            this._animator.SetBool("show", this.scrollRect.horizontalNormalizedPosition < 0.7f);
        }
    }
}

