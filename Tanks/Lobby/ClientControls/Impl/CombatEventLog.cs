namespace Tanks.Lobby.ClientControls.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class CombatEventLog : MonoBehaviour, UILog
    {
        [SerializeField]
        private bool attachToRight = true;
        private const int INVALID_RANK = -1;
        private static readonly UserElementDescription PARSE_FAILED_ELEMENT = new UserElementDescription(string.Empty, -1, Color.black);
        [SerializeField]
        private CombatEventLogMessageContainter messagesContainer;
        private int currentlyVisibleMessages;
        private float scrollHeight;
        private int spaceBetweenMessages;
        private readonly Queue<CombatEventLogMessage> messagesToDelete = new Queue<CombatEventLogMessage>();
        private readonly Regex userPattern = new Regex("{[0-9][0-9]?[0-9]?:[0-9a-fA-F]*:[^}]*}");

        public void AddMessage(string messageText)
        {
            base.StartCoroutine(this.AddMessageCoroutine(messageText));
        }

        [DebuggerHidden]
        private IEnumerator AddMessageCoroutine(string messageText) => 
            new <AddMessageCoroutine>c__Iterator0 { 
                messageText = messageText,
                $this = this
            };

        public void AddMessageImmediate(string messageText)
        {
            CombatEventLogMessage messageInstanceAndAttachToContainer = this.messagesContainer.GetMessageInstanceAndAttachToContainer();
            this.ParseAndConstructMessageLine(messageText, messageInstanceAndAttachToContainer);
            this.currentlyVisibleMessages++;
            int maxVisibleMessages = this.messagesContainer.MaxVisibleMessages;
            List<CombatEventLogMessage> visibleChildMessages = this.messagesContainer.VisibleChildMessages;
            for (int i = 0; i < (this.currentlyVisibleMessages - maxVisibleMessages); i++)
            {
                visibleChildMessages[i].RequestDelete();
            }
            this.messagesContainer.AddMessage(messageInstanceAndAttachToContainer);
        }

        private void AddTextElement(string text, CombatEventLogMessage message)
        {
            if (text.Length > 0)
            {
                string str = "<sprite name=\"";
                string oldValue = "\">";
                if (text.Contains(str))
                {
                    TankPartItemIcon imageInstance = this.messagesContainer.GetImageInstance();
                    imageInstance.SetIconWithName(text.Replace(str, string.Empty).Replace(oldValue, string.Empty).Replace(" ", string.Empty));
                    message.Attach(imageInstance.RectTransform, this.attachToRight);
                }
                else
                {
                    CombatEventLogText textInstance = this.messagesContainer.GetTextInstance();
                    textInstance.Text.text = text;
                    message.Attach(textInstance.RectTransform, this.attachToRight);
                }
            }
        }

        private void AddUserElement(string userElementString, CombatEventLogMessage message)
        {
            UserElementDescription objA = this.ParseAndValidateUserElement(userElementString);
            if (ReferenceEquals(objA, PARSE_FAILED_ELEMENT))
            {
                this.AddTextElement(userElementString, message);
            }
            else
            {
                CombatEventLogUser userInstance = this.messagesContainer.GetUserInstance();
                userInstance.RankIcon.SelectSprite(objA.RankIndex.ToString());
                userInstance.UserName.text = objA.Uid;
                userInstance.UserName.color = objA.ElementColor;
                message.Attach(userInstance.RectTransform, this.attachToRight);
            }
        }

        private void Awake()
        {
            this.spaceBetweenMessages = (int) this.messagesContainer.VerticalLayout.spacing;
            Vector2 anchoredPosition = this.messagesContainer.AnchoredPosition;
            anchoredPosition.y = this.scrollHeight;
            this.messagesContainer.AnchoredPosition = anchoredPosition;
        }

        public void Clear()
        {
            this.currentlyVisibleMessages = 0;
            this.scrollHeight = 0f;
            this.messagesContainer.Clear();
        }

        private unsafe void DestroyMessage(CombatEventLogMessage message)
        {
            float preferredHeight = message.LayoutElement.preferredHeight;
            this.scrollHeight -= preferredHeight + this.spaceBetweenMessages;
            Vector2 anchoredPosition = this.messagesContainer.AnchoredPosition;
            Vector2* vectorPtr1 = &anchoredPosition;
            vectorPtr1->y -= preferredHeight + this.spaceBetweenMessages;
            this.messagesContainer.AnchoredPosition = anchoredPosition;
            if (message != null)
            {
                this.messagesContainer.DestroyMessage(message);
            }
            this.currentlyVisibleMessages--;
        }

        private void DestroyMessageIfHidden()
        {
            if (this.messagesToDelete.Count > 0)
            {
                LayoutElement layoutElement = this.messagesToDelete.Peek().LayoutElement;
                if (((this.messagesContainer.AnchoredPosition.y - layoutElement.preferredHeight) - this.spaceBetweenMessages) >= 0f)
                {
                    this.DestroyMessage(this.messagesToDelete.Dequeue());
                }
            }
        }

        private UserElementDescription FinishParsing(string rankStr, string colorStr, string uidStr)
        {
            Color color;
            int rankIndex = this.ValidateAndParseRank(rankStr);
            bool flag = ColorUtility.TryParseHtmlString("#" + colorStr.ToLower(), out color);
            return (((rankIndex == -1) || !flag) ? PARSE_FAILED_ELEMENT : new UserElementDescription(uidStr, rankIndex, color));
        }

        private unsafe void MoveMessage()
        {
            if (this.messagesContainer.ChildCount > 0)
            {
                Vector2 anchoredPosition = this.messagesContainer.AnchoredPosition;
                if (anchoredPosition.y < this.scrollHeight)
                {
                    Vector2* vectorPtr1 = &anchoredPosition;
                    vectorPtr1->y += this.scrollHeight * Time.deltaTime;
                }
                if (anchoredPosition.y > this.scrollHeight)
                {
                    anchoredPosition.y = this.scrollHeight;
                }
                this.messagesContainer.AnchoredPosition = anchoredPosition;
            }
        }

        private void OnDeleteMessage(CombatEventLogMessage message)
        {
            this.messagesToDelete.Enqueue(message);
        }

        private void OnScrollLog(float height)
        {
            this.scrollHeight += height + this.spaceBetweenMessages;
        }

        private void ParseAndConstructMessageLine(string messageText, CombatEventLogMessage message)
        {
            MatchCollection matchs = this.userPattern.Matches(messageText);
            int count = matchs.Count;
            int startIndex = 0;
            for (int i = 0; i < count; i++)
            {
                Match match = matchs[i];
                if (match.Index != startIndex)
                {
                    this.AddTextElement(messageText.Substring(startIndex, match.Index - startIndex), message);
                }
                this.AddUserElement(match.Value, message);
                startIndex = match.Index + match.Length;
            }
            this.AddTextElement(messageText.Substring(startIndex, messageText.Length - startIndex), message);
        }

        private UserElementDescription ParseAndValidateUserElement(string userElementString)
        {
            char[] separator = new char[] { ':' };
            string[] strArray = userElementString.Substring(1, userElementString.Length - 2).Split(separator);
            return ((strArray.Length != 3) ? PARSE_FAILED_ELEMENT : this.FinishParsing(strArray[0], strArray[1], strArray[2]));
        }

        private void Update()
        {
            this.DestroyMessageIfHidden();
            this.MoveMessage();
        }

        private int ValidateAndParseRank(string rankStr)
        {
            try
            {
                int num = int.Parse(rankStr);
                int count = this.messagesContainer.UserPrefab.RankIcon.Count;
                return (((num <= 0) || (num > count)) ? -1 : num);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        [CompilerGenerated]
        private sealed class <AddMessageCoroutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal string messageText;
            internal CombatEventLog $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForSeconds(0.1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        break;

                    case 1:
                        this.$this.AddMessageImmediate(this.messageText);
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        break;

                    case 2:
                        this.$this.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
                        this.$PC = -1;
                        goto TR_0000;

                    default:
                        goto TR_0000;
                }
                return true;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        private class UserElementDescription
        {
            public UserElementDescription(string uid, int rankIndex, Color elementColor)
            {
                this.Uid = uid;
                this.RankIndex = rankIndex;
                this.ElementColor = elementColor;
            }

            public string Uid { get; set; }

            public int RankIndex { get; set; }

            public Color ElementColor { get; set; }
        }
    }
}

