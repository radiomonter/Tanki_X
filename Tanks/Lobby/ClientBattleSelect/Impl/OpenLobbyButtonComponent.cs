namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;

    public class OpenLobbyButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI _buttonText;
        [SerializeField]
        private LocalizedField _openText;
        [SerializeField]
        private LocalizedField _openTooltipText;
        [SerializeField]
        private LocalizedField _shareTooltipText;
        [SerializeField]
        private TooltipShowBehaviour _tooltip;
        [SerializeField]
        private GaragePrice _price;
        private long _lobbyId;

        public void ResetButtonText()
        {
            this._buttonText.text = (string) this._openText;
            this._tooltip.TipText = (string) this._openTooltipText;
        }

        public long LobbyId
        {
            get => 
                this._lobbyId;
            set
            {
                this._lobbyId = value;
                this._buttonText.text = this._lobbyId.ToString();
                this._tooltip.TipText = (string) this._shareTooltipText;
            }
        }

        public int Price
        {
            set
            {
                this._price.transform.parent.gameObject.SetActive(value > 0);
                this._price.SetPrice(0, value);
            }
        }
    }
}

