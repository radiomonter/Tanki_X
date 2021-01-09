namespace Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class ModuleContainerRewardSystem : ECSSystem
    {
        [OnEventFire]
        public void BuyContainer(ButtonClickEvent e, SingleNode<SpecialOfferCrystalButtonComponent> button, [JoinAll] SingleNode<BattleResultsComponent> battleResults, [JoinAll] SingleNode<BattleResultsAwardsScreenComponent> screen)
        {
            <BuyContainer>c__AnonStorey0 storey = new <BuyContainer>c__AnonStorey0 {
                screen = screen,
                itemId = battleResults.component.ResultForClient.PersonalResult.Reward.GetComponent<ModuleContainerPersonalBattleRewardComponent>().СontainerId
            };
            storey.regularPrice = Flow.Current.EntityRegistry.GetEntity(storey.itemId).GetComponent<XPriceItemComponent>().Price;
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(storey.itemId);
            Object.FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(item, new Action(storey.<>m__0), storey.regularPrice, 1, item.Name, true, null);
        }

        [OnEventFire]
        public void ShowModuleContainerReward(ShowRewardEvent e, SingleNode<BattleResultsAwardsScreenComponent> screen, SingleNode<ModuleContainerPersonalBattleRewardComponent> personalReward, [JoinBy(typeof(BattleRewardGroupComponent))] ModuleContainerRewardNote battleReward, [JoinAll] SingleNode<BattleResultsComponent> battleResults)
        {
            string name = battleReward.descriptionItem.Name;
            string descriptionText = string.Empty;
            descriptionText = (battleResults.component.ResultForClient.PersonalResult.TeamBattleResult != TeamBattleResult.WIN) ? battleReward.moduleContainerRewardTextConfig.LooseText : battleReward.moduleContainerRewardTextConfig.WinText;
            long id = personalReward.component.СontainerId;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(id);
            List<SpecialOfferItem> items = new List<SpecialOfferItem> {
                new SpecialOfferItem(0, entity.GetComponent<ImageItemComponent>().SpriteUid, entity.GetComponent<DescriptionItemComponent>().Name)
            };
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.component.specialOfferUI;
            specialOfferUI.ShowContent(name, descriptionText, items);
            specialOfferUI.SetCrystalButton(0, entity.GetComponent<XPriceItemComponent>().Price, 0, true);
            specialOfferUI.Appear();
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <BuyContainer>c__AnonStorey0
        {
            internal SingleNode<BattleResultsAwardsScreenComponent> screen;
            internal int regularPrice;
            internal long itemId;

            internal void <>m__0()
            {
                <BuyContainer>c__AnonStorey1 storey = new <BuyContainer>c__AnonStorey1 {
                    <>f__ref$0 = this,
                    ui = this.screen.component.specialOfferUI
                };
                Action onOpen = new Action(storey.<>m__0);
                storey.ui.SetOpenButton(this.itemId, 1, onOpen);
            }

            private sealed class <BuyContainer>c__AnonStorey1
            {
                internal BattleResultSpecialOfferUiComponent ui;
                internal ModuleContainerRewardSystem.<BuyContainer>c__AnonStorey0 <>f__ref$0;

                internal void <>m__0()
                {
                    this.ui.SetCrystalButton(0, this.<>f__ref$0.regularPrice, 0, true);
                }
            }
        }

        public class ModuleContainerRewardNote : Node
        {
            public ModuleContainerRewardTextConfigComponent moduleContainerRewardTextConfig;
            public DescriptionItemComponent descriptionItem;
        }
    }
}

