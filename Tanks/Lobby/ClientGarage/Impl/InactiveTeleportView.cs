namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Text;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class InactiveTeleportView : MonoBehaviour
    {
        public GameObject percentText;
        public GameObject successTeleportationText;
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI successTimerText;
        public LocalizedField timerTextStr;
        public Image fill;
        private Date endDate;
        private float durationInSec;
        private static string PERCENT = "%";
        private StringBuilder stringBuilder;

        public void Awake()
        {
            this.stringBuilder = new StringBuilder(10);
        }

        private string GetPercentText(float progress)
        {
            int num = (int) (progress * 100f);
            this.stringBuilder.Length = 0;
            this.stringBuilder.AppendFormat("{0}" + PERCENT, num);
            return this.stringBuilder.ToString();
        }

        private string GetTimerText()
        {
            this.stringBuilder.Length = 0;
            this.stringBuilder.Append((string) this.timerTextStr);
            return TimerUtils.GetTimerText(this.stringBuilder, (float) (this.endDate - Date.Now));
        }

        public void Update()
        {
            float progress = Date.Now.GetProgress(this.endDate - this.durationInSec, this.durationInSec);
            if (this.percentText.activeSelf)
            {
                this.percentText.GetComponent<TextMeshProUGUI>().text = this.GetPercentText(progress);
            }
            this.fill.fillAmount = (this.fill.fillAmount <= progress) ? progress : (this.fill.fillAmount - (((this.fill.fillAmount - progress) * Time.deltaTime) / 0.2f));
            this.timerText.text = this.GetTimerText();
            this.successTimerText.text = this.timerText.text;
        }

        public void UpdateView(Date endDate, float durationInSec, bool successTeleportation)
        {
            this.percentText.SetActive(!successTeleportation);
            this.successTeleportationText.SetActive(successTeleportation);
            this.endDate = endDate;
            this.durationInSec = durationInSec;
            this.fill.gameObject.SetActive(true);
        }
    }
}

