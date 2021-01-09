namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using TMPro;
    using UnityEngine;

    public class PremiumActivatedUIComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private GameObject premIcon;
        [SerializeField]
        private GameObject xpremIcon;
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI reason;
        [SerializeField]
        private TextMeshProUGUI days;
        [SerializeField]
        private LocalizedField promoPremTitle;
        [SerializeField]
        private LocalizedField promoPremText;
        [SerializeField]
        private LocalizedField usualPremTitle;
        [SerializeField]
        private LocalizedField premDays;
        [SerializeField]
        private LocalizedField dayLocalizationCases;

        public void ShowPrem(List<Animator> animators, bool premWithQuest, int daysCount, bool promo = false)
        {
            this.premIcon.SetActive(!premWithQuest);
            this.xpremIcon.SetActive(premWithQuest);
            this.days.text = string.Format((string) this.premDays, daysCount, CasesUtil.GetLocalizedCase((string) this.dayLocalizationCases, daysCount));
            if (!promo)
            {
                this.title.text = (string) this.usualPremTitle;
            }
            else
            {
                this.title.text = (string) this.promoPremTitle;
                this.reason.text = string.Format((string) this.promoPremText, SelfUserComponent.SelfUser.GetComponent<UserUidComponent>().Uid);
                this.reason.gameObject.SetActive(true);
            }
            this.Show(animators);
        }
    }
}

