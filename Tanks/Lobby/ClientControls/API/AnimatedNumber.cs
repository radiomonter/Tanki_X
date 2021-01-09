namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;

    public class AnimatedNumber : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField]
        private TextMeshProUGUI numberText;
        [SerializeField]
        private string format = "{0:#}";
        [SerializeField]
        private float duration = 0.15f;
        private float time;
        private float _currentValue;
        private float targetValue = -1f;
        private bool immediatePending;

        private void OnEnable()
        {
            if (!this.immediatePending)
            {
                this.currentValue = 0f;
            }
            this.immediatePending = false;
        }

        public void SetFormat(string newFormat)
        {
            this.format = newFormat;
        }

        public void SetImmediate(float value)
        {
            this.targetValue = value;
            this.currentValue = this.targetValue;
            this.numberText.text = string.Format(this.format, value);
            this.immediatePending = !base.gameObject.activeInHierarchy;
        }

        private void Update()
        {
            if (this.currentValue != this.targetValue)
            {
                this.currentValue = Mathf.Lerp(this.currentValue, this.targetValue, this.curve.Evaluate(Mathf.Clamp01(this.time / this.duration)));
                this.time += Time.deltaTime;
            }
        }

        private float currentValue
        {
            get => 
                this._currentValue;
            set
            {
                this._currentValue = value;
                this.numberText.text = string.Format(this.format, this.currentValue);
            }
        }

        public float Value
        {
            get => 
                this.targetValue;
            set
            {
                this.targetValue = value;
                this.time = 0f;
            }
        }
    }
}

