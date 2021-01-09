namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class FlagsHUDComponent : BehaviourComponent, AttachToEntityListener
    {
        [SerializeField]
        private FlagController blueFlag;
        [SerializeField]
        private RectTransform blueFlagTransform;
        [SerializeField]
        private FlagController redFlag;
        [SerializeField]
        private RectTransform redFlagTransform;
        private int showRequests;

        public void AttachedToEntity(Entity entity)
        {
            this.showRequests = 0;
        }

        private void OnEnable()
        {
        }

        public void RequestHide()
        {
        }

        public void RequestShow()
        {
        }

        private void SetFlagPosition(RectTransform flag, float position)
        {
            Vector2 vector = new Vector2(position, 0f);
            flag.anchorMin = vector;
            flag.anchorMax = vector;
            flag.anchoredPosition = new Vector2(0f, ((position == 0f) || (position == 1f)) ? 0f : -8.5f);
        }

        public FlagController BlueFlag =>
            this.blueFlag;

        public FlagController RedFlag =>
            this.redFlag;

        public float RedFlagNormalizedPosition
        {
            set
            {
                if ((value > 0.5f) && (this.blueFlagTransform.anchorMax.x < 0.5f))
                {
                    this.redFlagTransform.SetAsLastSibling();
                }
                this.SetFlagPosition(this.redFlagTransform, 1f - Mathf.Clamp01(value));
            }
        }

        public float BlueFlagNormalizedPosition
        {
            set
            {
                if ((value > 0.5f) && (this.redFlagTransform.anchorMax.x > 0.5f))
                {
                    this.blueFlagTransform.SetAsLastSibling();
                }
                this.SetFlagPosition(this.blueFlagTransform, Mathf.Clamp01(value));
            }
        }
    }
}

