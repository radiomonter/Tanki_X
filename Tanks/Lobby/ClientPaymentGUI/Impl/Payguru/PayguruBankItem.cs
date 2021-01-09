namespace Tanks.Lobby.ClientPaymentGUI.Impl.Payguru
{
    using System;
    using TMPro;
    using UnityEngine;

    public class PayguruBankItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI bankData;

        public void Init(PayguruAbbreviatedBankInfo bank)
        {
            object[] objArray1 = new object[] { bank.Name, "\nBranch and account No: ", bank.BranchNumber, "-", bank.AccountNumber, "\nIBAN: ", bank.AccountIban, "\n" };
            this.BankData = string.Concat(objArray1);
        }

        public string BankData
        {
            get => 
                this.bankData.text;
            set => 
                this.bankData.text = value;
        }
    }
}

