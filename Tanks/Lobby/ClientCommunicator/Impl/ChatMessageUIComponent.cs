namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class ChatMessageUIComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private TextMeshProUGUI firstPartText;
        [SerializeField]
        private TextMeshProUGUI secondPartText;
        public bool showed;

        public string FirstPartText
        {
            get => 
                this.firstPartText.text;
            set
            {
                this.firstPartText.text = value;
                Canvas.ForceUpdateCanvases();
                this.firstPartText.GetComponent<LayoutElement>().minWidth = this.firstPartText.rectTransform.rect.width;
            }
        }

        public Color FirstPartTextColor
        {
            set => 
                this.firstPartText.color = value;
        }

        public string SecondPartText
        {
            get => 
                this.secondPartText.text;
            set => 
                this.secondPartText.text = value;
        }

        public Color SecondPartTextColor
        {
            set => 
                this.secondPartText.color = value;
        }
    }
}

