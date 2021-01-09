namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class BattleItemContentComponent : UIBehaviour, Component, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        private const string GEAR_SPEED_MULTIPLIER = "GearTransparencySign";
        private const string GEAR_TRANSPARENCY_STATE_NAME = "GearTransparency";
        private const string SHOW_PREVIEW_NAME = "ShowPreview";
        private const string HIGHLIGHTED_NAME = "Highlighted";
        private const string NORMAL_NAME = "Normal";
        private const float GEAR_REVERSE_MULTIPLIER = -0.66f;
        private const int GEAR_TRANSPARENCY_STATE_LAYER_INDEX = 1;
        [SerializeField]
        private Text battleModeTextField;
        [SerializeField]
        private Text debugInfoTextField;
        [SerializeField]
        private Text timeTextField;
        [SerializeField]
        private Text userCountTextField;
        [SerializeField]
        private Text scoreTextField;
        [SerializeField]
        private RawImage previewImage;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Material grayscaleMaterial;
        [SerializeField]
        private RectTransform timeTransform;
        [SerializeField]
        private RectTransform scoreTransform;
        [SerializeField]
        private RectTransform scoreTankIcon;
        [SerializeField]
        private RectTransform scoreFlagIcon;
        private bool entranceLocked;
        private int showPreviewID;
        private int gearTransparencyStateID;
        private int gearSpeedMultiplierID;
        private int showGearID;
        private int highlightedID;
        private int normalID;

        protected override void Awake()
        {
            this.showPreviewID = Animator.StringToHash("ShowPreview");
            this.normalID = Animator.StringToHash("Normal");
            this.highlightedID = Animator.StringToHash("Highlighted");
            this.gearSpeedMultiplierID = Animator.StringToHash("GearTransparencySign");
            this.gearTransparencyStateID = Animator.StringToHash("GearTransparency");
        }

        public void HideScore()
        {
            this.scoreTransform.gameObject.SetActive(false);
        }

        public void HideTime()
        {
            this.scoreTransform.anchoredPosition = this.timeTransform.anchoredPosition;
            this.timeTransform.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.animator.SetTrigger(this.highlightedID);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.animator.SetTrigger(this.normalID);
        }

        protected override void OnRectTransformDimensionsChange()
        {
            if (this.previewImage.texture != null)
            {
                Rect rect = new Rect(0f, 0f, (float) this.previewImage.texture.width, (float) this.previewImage.texture.height);
                float num = rect.width / rect.height;
                RectTransform transform = (RectTransform) base.transform;
                ((RectTransform) this.previewImage.transform).sizeDelta = ((transform.rect.width / transform.rect.height) >= num) ? new Vector2(transform.rect.width, transform.rect.width / num) : new Vector2(num * transform.rect.height, transform.rect.height);
            }
        }

        public void SetDebugField(string text)
        {
            this.debugInfoTextField.text = text;
        }

        public void SetFlagAsScoreIcon()
        {
            this.scoreTankIcon.gameObject.SetActive(false);
            this.scoreFlagIcon.gameObject.SetActive(true);
        }

        public void SetModeField(string text)
        {
            this.battleModeTextField.text = text;
        }

        public void SetPreview(Texture2D image)
        {
            this.previewImage.texture = image;
            this.animator.SetTrigger(this.showPreviewID);
            this.animator.SetFloat(this.gearSpeedMultiplierID, -0.66f);
            float normalizedTime = Mathf.Clamp01(this.animator.GetCurrentAnimatorStateInfo(1).normalizedTime);
            this.animator.Play(this.gearTransparencyStateID, 1, normalizedTime);
            this.OnRectTransformDimensionsChange();
        }

        public void SetScoreField(string text)
        {
            this.scoreTextField.text = text;
        }

        public void SetTimeField(string text)
        {
            this.timeTextField.text = text;
        }

        public void SetUserCountField(string text)
        {
            this.userCountTextField.text = text;
        }

        public bool EntranceLocked
        {
            get => 
                this.entranceLocked;
            set
            {
                this.entranceLocked = value;
                if (value)
                {
                    this.previewImage.material = this.grayscaleMaterial;
                    base.SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
                }
                else
                {
                    this.previewImage.material = null;
                    base.SendMessageUpwards("OnItemEnabled", SendMessageOptions.RequireReceiver);
                }
            }
        }
    }
}

