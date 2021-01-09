namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(NormalizedAnimatedValue)), RequireComponent(typeof(TextMeshProUGUI))]
    public class TextAnimation : MonoBehaviour
    {
        private string targetText;
        private bool inFadeMode;

        private void OnDisable()
        {
            this.targetText = string.Empty;
            base.GetComponent<TextMeshProUGUI>().text = string.Empty;
        }

        private void SwitchMode()
        {
        }

        public string Text
        {
            set
            {
                if (this.targetText != value)
                {
                    base.GetComponent<TextMeshProUGUI>().text = value;
                    base.GetComponent<Animator>().SetTrigger("Start");
                }
            }
        }
    }
}

