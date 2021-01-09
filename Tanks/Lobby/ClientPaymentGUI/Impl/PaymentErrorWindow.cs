namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class PaymentErrorWindow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI info;
        private Action onHide;

        public void Hide()
        {
            base.GetComponent<Animator>().SetTrigger("cancel");
            this.onHide();
        }

        public void Show(Entity item, Entity method, Action onHide)
        {
            this.onHide = onHide;
            base.gameObject.SetActive(true);
            this.info.text = ShopDialogs.FormatItem(item, method);
        }
    }
}

