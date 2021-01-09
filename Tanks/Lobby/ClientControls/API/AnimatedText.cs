namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AnimatedText : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI message;
        private bool textAnimation = true;
        private string resultText;
        private int currentCharIndex;
        [SerializeField]
        private float textAnimationDelay = 0.01f;
        private float timer;

        public void Animate()
        {
            this.textAnimation = true;
            this.CurrentCharIndex = 0;
        }

        public void ForceComplete()
        {
            this.CurrentCharIndex = this.resultText.Length;
        }

        private void Reset()
        {
            this.message = base.GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            this.UpdateTextAnimation();
        }

        private void UpdateTextAnimation()
        {
            if (this.textAnimation)
            {
                this.timer += Time.deltaTime;
                if (this.timer > this.textAnimationDelay)
                {
                    this.timer = 0f;
                    this.CurrentCharIndex++;
                }
            }
        }

        public bool TextAnimation =>
            this.textAnimation;

        public string ResultText
        {
            get => 
                this.resultText;
            set
            {
                this.message.text = string.Empty;
                this.resultText = value;
            }
        }

        public int CurrentCharIndex
        {
            get => 
                this.currentCharIndex;
            set
            {
                this.currentCharIndex = value;
                if (this.currentCharIndex >= this.resultText.Length)
                {
                    this.message.text = this.resultText;
                    this.textAnimation = false;
                }
                else
                {
                    char ch = this.resultText[this.currentCharIndex];
                    string str = string.Empty + ch;
                    if (ch == '<')
                    {
                        while ((ch != '>') && (this.currentCharIndex < (this.resultText.Length - 1)))
                        {
                            this.currentCharIndex++;
                            ch = this.resultText[this.currentCharIndex];
                            str = str + ch;
                        }
                    }
                    this.message.text = this.message.text + str;
                }
            }
        }
    }
}

