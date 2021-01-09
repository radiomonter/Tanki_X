namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class ShopTab : Tab
    {
        [SerializeField]
        private bool showBackground = true;
        [SerializeField]
        private Animator backgroundAnimator;

        public override void Hide()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.Hide();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ContainersUI component = base.gameObject.GetComponent<ContainersUI>();
            if (component != null)
            {
                component.OnEnable();
            }
            this.backgroundAnimator.SetBool("Background", this.showBackground);
        }
    }
}

