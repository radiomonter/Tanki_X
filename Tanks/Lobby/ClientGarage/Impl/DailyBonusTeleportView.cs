namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class DailyBonusTeleportView : MonoBehaviour
    {
        public List<Image> teleports;
        public GameObject yelloCrystal;
        public GameObject brokenCrystal;
        public GameObject crystalOutline;
        public Button getNewTeleportButton;
        public Image fill;
        public GameObject upgradeTeleportView;
        public InactiveTeleportView inactiveTeleportView;
        public ActiveTeleportView activeTeleportView;
        public DetailTargetTeleportView detailTargetTeleportView;
        public GameObject brokenTeleport;
        public List<Image> lines;
        public Color activeColor;
        private GameObject currentStateView;
        private bool currentViewIsHiding;
        public Action<DailyBonusTeleportState> onStateChanged;
        private DailyBonusTeleportState state;

        private void ChangeState(GameObject newStateView, DailyBonusTeleportState newState)
        {
            <ChangeState>c__AnonStorey0 storey = new <ChangeState>c__AnonStorey0 {
                newStateView = newStateView,
                $this = this
            };
            if (this.currentStateView == null)
            {
                this.ShowStateView(storey.newStateView);
            }
            else
            {
                this.currentStateView.GetComponent<AnimationEventListener>().SetHideHandler(new Action(storey.<>m__0));
                this.currentStateView.GetComponent<Animator>().SetTrigger("hide");
                this.currentViewIsHiding = true;
            }
            if (this.state != newState)
            {
                this.state = newState;
                if (this.onStateChanged != null)
                {
                    this.onStateChanged(this.state);
                }
            }
        }

        private bool IsChargedState(DailyBonusTeleportState state) => 
            ((state == DailyBonusTeleportState.Active) || (state == DailyBonusTeleportState.Upgradable)) || (state == DailyBonusTeleportState.DetailTarget);

        public void OnEnable()
        {
            this.state = DailyBonusTeleportState.None;
            this.currentStateView = null;
            this.getNewTeleportButton.gameObject.SetActive(false);
            this.upgradeTeleportView.SetActive(false);
            this.inactiveTeleportView.gameObject.SetActive(false);
            this.activeTeleportView.gameObject.SetActive(false);
            this.detailTargetTeleportView.gameObject.SetActive(false);
            this.brokenTeleport.SetActive(false);
            this.fill.gameObject.SetActive(false);
            this.yelloCrystal.SetActive(false);
            this.brokenCrystal.SetActive(false);
        }

        public void SetActiveView(int zoneIndex)
        {
            this.SetTeleportCircleView(zoneIndex);
            this.UpdateColorElements(DailyBonusTeleportState.Active);
            this.ShowYelloCrystal();
            this.ChangeState(this.activeTeleportView.gameObject, DailyBonusTeleportState.Active);
            this.activeTeleportView.UpdateView();
        }

        public void SetBrokenView()
        {
            foreach (Image image in this.teleports)
            {
                image.gameObject.SetActive(false);
            }
            this.brokenTeleport.SetActive(true);
            this.UpdateColorElements(DailyBonusTeleportState.Broken);
            this.ShowBrokenCrystal();
            this.ChangeState(this.getNewTeleportButton.gameObject, DailyBonusTeleportState.Broken);
        }

        public void SetDetailTargetView(int zoneIndex, DailyBonusGarageItemReward detailGarageItem)
        {
            this.SetTeleportCircleView(zoneIndex);
            this.UpdateColorElements(DailyBonusTeleportState.DetailTarget);
            this.ShowYelloCrystal();
            this.ChangeState(this.detailTargetTeleportView.gameObject, DailyBonusTeleportState.DetailTarget);
            this.detailTargetTeleportView.UpdateView(detailGarageItem);
        }

        public void SetInactiveState(int zoneIndex, Date endDate, float durationInSec)
        {
            this.SetTeleportCircleView(zoneIndex);
            this.UpdateColorElements(DailyBonusTeleportState.Inactive);
            this.ShowYelloCrystal();
            bool successTeleportation = (this.state == DailyBonusTeleportState.Active) || (this.state == DailyBonusTeleportState.DetailTarget);
            this.inactiveTeleportView.UpdateView(endDate, durationInSec, successTeleportation);
            this.ChangeState(this.inactiveTeleportView.gameObject, DailyBonusTeleportState.Inactive);
        }

        private void SetTeleportCircleView(int zoneIndex)
        {
            this.brokenTeleport.SetActive(false);
            zoneIndex = Math.Min(zoneIndex, this.teleports.Count - 1);
            if (!this.teleports[zoneIndex].gameObject.activeSelf)
            {
                foreach (Image image in this.teleports)
                {
                    image.gameObject.SetActive(false);
                }
                this.teleports[zoneIndex].gameObject.SetActive(true);
            }
            this.yelloCrystal.SetActive(true);
            this.SetZeroImageAlpha(this.yelloCrystal.GetComponent<Image>());
            this.yelloCrystal.GetComponent<Animator>().SetTrigger("show");
            this.brokenCrystal.GetComponent<AnimationEventListener>().SetHideHandler(() => this.brokenCrystal.SetActive(false));
            this.brokenCrystal.GetComponent<Animator>().SetTrigger("hide");
        }

        public void SetUpgradableView(int zoneIndex)
        {
            this.SetTeleportCircleView(zoneIndex);
            this.UpdateColorElements(DailyBonusTeleportState.Upgradable);
            this.fill.gameObject.SetActive(true);
            this.fill.fillAmount = 1f;
            this.ShowYelloCrystal();
            this.ChangeState(this.upgradeTeleportView, DailyBonusTeleportState.Upgradable);
        }

        private void SetZeroImageAlpha(Image image)
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
        }

        private void ShowBrokenCrystal()
        {
            this.SetZeroImageAlpha(this.brokenCrystal.GetComponent<Image>());
            this.brokenCrystal.SetActive(true);
            this.brokenCrystal.GetComponent<Animator>().SetTrigger("show");
            this.yelloCrystal.GetComponent<AnimationEventListener>().SetHideHandler(() => this.yelloCrystal.SetActive(false));
            this.yelloCrystal.GetComponent<Animator>().SetTrigger("hide");
        }

        private void ShowStateView(GameObject newStateView)
        {
            newStateView.GetComponent<CanvasGroup>().alpha = 0f;
            newStateView.SetActive(true);
            this.currentStateView = newStateView;
            this.currentViewIsHiding = false;
        }

        private void ShowYelloCrystal()
        {
            if (!this.yelloCrystal.activeSelf)
            {
                this.yelloCrystal.SetActive(true);
            }
            this.brokenCrystal.GetComponent<Animator>().SetTrigger("hide");
            this.yelloCrystal.GetComponent<Animator>().SetTrigger("show");
        }

        private void UpdateColorElements(DailyBonusTeleportState state)
        {
            switch (state)
            {
                case DailyBonusTeleportState.None:
                case DailyBonusTeleportState.Inactive:
                case DailyBonusTeleportState.Broken:
                    this.crystalOutline.GetComponent<Animator>().SetTrigger("hide");
                    this.crystalOutline.GetComponent<AnimationEventListener>().SetHideHandler(() => this.crystalOutline.SetActive(false));
                    this.fill.gameObject.SetActive(false);
                    break;

                default:
                    this.crystalOutline.SetActive(true);
                    this.fill.gameObject.SetActive(true);
                    break;
            }
            foreach (Image image in this.teleports)
            {
                image.GetComponent<Animator>().SetBool("yello", this.IsChargedState(state));
            }
            foreach (Image image2 in this.lines)
            {
                image2.GetComponent<Animator>().SetBool("yello", this.IsChargedState(state));
            }
        }

        public void ViewSelectedBonus(MapViewBonusElement element)
        {
            if (!this.currentViewIsHiding && (this.activeTeleportView.gameObject == this.currentStateView))
            {
                this.activeTeleportView.ViewBonus(element);
            }
        }

        public DailyBonusTeleportState State =>
            this.state;

        [CompilerGenerated]
        private sealed class <ChangeState>c__AnonStorey0
        {
            internal GameObject newStateView;
            internal DailyBonusTeleportView $this;

            internal void <>m__0()
            {
                this.$this.currentStateView.SetActive(false);
                this.$this.ShowStateView(this.newStateView);
            }
        }
    }
}

