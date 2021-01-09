namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class MessageObject : ECSBehaviour, IPointerClickHandler, IEventSystemHandler
    {
        [SerializeField]
        private bool first;
        [SerializeField]
        private RectTransform back;
        [SerializeField]
        private ImageSkin userAvatarImageSkin;
        [SerializeField]
        private GameObject userAvatar;
        [SerializeField]
        private GameObject systemAvatar;
        [SerializeField]
        private bool self;
        [SerializeField]
        private TMP_Text nick;
        [SerializeField]
        private TMP_Text text;
        [SerializeField]
        private TMP_Text time;
        [SerializeField]
        private GameObject _tooltipPrefab;
        private ChatMessage message;

        public void OnClick(PointerEventData eventData, string link)
        {
            if (!this.Message.System)
            {
                ChatMessageClickEvent eventInstance = new ChatMessageClickEvent {
                    EventData = eventData,
                    Link = link
                };
                base.ScheduleEvent(eventInstance, new EntityStub());
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    GUIUtility.systemCopyBuffer = link;
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this._tooltipPrefab && (eventData.button == PointerEventData.InputButton.Right))
            {
                string data = this.message.Author + ":" + this.message.Message;
                TooltipController.Instance.ShowTooltip(Input.mousePosition, data, this._tooltipPrefab, false);
            }
        }

        public void Set(ChatMessage message, Func<ChatType, Color> getChatColorFunc)
        {
            if (this.first && !this.self)
            {
                this.nick.text = message.GetNickText();
            }
            this.text.text = message.GetMessageText();
            this.time.text = message.Time;
            if (!this.self && this.first)
            {
                this.userAvatar.SetActive(!message.System);
                this.systemAvatar.SetActive(message.System);
                if (!message.System)
                {
                    this.userAvatarImageSkin.SpriteUid = message.AvatarId;
                }
            }
            this.message = message;
        }

        private void Start()
        {
            if (this.nick)
            {
                this.nick.gameObject.GetComponent<ChatMessageClickHandler>().Handler = new Action<PointerEventData, string>(this.OnClick);
            }
            this.text.gameObject.GetComponent<ChatMessageClickHandler>().Handler = new Action<PointerEventData, string>(this.OnClick);
        }

        public bool First =>
            this.first;

        public ChatMessage Message =>
            this.message;
    }
}

