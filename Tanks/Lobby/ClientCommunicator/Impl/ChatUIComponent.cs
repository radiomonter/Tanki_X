namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChatUIComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject inputPanel;
        [SerializeField]
        private Image bottomLineImage;
        [SerializeField]
        private TextMeshProUGUI inputHintText;
        [SerializeField]
        private Color commonNicknameColor;
        [SerializeField]
        private Color commonTextColor;
        [SerializeField]
        private Color redTeamNicknameColor;
        [SerializeField]
        private Color redTeamTextColor;
        [SerializeField]
        private Color blueTeamNicknameColor;
        [SerializeField]
        private Color blueTeamTextColor;
        [SerializeField]
        private PaletteColorField systemMessageColor;
        [SerializeField]
        private GameObject messagesContainer;
        [SerializeField]
        private LayoutElement scrollViewLayoutElement;
        [SerializeField]
        private RectTransform scrollViewRectTransform;
        [SerializeField]
        private RectTransform inputFieldRectTransform;
        [SerializeField]
        private GameObject scrollBarHandle;
        [SerializeField]
        private int maxVisibleMessagesInActiveState = 6;
        [SerializeField]
        private int maxVisibleMessagesInPassiveState = 3;
        private string savedInputMessage = string.Empty;

        public void SetHintSize(bool teamMode)
        {
            this.inputHintText.rectTransform.sizeDelta = new Vector2(!teamMode ? 56f : 86f, this.inputHintText.rectTransform.sizeDelta.y);
            this.inputFieldRectTransform.sizeDelta = new Vector2(!teamMode ? 340f : 310f, this.inputHintText.rectTransform.sizeDelta.y);
        }

        public string SavedInputMessage
        {
            get => 
                this.savedInputMessage;
            set => 
                this.savedInputMessage = value;
        }

        public Color BottomLineColor
        {
            get => 
                this.bottomLineImage.color;
            set
            {
                if (this.bottomLineImage != null)
                {
                    value.a = 0.4f;
                    this.bottomLineImage.color = value;
                }
            }
        }

        public string InputHintText
        {
            get => 
                this.inputHintText.text;
            set => 
                this.inputHintText.text = value;
        }

        public Color InputHintColor
        {
            get => 
                this.inputHintText.color;
            set => 
                this.inputHintText.color = value;
        }

        public Color InputTextColor
        {
            get => 
                this.inputPanel.GetComponentInChildren<InputField>().textComponent.color;
            set => 
                this.inputPanel.GetComponentInChildren<InputField>().textComponent.color = value;
        }

        public bool InputPanelActivity
        {
            get => 
                this.inputPanel.activeSelf;
            set => 
                this.inputPanel.SetActive(value);
        }

        public GameObject MessagesContainer =>
            this.messagesContainer;

        public float ScrollViewHeight
        {
            get => 
                this.scrollViewLayoutElement.preferredHeight;
            set => 
                this.scrollViewLayoutElement.preferredHeight = value;
        }

        public float ScrollViewPosY =>
            this.scrollViewRectTransform.anchoredPosition.y;

        public bool ScrollBarActivity
        {
            get => 
                this.scrollBarHandle.activeSelf;
            set => 
                this.scrollBarHandle.SetActive(value);
        }

        public Color CommonNicknameColor =>
            this.commonNicknameColor;

        public Color CommonTextColor =>
            this.commonTextColor;

        public Color RedTeamNicknameColor =>
            this.redTeamNicknameColor;

        public Color RedTeamTextColor =>
            this.redTeamTextColor;

        public Color BlueTeamNicknameColor =>
            this.blueTeamNicknameColor;

        public Color BlueTeamTextColor =>
            this.blueTeamTextColor;

        public Color SystemMessageColor =>
            (Color) this.systemMessageColor;

        public int MaxVisibleMessagesInActiveState =>
            this.maxVisibleMessagesInActiveState;

        public int MaxVisibleMessagesInPassiveState =>
            this.maxVisibleMessagesInPassiveState;
    }
}

