namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Animator))]
    public class EffectHUDComponent : BehaviourComponent, AttachToEntityListener, DetachFromEntityListener
    {
        [SerializeField]
        private ImageSkin icon;
        [SerializeField]
        private Image indicator;
        [SerializeField]
        private Image indicatorLighting;
        [SerializeField]
        private Image durationProgress;
        [SerializeField]
        private PaletteColorField buffColor;
        [SerializeField]
        private PaletteColorField debuffColor;
        [SerializeField]
        private TextMeshProUGUI timerText;
        private bool ticking;
        private float duration;
        private float timer;
        private float lastTimer = -1f;
        private Entity entity;

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        public void DetachedFromEntity(Entity entity)
        {
            this.entity = null;
        }

        private void Init(PaletteColorField color, string icon)
        {
            Color color2 = color.Color;
            color2.a = 1f;
            this.indicator.color = color2;
            this.indicatorLighting.color = color2;
            this.icon.SpriteUid = icon;
        }

        public void InitBuff(string icon)
        {
            this.Init(this.buffColor, icon);
        }

        public void InitDebuff(string icon)
        {
            this.Init(this.debuffColor, icon);
        }

        public void InitDuration(float duration)
        {
            this.SetFillAmount(this.durationProgress, 1f);
            this.duration = duration;
            this.timer = 0f;
            this.ticking = duration != 0f;
            this.SetTimerText();
        }

        public void Kill()
        {
            base.GetComponent<Animator>().SetTrigger("Kill");
        }

        private void OnDisable()
        {
            if (ECSBehaviour.EngineService != null)
            {
                if ((this.entity != null) && this.entity.HasComponent<EffectHUDComponent>())
                {
                    this.entity.RemoveComponent<EffectHUDComponent>();
                }
                Destroy(base.gameObject);
            }
        }

        private void OnReadyToDie()
        {
            base.gameObject.SetActive(false);
        }

        public void SetAllDirty()
        {
            foreach (Graphic graphic in base.GetComponentsInChildren<Graphic>(true))
            {
                graphic.SetAllDirty();
            }
        }

        private void SetFillAmount(Image image, float fillAmount)
        {
            image.rectTransform.anchorMax = new Vector2(1f, fillAmount);
        }

        private void SetTimerText()
        {
            this.timerText.text = $"{this.duration - this.timer:0}";
        }

        private void Update()
        {
            if (this.ticking)
            {
                this.timer += Time.deltaTime;
                this.timer = Mathf.Min(this.timer, this.duration);
                if (this.timer != this.lastTimer)
                {
                    this.lastTimer = this.timer;
                    float fillAmount = 1f - (this.timer / this.duration);
                    this.SetFillAmount(this.durationProgress, fillAmount);
                    if (fillAmount <= 0f)
                    {
                        this.ticking = false;
                    }
                    this.SetTimerText();
                }
            }
        }
    }
}

