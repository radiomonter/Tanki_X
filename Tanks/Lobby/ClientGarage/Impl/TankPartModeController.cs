namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;
    using UnityEngine.Events;

    public class TankPartModeController
    {
        private TankPartCollectionView turretCollectionView;
        private TankPartCollectionView hullCollectionView;
        private CollectionView collectionView;
        private TankPartModuleType currentMode;
        public Action onModeChange;

        public TankPartModeController(TankPartCollectionView turretCollectionView, TankPartCollectionView hullCollectionView, CollectionView collectionView)
        {
            this.turretCollectionView = turretCollectionView;
            this.hullCollectionView = hullCollectionView;
            this.collectionView = collectionView;
            turretCollectionView.GetComponent<SimpleClickHandler>().onClick = new Action<GameObject>(this.OnTurretClick);
            hullCollectionView.GetComponent<SimpleClickHandler>().onClick = new Action<GameObject>(this.OnHullClick);
            collectionView.turretToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnTurretToggleValueChanged));
            collectionView.hullToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnHullToggleValueChanged));
            collectionView.turretToggle.isOn = true;
            collectionView.hullToggle.isOn = false;
            this.currentMode = TankPartModuleType.WEAPON;
            this.UpdateView();
        }

        public TankPartModuleType GetMode() => 
            this.currentMode;

        private void OnHullClick(GameObject gameObject)
        {
            this.SetMode(TankPartModuleType.TANK);
        }

        private void OnHullToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                this.SetMode(TankPartModuleType.TANK);
            }
        }

        private void OnTurretClick(GameObject gameObject)
        {
            this.SetMode(TankPartModuleType.WEAPON);
        }

        private void OnTurretToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                this.SetMode(TankPartModuleType.WEAPON);
            }
        }

        public void SetMode(TankPartModuleType tankPartMode)
        {
            if (tankPartMode != this.currentMode)
            {
                this.currentMode = tankPartMode;
                this.UpdateView();
                if (this.onModeChange != null)
                {
                    this.onModeChange();
                }
            }
        }

        public void UpdateView()
        {
            this.collectionView.SwitchMode(this.currentMode);
            if (this.currentMode == TankPartModuleType.WEAPON)
            {
                this.turretCollectionView.GetComponent<Animator>().SetBool("highlighted", true);
                this.turretCollectionView.slotContainer.blocksRaycasts = true;
                this.turretCollectionView.GetComponent<CanvasGroup>().interactable = false;
                this.hullCollectionView.GetComponent<Animator>().SetBool("highlighted", false);
                this.hullCollectionView.slotContainer.blocksRaycasts = false;
                this.hullCollectionView.GetComponent<CanvasGroup>().interactable = true;
            }
            else
            {
                this.turretCollectionView.GetComponent<Animator>().SetBool("highlighted", false);
                this.turretCollectionView.slotContainer.blocksRaycasts = false;
                this.turretCollectionView.GetComponent<CanvasGroup>().interactable = true;
                this.hullCollectionView.GetComponent<Animator>().SetBool("highlighted", true);
                this.hullCollectionView.slotContainer.blocksRaycasts = true;
                this.hullCollectionView.GetComponent<CanvasGroup>().interactable = false;
            }
            Cursors.SwitchToDefaultCursor();
        }
    }
}

