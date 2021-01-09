namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TeamFinishWindow : MonoBehaviour
    {
        [Tooltip("WIN = 0, DEFEAT = 1, DRAW = 2")]
        public Color[] titleColors;
        [SerializeField]
        private LocalizedField WinText;
        [SerializeField]
        private LocalizedField LoseText;
        [SerializeField]
        private LocalizedField TieText;
        [SerializeField]
        private GameObject earnedContainer;
        [SerializeField]
        private TextMeshProUGUI outcomeText;
        [SerializeField]
        private Text earnedText;
        [SerializeField]
        private Text amountValue;

        private void Show()
        {
            base.gameObject.SetActive(true);
            base.GetComponent<CanvasGroup>().alpha = 0f;
            base.GetComponent<Animator>().SetTrigger("Show");
        }

        public void ShowLose()
        {
            this.outcomeText.color = this.titleColors[1];
            this.outcomeText.text = this.LoseText.Value;
            this.Show();
        }

        public void ShowTie()
        {
            this.outcomeText.color = this.titleColors[2];
            this.outcomeText.text = this.TieText.Value;
            this.Show();
        }

        public void ShowWin()
        {
            this.outcomeText.color = this.titleColors[0];
            this.outcomeText.text = this.WinText.Value;
            this.Show();
        }

        public string EarnedText
        {
            set => 
                this.earnedText.text = value;
        }

        public bool CustomBattle
        {
            set => 
                this.earnedContainer.SetActive(false);
        }
    }
}

