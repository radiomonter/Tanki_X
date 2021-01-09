namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class TooltipShowBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [HideInInspector]
        public bool showTooltip = true;
        [HideInInspector]
        public bool customContentPrefab;
        [HideInInspector]
        public GameObject customPrefab;
        [HideInInspector]
        public bool defaultBackground = true;
        [HideInInspector]
        public bool overrideDelay;
        [HideInInspector]
        public float customDelay = 0.2f;
        public LocalizedField localizedTip;
        private float hoverTimer;
        private bool pointerIn;
        protected bool tooltipShowed;
        private Vector3 lastCursorPosition;
        protected string tipText = string.Empty;
        protected object customData;

        protected virtual void Awake()
        {
            if (string.IsNullOrEmpty(this.tipText) && !string.IsNullOrEmpty(this.localizedTip.Value))
            {
                this.TipText = this.localizedTip.Value;
            }
        }

        private bool HasShowData() => 
            !string.IsNullOrEmpty(this.tipText) || (this.customContentPrefab && (this.customData != null));

        public void HideTooltip()
        {
            this.pointerIn = false;
            this.hoverTimer = 0f;
            if (this.tooltipShowed)
            {
                TooltipController.Instance.HideTooltip();
            }
            this.tooltipShowed = false;
        }

        private void OnDisable()
        {
            this.HideTooltip();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.pointerIn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.HideTooltip();
        }

        public void SetCustomContentData(object data)
        {
            if (!this.customContentPrefab)
            {
                throw new Exception("Couldn't set custom content data. You have to set custom prefab");
            }
            this.customData = data;
        }

        public virtual void ShowTooltip(Vector3 mousePosition)
        {
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
            if (!eventInstance.TutorialIsActive)
            {
                this.tooltipShowed = true;
                if (!this.customContentPrefab)
                {
                    TooltipController.Instance.ShowTooltip(mousePosition, this.tipText, null, this.defaultBackground);
                }
                else
                {
                    object customData = this.customData;
                    if (this.customData == null)
                    {
                        object local1 = this.customData;
                        customData = this.tipText;
                    }
                    TooltipController.Instance.ShowTooltip(mousePosition, customData, this.customPrefab, this.defaultBackground);
                }
            }
        }

        private void Update()
        {
            if (this.HasShowData() && this.showTooltip)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    this.HideTooltip();
                }
                if (this.pointerIn && !this.tooltipShowed)
                {
                    Vector3 mousePosition = Input.mousePosition;
                    if (this.lastCursorPosition != mousePosition)
                    {
                        this.hoverTimer = 0f;
                    }
                    this.lastCursorPosition = mousePosition;
                    this.hoverTimer += Time.deltaTime;
                    float num = !this.overrideDelay ? (!TooltipController.Instance.quickShow ? TooltipController.Instance.delayBeforeTooltipShowAfterCursorStop : TooltipController.Instance.delayBeforeQuickShow) : this.customDelay;
                    if (this.hoverTimer >= num)
                    {
                        this.ShowTooltip(mousePosition);
                    }
                }
            }
        }

        public void UpdateTipText()
        {
            this.TipText = this.localizedTip.Value;
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public string TipText
        {
            get => 
                this.tipText;
            set => 
                this.tipText = value;
        }
    }
}

