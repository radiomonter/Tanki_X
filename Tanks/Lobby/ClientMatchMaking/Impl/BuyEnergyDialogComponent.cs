namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;

    public class BuyEnergyDialogComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private TextMeshProUGUI message;
        [SerializeField]
        private TextMeshProUGUI buyButtonText;
        [SerializeField]
        private LocalizedField messageLoc;
        [SerializeField]
        private LocalizedField buyButtonLoc;
        [SerializeField]
        private LocalizedField chargesAmountSingularText;
        [SerializeField]
        private LocalizedField chargesAmountPlural1Text;
        [SerializeField]
        private LocalizedField chargesAmountPlural2Text;
        private long energyCount;
        private long xprice;
        [CompilerGenerated]
        private static Action <>f__am$cache0;

        public override void Hide()
        {
            base.Hide();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate {
                };
            }
            MainScreenComponent.Instance.OverrideOnBack(<>f__am$cache0);
        }

        private string Pluralize(int amount)
        {
            CaseType @case = CasesUtil.GetCase(amount);
            if (@case == CaseType.DEFAULT)
            {
                return string.Format(this.chargesAmountPlural1Text.Value, amount);
            }
            if (@case == CaseType.ONE)
            {
                return string.Format(this.chargesAmountSingularText.Value, amount);
            }
            if (@case != CaseType.TWO)
            {
                throw new Exception("ivnalid case");
            }
            return string.Format(this.chargesAmountPlural2Text.Value, amount);
        }

        public void Show(long count, long price)
        {
            this.energyCount = count;
            this.message.text = string.Format((string) this.messageLoc, this.Pluralize((int) this.energyCount));
            this.xprice = price;
            this.buyButtonText.text = string.Format((string) this.buyButtonLoc, price);
            base.Show(null);
        }

        public long EnergyCount =>
            this.energyCount;

        public long Price =>
            this.xprice;
    }
}

