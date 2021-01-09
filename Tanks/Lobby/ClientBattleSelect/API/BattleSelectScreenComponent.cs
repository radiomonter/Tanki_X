namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [SerialVersionUID(0x8d2e6e0ea75fdaaL)]
    public class BattleSelectScreenComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private EntityBehaviour itemContentPrefab;
        [SerializeField]
        private GameObject prevBattlesButton;
        [SerializeField]
        private GameObject nextBattlesButton;
        [SerializeField]
        private GameObject enterBattleDMButton;
        [SerializeField]
        private GameObject enterBattleRedButton;
        [SerializeField]
        private GameObject enterBattleBlueButton;
        [SerializeField]
        private GameObject enterAsSpectatorButton;
        [SerializeField]
        private RectTransform battleInfoPanelsContainer;
        [SerializeField]
        private GameObject dmInfoPanel;
        [SerializeField]
        private GameObject tdmInfoPanel;
        [SerializeField]
        private GameObject entrancePanel;
        [SerializeField]
        private GameObject friendsPanel;
        [SerializeField]
        private Text enterBattleDMButtonText;
        [SerializeField]
        private Text enterBattleRedButtonText;
        [SerializeField]
        private Text enterBattleBlueButtonText;
        [SerializeField]
        private Text enterAsSpectatorButtonText;

        private void Awake()
        {
            this.DMInfoPanel.SetActive(false);
            this.TDMInfoPanel.SetActive(false);
        }

        public Text EnterAsSpectatorButtonText =>
            this.enterAsSpectatorButtonText;

        public Text EnterBattleBlueButtonText =>
            this.enterBattleBlueButtonText;

        public Text EnterBattleRedButtonText =>
            this.enterBattleRedButtonText;

        public Text EnterBattleDmButtonText =>
            this.enterBattleDMButtonText;

        public EntityBehaviour ItemContentPrefab =>
            this.itemContentPrefab;

        public GameObject PrevBattlesButton =>
            this.prevBattlesButton;

        public GameObject NextBattlesButton =>
            this.nextBattlesButton;

        public GameObject EnterBattleDMButton =>
            this.enterBattleDMButton;

        public GameObject EnterBattleRedButton =>
            this.enterBattleRedButton;

        public GameObject EnterBattleBlueButton =>
            this.enterBattleBlueButton;

        public GameObject EnterBattleAsSpectatorButton =>
            this.enterAsSpectatorButton;

        public RectTransform BattleInfoPanelsContainer =>
            this.battleInfoPanelsContainer;

        public GameObject DMInfoPanel =>
            this.dmInfoPanel;

        public GameObject TDMInfoPanel =>
            this.tdmInfoPanel;

        public GameObject EntrancePanel =>
            this.entrancePanel;

        public GameObject FriendsPanel =>
            this.friendsPanel;

        public bool DebugEnabled { get; set; }
    }
}

