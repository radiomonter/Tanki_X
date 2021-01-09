namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChatDialogComponent : BehaviourComponent, MainScreenComponent.IPanelShowListener
    {
        [SerializeField]
        private CanvasGroup maximazedCanvasGroup;
        [SerializeField]
        private CanvasGroup minimazedCanvasGroup;
        [SerializeField]
        private GameObject minimizeButton;
        [SerializeField]
        private GameObject maximizeButton;
        private bool hidden;
        private int MAX_MESSAGE_LENGTH = 200;
        [SerializeField]
        private int baseBottomHeight = 60;
        [SerializeField]
        private int bottomHeightLineAdditional = 0x18;
        [SerializeField]
        private RectTransform bottom;
        [SerializeField]
        private TMP_InputField inputField;
        [SerializeField]
        private GameObject inputFieldInactivePlaceholder;
        [SerializeField]
        private GameObject sendButton;
        private int caretCrutch;
        private int MAX_NEW_LINES = 5;
        private int forceNewLine;
        [SerializeField]
        private TMP_Text lastMessage;
        [SerializeField]
        private TMP_Text unreadCounter;
        private int unread;
        [SerializeField]
        private GameObject unreadBadge;
        [SerializeField]
        private ScrollRect messagesScroll;
        private int waitMessagesScroll;
        private bool autoScroll = true;
        private float lastScrollPos;
        private const int MAX_MESSAGES = 50;
        [SerializeField]
        private ImageSkin activePersonalChannelIcon;
        [SerializeField]
        private ImageSkin activeNotPersonalChannelIcon;
        [SerializeField]
        private GameObject chatIcon;
        [SerializeField]
        private GameObject userIcon;
        [SerializeField]
        private TMP_Text activeChannelName;
        [SerializeField]
        private MessageObject firstSelfMessagePrefab;
        [SerializeField]
        private MessageObject secondSelfMessagePrefab;
        [SerializeField]
        private MessageObject firstOpponentMessagePrefab;
        [SerializeField]
        private MessageObject secondOpponentMessagePrefab;
        private List<MessageObject> messages = new List<MessageObject>();
        [SerializeField]
        private Transform messagesRoot;
        [SerializeField]
        private List<ChatUISettings> uiSettings;
        [SerializeField]
        private List<ChannelRoot> channelRoots;

        public void AddUIMessage(ChatMessage message)
        {
            if (this.messages.Count == 50)
            {
                DestroyImmediate(this.messages[0].gameObject);
                this.messages.RemoveAt(0);
                if (!this.messages[0].First)
                {
                    ChatMessage message2 = this.messages[0].Message;
                    DestroyImmediate(this.messages[0].gameObject);
                    this.messages[0] = this.CreateMessage(message2, true);
                    this.messages[0].transform.SetAsFirstSibling();
                }
            }
            bool first = ((this.messages.Count == 0) || (message.System || (this.messages.Last<MessageObject>().Message.Author != message.Author))) || (this.messages.Last<MessageObject>().Message.ChatId != message.ChatId);
            this.messages.Add(this.CreateMessage(message, first));
        }

        private void Awake()
        {
            this.SetBadgeOnStart();
        }

        public void ChangeName(GameObject tab, ChatType type, string name)
        {
            <ChangeName>c__AnonStorey2 storey = new <ChangeName>c__AnonStorey2 {
                type = type
            };
            ChatUISettings settings = this.uiSettings.Find(new Predicate<ChatUISettings>(storey.<>m__0));
            ChatChannelUIComponent component = tab.GetComponent<ChatChannelUIComponent>();
            this.activeNotPersonalChannelIcon.SpriteUid = settings.IconName;
            if (string.IsNullOrEmpty(name))
            {
                name = settings.DefaultName.Value;
            }
            component.Name = $"<color=#{settings.Color.ToHexString()}>{name}</color>";
        }

        private void CheckInput()
        {
            if (!this.hidden && (this.IsOpen() && Input.GetKeyDown(KeyCode.Escape)))
            {
                this.Minimize(false);
            }
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    this.lastMessage.gameObject.SetActive(false);
                }
                else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
                {
                    this.lastMessage.gameObject.SetActive(true);
                }
            }
        }

        public EntityBehaviour CreateChatChannel(ChatType type)
        {
            <CreateChatChannel>c__AnonStorey1 storey = new <CreateChatChannel>c__AnonStorey1 {
                type = type
            };
            storey.sectionType = this.GetSection(storey.type);
            Transform parent = this.channelRoots.Find(new Predicate<ChannelRoot>(storey.<>m__0)).Parent;
            ChatUISettings settings = this.uiSettings.Find(new Predicate<ChatUISettings>(storey.<>m__1));
            EntityBehaviour behaviour = Instantiate<EntityBehaviour>(settings.ChatTabPrefab);
            behaviour.transform.SetParent(parent, false);
            ChatChannelUIComponent component = behaviour.gameObject.GetComponent<ChatChannelUIComponent>();
            component.Tab = behaviour.gameObject;
            if (storey.type != ChatType.PERSONAL)
            {
                component.SetIcon(settings.IconName);
            }
            component.Name = settings.DefaultName.Value;
            return behaviour;
        }

        private MessageObject CreateMessage(ChatMessage message, bool first)
        {
            MessageObject secondOpponentMessagePrefab = this.secondOpponentMessagePrefab;
            if (message.Self)
            {
                secondOpponentMessagePrefab = !first ? this.secondSelfMessagePrefab : this.firstSelfMessagePrefab;
            }
            else if (first)
            {
                secondOpponentMessagePrefab = this.firstOpponentMessagePrefab;
            }
            this.ScrollToEnd();
            MessageObject obj3 = Instantiate<MessageObject>(secondOpponentMessagePrefab);
            obj3.Set(message, new Func<ChatType, Color>(this.GetColorByChatType));
            obj3.transform.SetParent(this.messagesRoot, false);
            return obj3;
        }

        public void ForceMinimize()
        {
            this.hidden = false;
            this.maximizeButton.SetActive(true);
            this.minimizeButton.SetActive(false);
            this.maximazedCanvasGroup.alpha = 0f;
            this.maximazedCanvasGroup.blocksRaycasts = false;
            this.maximazedCanvasGroup.interactable = false;
            this.minimazedCanvasGroup.alpha = 1f;
            base.GetComponent<Animator>().SetBool("show", false);
        }

        private Color GetColorByChatType(ChatType chatType)
        {
            <GetColorByChatType>c__AnonStorey0 storey = new <GetColorByChatType>c__AnonStorey0 {
                chatType = chatType
            };
            return this.uiSettings.Find(new Predicate<ChatUISettings>(storey.<>m__0)).Color;
        }

        public ChatSectionType GetSection(ChatType type) => 
            (type == ChatType.PERSONAL) ? ChatSectionType.Personal : (((type == ChatType.CUSTOMGROUP) || (type == ChatType.SQUAD)) ? ChatSectionType.Group : ChatSectionType.Common);

        public void Hide()
        {
            this.hidden = true;
            this.maximizeButton.SetActive(false);
            this.minimizeButton.SetActive(false);
            this.minimazedCanvasGroup.alpha = 0f;
            this.minimazedCanvasGroup.blocksRaycasts = false;
            this.minimazedCanvasGroup.interactable = false;
            this.maximazedCanvasGroup.alpha = 0f;
            this.maximazedCanvasGroup.blocksRaycasts = false;
            this.maximazedCanvasGroup.interactable = false;
            base.GetComponent<Animator>().SetBool("show", false);
        }

        public bool IsHidden() => 
            this.hidden;

        public bool IsOpen() => 
            this.maximazedCanvasGroup.interactable;

        public void LateUpdate()
        {
            this.ScrollWaitUpdate();
            this.UpdateCaretAndSize();
            this.CheckInput();
        }

        public void Maximaze()
        {
            this.hidden = false;
            this.minimazedCanvasGroup.alpha = 0f;
            this.minimazedCanvasGroup.blocksRaycasts = false;
            this.minimazedCanvasGroup.interactable = false;
            this.maximazedCanvasGroup.alpha = 1f;
            this.maximazedCanvasGroup.blocksRaycasts = true;
            this.maximazedCanvasGroup.interactable = true;
            this.maximizeButton.SetActive(false);
            this.minimizeButton.SetActive(true);
            this.ScrollToEnd();
            base.GetComponent<Animator>().SetBool("show", true);
            ECSBehaviour.EngineService.Engine.ScheduleEvent(new ChatMaximazedEvent(), new EntityStub());
        }

        public void Minimize(bool memory = false)
        {
            if (!this.hidden)
            {
                this.ForceMinimize();
            }
        }

        public void OnHide()
        {
        }

        public void OnInputFieldSubmit(string text)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                if (!Input.GetKey(KeyCode.RightControl) && (!Input.GetKey(KeyCode.LeftControl) && (!Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift))))
                {
                    this.Send();
                }
                else if ((this.forceNewLine >= this.MAX_NEW_LINES) || (this.inputField.text.Length >= this.MAX_MESSAGE_LENGTH))
                {
                    this.caretCrutch = 2;
                }
                else
                {
                    this.inputField.text = this.inputField.text + "\n";
                    this.forceNewLine++;
                    this.caretCrutch = 3;
                }
            }
        }

        public void OnPanelShow(MainScreenComponent.MainScreens screen)
        {
            switch (screen)
            {
                case MainScreenComponent.MainScreens.Cards:
                case MainScreenComponent.MainScreens.StarterPack:
                case MainScreenComponent.MainScreens.TankRent:
                    break;

                default:
                    if ((screen == MainScreenComponent.MainScreens.MatchLobby) || (screen == MainScreenComponent.MainScreens.MatchSearching))
                    {
                        break;
                    }
                    this.ForceMinimize();
                    return;
            }
            this.Hide();
        }

        public void OnScrollRectChanged(Vector2 pos)
        {
            if (this.autoScroll)
            {
                if ((this.waitMessagesScroll == 0) && ((pos.y > 0.1) && ((this.lastScrollPos != pos.y) && (pos.y <= 0.99))))
                {
                    this.autoScroll = false;
                }
            }
            else if (pos.y <= 0.1)
            {
                this.autoScroll = true;
            }
            this.lastScrollPos = pos.y;
        }

        public void OnShow()
        {
        }

        public void Reset()
        {
            this.autoScroll = true;
            this.lastScrollPos = 0f;
            this.waitMessagesScroll = 3;
        }

        private void ResetBottomSize()
        {
            this.bottom.sizeDelta = new Vector2(this.bottom.sizeDelta.x, (float) this.baseBottomHeight);
        }

        private void ScrollToEnd()
        {
            if (this.autoScroll)
            {
                this.waitMessagesScroll = 3;
            }
        }

        private void ScrollWaitUpdate()
        {
            if (this.waitMessagesScroll > 0)
            {
                this.waitMessagesScroll--;
                if (this.waitMessagesScroll == 0)
                {
                    this.messagesRoot.GetComponent<CanvasGroup>().alpha = 1f;
                    this.messagesScroll.normalizedPosition = Vector2.zero;
                }
            }
        }

        public void SelectChannel(ChatType type, List<ChatMessage> messages)
        {
            this.messagesRoot.GetComponent<CanvasGroup>().alpha = 0f;
            this.Reset();
            this.inputField.text = string.Empty;
            this.forceNewLine = 0;
            this.ResetBottomSize();
            foreach (MessageObject obj2 in this.messages)
            {
                DestroyImmediate(obj2.gameObject);
            }
            this.messages = new List<MessageObject>();
            int num = messages.Count - 50;
            if (num < 0)
            {
                num = 0;
            }
            for (int i = num; i < messages.Count; i++)
            {
                this.AddUIMessage(messages[i]);
            }
        }

        public void Send()
        {
            this.SendMessage(this.inputField.text);
            this.inputField.ActivateInputField();
            this.inputField.text = string.Empty;
            this.ResetBottomSize();
            this.forceNewLine = 0;
            this.autoScroll = true;
        }

        public void SendMessage(string message)
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent(new SendMessageEvent(message), new EntityStub());
        }

        private void SetBadgeOnStart()
        {
            this.unreadBadge.SetActive(this.unread > 0);
        }

        public void SetHeader(string spriteUid, string header, bool personal = false)
        {
            this.activePersonalChannelIcon.SpriteUid = spriteUid;
            this.activeNotPersonalChannelIcon.SpriteUid = spriteUid;
            this.activeChannelName.text = header;
            this.chatIcon.SetActive(!personal);
            this.userIcon.SetActive(personal);
        }

        public void SetInputInteractable(bool interactable)
        {
            this.sendButton.SetActive(interactable);
            this.inputField.gameObject.SetActive(interactable);
            this.inputFieldInactivePlaceholder.SetActive(!interactable);
        }

        public void SetLastMessage(ChatMessage message)
        {
            this.lastMessage.text = message.GetEllipsis(new Func<ChatType, Color>(this.GetColorByChatType));
        }

        public void Show()
        {
            if (MainScreenComponent.Instance.isActiveAndEnabled && !TutorialCanvas.Instance.IsShow)
            {
                this.OnPanelShow(MainScreenComponent.Instance.GetCurrentPanel());
            }
        }

        private void Update()
        {
            if (this.IsOpen())
            {
                Carousel.BlockAxisAtCurrentTick();
            }
        }

        private void UpdateCaretAndSize()
        {
            if (this.caretCrutch > 0)
            {
                this.caretCrutch--;
                if (this.caretCrutch == 1)
                {
                    this.inputField.ActivateInputField();
                    this.inputField.MoveTextEnd(false);
                }
                if (this.caretCrutch == 0)
                {
                    this.bottom.sizeDelta = new Vector2(this.bottom.sizeDelta.x, Math.Max((float) this.baseBottomHeight, this.inputField.textComponent.preferredHeight + 36f));
                }
            }
        }

        public void ValidateInput(string text)
        {
            this.caretCrutch = 1;
        }

        public int Unread
        {
            get => 
                this.unread;
            set
            {
                this.unread = (value <= 0) ? 0 : value;
                this.unreadBadge.SetActive(this.unread > 0);
                this.unreadCounter.text = (this.unread <= 0x63) ? this.unread.ToString() : "99+";
            }
        }

        [CompilerGenerated]
        private sealed class <ChangeName>c__AnonStorey2
        {
            internal ChatType type;

            internal bool <>m__0(ChatDialogComponent.ChatUISettings x) => 
                x.Type == this.type;
        }

        [CompilerGenerated]
        private sealed class <CreateChatChannel>c__AnonStorey1
        {
            internal ChatDialogComponent.ChatSectionType sectionType;
            internal ChatType type;

            internal bool <>m__0(ChatDialogComponent.ChannelRoot x) => 
                x.ChatSection == this.sectionType;

            internal bool <>m__1(ChatDialogComponent.ChatUISettings x) => 
                x.Type == this.type;
        }

        [CompilerGenerated]
        private sealed class <GetColorByChatType>c__AnonStorey0
        {
            internal ChatType chatType;

            internal bool <>m__0(ChatDialogComponent.ChatUISettings x) => 
                x.Type == this.chatType;
        }

        [Serializable]
        public class ChannelRoot
        {
            [SerializeField]
            private Transform parent;
            [SerializeField]
            private ChatDialogComponent.ChatSectionType chatSection;

            public Transform Parent =>
                this.parent;

            public ChatDialogComponent.ChatSectionType ChatSection =>
                this.chatSection;
        }

        public enum ChatSectionType
        {
            Common,
            Group,
            Personal
        }

        [Serializable]
        public class ChatUISettings
        {
            [SerializeField]
            private ChatType type;
            [SerializeField]
            private UnityEngine.Color color;
            [SerializeField]
            private string iconName;
            [SerializeField]
            private LocalizedField defaultName;
            [SerializeField]
            private EntityBehaviour chatTabPrefab;

            public ChatType Type =>
                this.type;

            public UnityEngine.Color Color =>
                this.color;

            public string IconName =>
                this.iconName;

            public LocalizedField DefaultName =>
                this.defaultName;

            public EntityBehaviour ChatTabPrefab =>
                this.chatTabPrefab;
        }
    }
}

