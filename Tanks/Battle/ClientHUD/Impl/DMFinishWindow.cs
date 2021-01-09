namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class DMFinishWindow : LocalizedControl
    {
        [SerializeField]
        private Text timeIsUpText;

        public void Show()
        {
            base.gameObject.SetActive(true);
            base.GetComponent<CanvasGroup>().alpha = 0f;
            base.GetComponent<Animator>().SetTrigger("Show");
        }

        public string TimeIsUpText
        {
            set => 
                this.timeIsUpText.text = value;
        }
    }
}

