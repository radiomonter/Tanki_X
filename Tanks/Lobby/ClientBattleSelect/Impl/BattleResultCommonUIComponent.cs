namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class BattleResultCommonUIComponent : UIBehaviour, Component
    {
        private ResultScreenParts currentPart;
        private bool customBattle;
        private bool spectator;
        private bool tutor;
        private bool squad;
        private bool enoughEnergy;
        public Image tankPreviewImage1;
        public Image tankPreviewImage2;
        public TopPanelButtons topPanelButtons;
        public BottomPanelButtons bottomPanelButtons;
        [SerializeField]
        private GameObject[] screenParts;

        public void ContinueAfterBestPlayer()
        {
            if (!this.spectator)
            {
                this.ShowAwards();
            }
            else
            {
                this.ShowStats();
                this.HideTopPanel();
            }
        }

        public void HideBottomPanel()
        {
            base.GetComponent<Animator>().SetBool("showBottomPanel", false);
        }

        public void HideTopPanel()
        {
            base.GetComponent<Animator>().SetBool("showTopPanel", false);
        }

        private void OnDisable()
        {
            this.CurrentPart = ResultScreenParts.None;
            foreach (GameObject obj2 in this.screenParts)
            {
                obj2.SetActive(false);
            }
        }

        public void ShowAwards()
        {
            this.CurrentPart = ResultScreenParts.Awards;
            this.topPanelButtons.ActivateButton(1);
        }

        public void ShowBestPlayer()
        {
            this.HideTopPanel();
            this.HideBottomPanel();
            this.CurrentPart = ResultScreenParts.BestPlayer;
            this.topPanelButtons.ActivateButton(0);
            MVPScreenUIComponent.ShowCounter++;
        }

        public void ShowBottomPanel()
        {
            base.GetComponent<Animator>().SetBool("showBottomPanel", true);
            this.bottomPanelButtons.BattleSeriesResult.SetActive((!this.spectator && !this.customBattle) && !this.tutor);
            this.bottomPanelButtons.TryAgainButton.SetActive((!this.spectator && (!this.customBattle && (!this.tutor && !this.squad))) && this.enoughEnergy);
            this.bottomPanelButtons.MainScreenButton.gameObject.SetActive(this.spectator || !this.customBattle);
            this.bottomPanelButtons.ContinueButton.gameObject.SetActive(!this.spectator && this.customBattle);
        }

        public void ShowScreen(bool customBattle, bool spectator, bool tutor, bool squad, bool enoughEnergy)
        {
            this.customBattle = customBattle;
            this.spectator = spectator;
            this.tutor = tutor;
            this.squad = squad;
            this.enoughEnergy = enoughEnergy;
            if (customBattle)
            {
                this.ShowStats();
            }
            else
            {
                this.ShowBestPlayer();
                MVPScreenUIComponent.ShowCounter = 0;
            }
        }

        public void ShowStats()
        {
            this.CurrentPart = ResultScreenParts.Stats;
            this.ShowBottomPanel();
            if (!this.customBattle)
            {
                this.ShowTopPanel();
                this.topPanelButtons.ActivateButton(2);
            }
        }

        public void ShowTopPanel()
        {
            base.GetComponent<Animator>().SetBool("showTopPanel", true);
        }

        private ResultScreenParts CurrentPart
        {
            get => 
                this.currentPart;
            set
            {
                this.currentPart = value;
                base.GetComponent<Animator>().SetInteger("currentScreen", (int) value);
            }
        }

        private enum ResultScreenParts
        {
            None = -1,
            BestPlayer = 0,
            Awards = 1,
            Stats = 2
        }
    }
}

