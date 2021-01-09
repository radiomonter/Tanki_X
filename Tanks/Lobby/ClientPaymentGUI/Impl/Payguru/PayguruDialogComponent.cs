namespace Tanks.Lobby.ClientPaymentGUI.Impl.Payguru
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class PayguruDialogComponent : EntityBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label;
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private PayguruBankItem itemPrefab;
        private Action onBack;

        public void Cancel()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("cancel");
        }

        public void setBanksData(PayguruAbbreviatedBankInfo[] banksInfo)
        {
            foreach (PayguruAbbreviatedBankInfo info in banksInfo)
            {
                PayguruBankItem item = Instantiate<PayguruBankItem>(this.itemPrefab);
                item.transform.SetParent(this.scrollRect.content, false);
                item.gameObject.SetActive(true);
                item.Init(info);
            }
        }

        public void setOrderId(string orderId)
        {
            this.label.text = "Sipariş numaranız: " + orderId;
        }
    }
}

