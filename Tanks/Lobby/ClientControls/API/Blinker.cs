namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class Blinker : MonoBehaviour
    {
        private const float FORWARD = 1f;
        private const float BACKWARD = -1f;
        public float maxValue = 1f;
        public float minValue;
        public float speedCoeff = 1f;
        public float initialBlinkTimeInterval = 1f;
        public float minBlinkTimeInterval = 1f;
        public float blinkTimeIntervalDecrement;
        public float timeOffset;
        private float speed;
        private float valueDelta;
        private float time;
        private float currentBlinkTimeInterval;
        private float value;
        private float timeBeforeBlink;
        public OnBlinkEvent onBlink;

        private float GetSpeed(float direction) => 
            ((direction * this.speedCoeff) * this.valueDelta) / this.currentBlinkTimeInterval;

        private void OnDisable()
        {
            this.StopBlink();
        }

        private void OnEnable()
        {
            this.Reset();
        }

        private void Reset()
        {
            this.timeBeforeBlink = this.timeOffset;
            this.currentBlinkTimeInterval = this.initialBlinkTimeInterval;
            this.valueDelta = this.maxValue - this.minValue;
            this.value = this.maxValue;
            this.speed = this.GetSpeed(-1f);
        }

        public void StartBlink()
        {
            this.Reset();
            this.onBlink.Invoke(this.maxValue);
            base.enabled = true;
        }

        public void StopBlink()
        {
            base.enabled = false;
            this.onBlink.Invoke(this.maxValue);
        }

        private void Update()
        {
            this.timeBeforeBlink -= Time.deltaTime;
            if (this.timeBeforeBlink <= 0f)
            {
                this.time += Time.deltaTime;
                this.value += this.speed * Time.deltaTime;
                if (this.value > this.maxValue)
                {
                    this.value = this.maxValue;
                }
                if (this.value < this.minValue)
                {
                    this.value = this.minValue;
                }
                if (this.time >= this.currentBlinkTimeInterval)
                {
                    if (this.currentBlinkTimeInterval > this.minBlinkTimeInterval)
                    {
                        this.currentBlinkTimeInterval -= this.blinkTimeIntervalDecrement;
                        if (this.currentBlinkTimeInterval < this.minBlinkTimeInterval)
                        {
                            this.currentBlinkTimeInterval = this.minBlinkTimeInterval;
                        }
                    }
                    this.time = 0f;
                    this.speed = this.GetSpeed((this.speed >= 0f) ? -1f : 1f);
                }
                this.onBlink.Invoke(this.value);
            }
        }

        [Serializable]
        public class OnBlinkEvent : UnityEvent<float>
        {
        }
    }
}

