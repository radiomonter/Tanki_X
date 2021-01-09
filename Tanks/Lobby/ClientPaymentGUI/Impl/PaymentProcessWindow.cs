namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class PaymentProcessWindow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI info;

        public void Show(Entity item, Entity method)
        {
            base.gameObject.SetActive(true);
            this.info.text = ShopDialogs.FormatItem(item, method);
        }
    }
}

