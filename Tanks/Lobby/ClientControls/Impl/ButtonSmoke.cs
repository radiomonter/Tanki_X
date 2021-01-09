namespace Tanks.Lobby.ClientControls.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ButtonSmoke : UIBehaviour
    {
        [SerializeField]
        private ParticleSystem smoke;

        protected override void OnRectTransformDimensionsChange()
        {
            if (this.smoke != null)
            {
                this.smoke.transform.localScale = new Vector3(((RectTransform) base.transform).rect.width / 2f, 1f, 1f);
            }
        }

        public void Play()
        {
            if (this.smoke != null)
            {
                this.smoke.Play();
            }
        }

        protected override void Start()
        {
            if (QualitySettings.GetQualityLevel() < 3)
            {
                base.GetComponent<Animator>().SetLayerWeight(1, 0f);
            }
            this.OnRectTransformDimensionsChange();
        }

        public void Stop()
        {
            if (this.smoke != null)
            {
                this.smoke.Stop();
            }
        }
    }
}

