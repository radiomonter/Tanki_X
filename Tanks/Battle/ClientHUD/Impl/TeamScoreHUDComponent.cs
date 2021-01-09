namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TeamScoreHUDComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI redScore;
        [SerializeField]
        private LayoutElement space;
        [SerializeField]
        private RectTransform leftScoreBack;
        [SerializeField]
        private RectTransform rightScoreBack;
        [SerializeField]
        private TextMeshProUGUI blueScore;

        private void OnDisable()
        {
            base.gameObject.SetActive(false);
        }

        public void SetCtfMode()
        {
            this.space.preferredWidth = 391f;
            base.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -76f);
            Vector2 vector = new Vector2(0f, 0f);
            this.leftScoreBack.offsetMax = vector;
            this.rightScoreBack.offsetMin = vector;
        }

        public void SetTdmMode()
        {
            this.space.preferredWidth = 130.6f;
            base.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -37f);
            this.rightScoreBack.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(-20f, 0f);
            this.leftScoreBack.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(20f, 0f);
        }

        public int RedScore
        {
            set => 
                this.redScore.text = value.ToString();
        }

        public int BlueScore
        {
            set => 
                this.blueScore.text = value.ToString();
        }
    }
}

