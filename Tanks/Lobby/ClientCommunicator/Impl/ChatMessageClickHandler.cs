namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ChatMessageClickHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
    {
        [SerializeField]
        private TMP_Text textMessage;
        public Action<PointerEventData, string> Handler;

        public void OnPointerClick(PointerEventData eventData)
        {
            int index = TMP_TextUtilities.FindIntersectingLink(this.textMessage, (Vector3) eventData.position, eventData.pressEventCamera);
            if (index != -1)
            {
                string linkID = this.textMessage.textInfo.linkInfo[index].GetLinkID();
                if (linkID != string.Empty)
                {
                    if (this.Handler != null)
                    {
                        this.Handler(eventData, linkID);
                    }
                    else
                    {
                        Application.OpenURL(linkID);
                    }
                }
            }
        }
    }
}

