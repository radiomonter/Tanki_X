namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class GoToShopButton : MonoBehaviour
    {
        [SerializeField]
        private int tab;
        [SerializeField]
        private BaseDialogComponent _callDialog;

        private void Awake()
        {
            base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.Go));
        }

        private void Go()
        {
            MainScreenComponent.Instance.ShowShopIfNotVisible();
            ShopTabManager.shopTabIndex = this.tab;
            if (this._callDialog != null)
            {
                this._callDialog.Hide();
            }
        }

        public int DesiredShopTab
        {
            get => 
                this.tab;
            set => 
                this.tab = value;
        }

        public BaseDialogComponent CallDialog
        {
            get => 
                this._callDialog;
            set => 
                this._callDialog = value;
        }
    }
}

