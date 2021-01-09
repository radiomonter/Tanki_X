namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ShareEnergyScreenComponent : BehaviourComponent
    {
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button exitButton;
        [SerializeField]
        private Button hideButton;
        [SerializeField]
        private TextMeshProUGUI readyPlayers;
        [SerializeField]
        private LocalizedField notAllPlayersReady;
        [SerializeField]
        private LocalizedField allPlayersReady;
        [SerializeField]
        private CircleProgressBar teleportPriceProgressBar;

        public void BackClick(BaseEventData data)
        {
            base.ScheduleEvent<HideAllShareButtonsEvent>(new EntityStub());
        }

        private void OnDisable()
        {
            this.hideButton.onClick.RemoveListener(new UnityAction(MainScreenComponent.Instance.HideShareEnergyScreen));
        }

        private void OnEnable()
        {
            this.hideButton.onClick.AddListener(new UnityAction(MainScreenComponent.Instance.HideShareEnergyScreen));
        }

        public void ReadyPlayers(int ready, int allPlayers)
        {
            bool flag = allPlayers == ready;
            this.readyPlayers.text = !flag ? string.Format((string) this.notAllPlayersReady, ready, allPlayers) : this.allPlayersReady.Value;
        }

        public CircleProgressBar TeleportPriceProgressBar =>
            this.teleportPriceProgressBar;

        public bool SelfPlayerIsSquadLeader
        {
            set
            {
                this.startButton.gameObject.SetActive(value);
                this.exitButton.gameObject.SetActive(value);
                this.hideButton.gameObject.SetActive(!value);
            }
        }
    }
}

