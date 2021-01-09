namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Library.ClientUnityIntegration;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class ActivatorScreen : UnityAwareActivator<AutoCompleting>
    {
        [SerializeField]
        private CanvasGroup backgroundGroup;
        [SerializeField]
        private Text entranceMessage;
        [SerializeField]
        private float fadeOutTimeSec = 1f;
        private State state;
        private float fadeOutSpeed;

        protected override void Activate()
        {
            this.state = State.PREPARED;
        }

        private void OnEnable()
        {
            this.fadeOutSpeed = -1f / this.fadeOutTimeSec;
            this.backgroundGroup.alpha = 1f;
            this.state = State.IDLE;
        }

        private void Update()
        {
            float alpha = this.backgroundGroup.alpha;
            switch (this.state)
            {
                case State.PREPARED:
                    this.state = State.PREPARED_IDLE;
                    break;

                case State.PREPARED_IDLE:
                    this.entranceMessage.gameObject.SetActive(false);
                    this.state = State.FADE_OUT;
                    break;

                case State.FADE_OUT:
                    alpha += this.fadeOutSpeed * Time.deltaTime;
                    if (alpha <= 0f)
                    {
                        Destroy(base.gameObject);
                    }
                    else
                    {
                        this.backgroundGroup.alpha = alpha;
                    }
                    break;

                default:
                    break;
            }
        }

        private enum State
        {
            IDLE,
            PREPARED,
            PREPARED_IDLE,
            FADE_OUT
        }
    }
}

