namespace Tanks.Lobby.ClientLoading.API
{
    using System;
    using System.Text;
    using Tanks.Battle.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class LoadProgressBarView : MonoBehaviour
    {
        public ProgressBarComponent progressBar;
        public RectTransform endLine;
        public TextMeshProUGUI progressText;
        private StringBuilder stringBuilder = new StringBuilder();
        private RectTransform _progressBarRectTransform;
        private RectTransform _progressTextRectTransform;

        private void LateUpdate()
        {
            if (this.progressBarRectTransform != null)
            {
                float progressValue = this.progressBar.ProgressBar.ProgressValue;
                Rect rect = this.progressBarRectTransform.rect;
                float positionX = rect.x + (rect.width * progressValue);
                this.SetPosition(positionX, this.endLine, 1f);
                this.UpdateProgressText(progressValue);
                this.SetPosition(positionX, this.progressTextRectTransform, this.progressTextRectTransform.rect.width / 2f);
            }
        }

        private void SetPosition(float positionX, RectTransform uiElement, float borderOffset)
        {
            Rect rect = this.progressBarRectTransform.rect;
            Vector2 anchoredPosition = uiElement.anchoredPosition;
            anchoredPosition.x = Mathf.Clamp(positionX, rect.x + borderOffset, (rect.x + rect.width) - borderOffset);
            uiElement.anchoredPosition = anchoredPosition;
        }

        private void UpdateProgressText(float progress)
        {
            if (this.progressText != null)
            {
                this.stringBuilder.Length = 0;
                this.stringBuilder.Append(Mathf.Ceil(progress * 100f));
                this.stringBuilder.Append("%");
                this.progressText.text = this.stringBuilder.ToString();
                if (progress >= 1f)
                {
                    this.progressText.GetComponent<Animator>().SetBool("hide", true);
                }
            }
        }

        private RectTransform progressBarRectTransform
        {
            get
            {
                if (this._progressBarRectTransform == null)
                {
                    this._progressBarRectTransform = this.progressBar.GetComponent<RectTransform>();
                }
                return this._progressBarRectTransform;
            }
        }

        private RectTransform progressTextRectTransform
        {
            get
            {
                if (this._progressTextRectTransform == null)
                {
                    this._progressTextRectTransform = this.progressText.GetComponent<RectTransform>();
                }
                return this._progressTextRectTransform;
            }
        }

        public float ProgressValue
        {
            get => 
                this.progressBar.ProgressValue;
            set
            {
                this.progressBar.ProgressValue = value;
                this.LateUpdate();
            }
        }
    }
}

