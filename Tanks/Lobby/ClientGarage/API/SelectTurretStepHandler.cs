namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.Events;

    public class SelectTurretStepHandler : TutorialStepHandler
    {
        [SerializeField]
        private Carousel carousel;
        private GarageItem selectedItem;

        private void Complete(GarageItemUI garageItem)
        {
            if (ReferenceEquals(garageItem.Item, this.selectedItem))
            {
                TutorialCanvas.Instance.UnblockInteractable();
                base.StepComplete();
            }
        }

        public override void RunStep(TutorialData data)
        {
            TutorialCanvas.Instance.BlockInteractable();
            base.RunStep(data);
            this.selectedItem = GarageItemsRegistry.GetItem<TankPartItem>(data.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId);
            if (ReferenceEquals(this.carousel.Selected.Item, this.selectedItem))
            {
                this.Complete(this.carousel.Selected);
            }
            else
            {
                this.carousel.onItemSelected += new UnityAction<GarageItemUI>(this.Complete);
                this.carousel.Select(this.selectedItem, false);
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

