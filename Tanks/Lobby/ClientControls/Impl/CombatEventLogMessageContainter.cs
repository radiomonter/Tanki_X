namespace Tanks.Lobby.ClientControls.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class CombatEventLogMessageContainter : MonoBehaviour
    {
        [SerializeField]
        private int maxVisibleMessages = 5;
        [SerializeField]
        private CombatEventLogMessage messagePrefab;
        [SerializeField]
        private CombatEventLogText textPrefab;
        [SerializeField]
        private CombatEventLogUser userPrefab;
        [SerializeField]
        private TankPartItemIcon tankPartItemIconPrefab;
        [SerializeField]
        private RectTransform rectTransform;
        [SerializeField]
        private RectTransform rectTransformForMoving;
        [SerializeField]
        private VerticalLayoutGroup verticalLayout;
        public Vector2 anchoredPos;
        private readonly List<CombatEventLogMessage> visibleChildMessages = new List<CombatEventLogMessage>();

        public void AddMessage(CombatEventLogMessage message)
        {
            this.visibleChildMessages.Add(message);
            message.ShowMessage();
        }

        public void Clear()
        {
            while (this.visibleChildMessages.Count > 0)
            {
                this.DestroyMessage(this.visibleChildMessages[0]);
            }
            this.rectTransformForMoving.anchoredPosition = Vector2.zero;
        }

        public void DestroyMessage(CombatEventLogMessage message)
        {
            this.visibleChildMessages.Remove(message);
            Destroy(message.gameObject);
        }

        public TankPartItemIcon GetImageInstance() => 
            Instantiate<TankPartItemIcon>(this.tankPartItemIconPrefab);

        public CombatEventLogMessage GetMessageInstanceAndAttachToContainer()
        {
            CombatEventLogMessage message = Instantiate<CombatEventLogMessage>(this.messagePrefab);
            message.RectTransform.SetParent(this.rectTransform, false);
            return message;
        }

        public CombatEventLogText GetTextInstance() => 
            Instantiate<CombatEventLogText>(this.textPrefab);

        public CombatEventLogUser GetUserInstance() => 
            Instantiate<CombatEventLogUser>(this.userPrefab);

        private void Update()
        {
            this.anchoredPos = this.AnchoredPosition;
        }

        public List<CombatEventLogMessage> VisibleChildMessages =>
            this.visibleChildMessages;

        public int MaxVisibleMessages =>
            this.maxVisibleMessages;

        public Vector2 AnchoredPosition
        {
            get => 
                this.rectTransformForMoving.anchoredPosition;
            set => 
                this.rectTransformForMoving.anchoredPosition = value;
        }

        public int ChildCount =>
            this.rectTransform.childCount;

        public VerticalLayoutGroup VerticalLayout =>
            this.verticalLayout;

        public CombatEventLogUser UserPrefab =>
            this.userPrefab;
    }
}

