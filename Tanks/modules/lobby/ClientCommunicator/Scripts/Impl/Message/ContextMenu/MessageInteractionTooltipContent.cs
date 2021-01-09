namespace tanks.modules.lobby.ClientCommunicator.Scripts.Impl.Message.ContextMenu
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class MessageInteractionTooltipContent : InteractionTooltipContent
    {
        [SerializeField]
        private Button _copyMessageButton;
        [SerializeField]
        private Button _copyMessageWithNameButton;
        [SerializeField]
        private Button _copyNameButton;
        [SerializeField]
        private LocalizedField _messageCopiedText;
        private string _messageWithName;

        public void AddMessageToBuffer()
        {
            int startIndex = this._messageWithName.IndexOf(':') + 1;
            GUIUtility.systemCopyBuffer = this._messageWithName.Substring(startIndex, this._messageWithName.Length - startIndex);
            base.ShowResponse((string) this._messageCopiedText);
            base.Hide();
        }

        public void AddMessageWithNameToBuffer()
        {
            GUIUtility.systemCopyBuffer = this._messageWithName;
            base.ShowResponse((string) this._messageCopiedText);
            base.Hide();
        }

        public void AddNameToBuffer()
        {
            int index = this._messageWithName.IndexOf(':');
            GUIUtility.systemCopyBuffer = (index <= 0) ? this._messageWithName : this._messageWithName.Substring(0, index);
            base.ShowResponse((string) this._messageCopiedText);
            base.Hide();
        }

        protected override void Awake()
        {
            base.Awake();
            this._copyMessageButton.onClick.AddListener(new UnityAction(this.AddMessageToBuffer));
            this._copyMessageWithNameButton.onClick.AddListener(new UnityAction(this.AddMessageWithNameToBuffer));
            this._copyNameButton.onClick.AddListener(new UnityAction(this.AddNameToBuffer));
        }

        public override void Init(object data)
        {
            this._messageWithName = (string) data;
        }
    }
}

