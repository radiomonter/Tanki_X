namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TooltipController : MonoBehaviour
    {
        private static TooltipController instance;
        public Tooltip tooltip;
        public float delayBeforeTooltipShowAfterCursorStop = 0.1f;
        public float maxDelayForQuickShowAfterCursorStop = 0.2f;
        [HideInInspector]
        public bool quickShow;
        public float delayBeforeQuickShow = 0.1f;
        private bool tooltipIsShow;
        private float afterHideTimer;

        private void Awake()
        {
            instance = this;
        }

        public void HideTooltip()
        {
            this.afterHideTimer = 0f;
            this.quickShow = true;
            this.tooltipIsShow = false;
            this.tooltip.Hide();
        }

        public void ShowTooltip(Vector3 position, object data, GameObject tooltipContentPrefab = null, bool backgroundActive = true)
        {
            Vector2 vector;
            this.tooltipIsShow = true;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(base.GetComponent<RectTransform>(), position, null, out vector))
            {
                this.tooltip.Show((Vector3) vector, data, tooltipContentPrefab, backgroundActive);
            }
        }

        private void Update()
        {
            if (!this.tooltipIsShow && this.quickShow)
            {
                this.afterHideTimer += Time.deltaTime;
                if (this.afterHideTimer > this.maxDelayForQuickShowAfterCursorStop)
                {
                    this.quickShow = false;
                }
            }
        }

        public static TooltipController Instance =>
            instance;
    }
}

