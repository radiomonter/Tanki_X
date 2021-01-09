namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class GarageListItemContentSystem : ECSSystem
    {
        [OnEventFire]
        public void EnablePrice(NodeAddedEvent e, GarageListItemPriceNode item, [JoinAll] ScreenWithVisibleItemsInfoNode screen)
        {
            if (item.priceItem.IsBuyable)
            {
                item.garageListItemContent.PriceGameObject.SetActive(true);
            }
            else if (item.xPriceItem.IsBuyable)
            {
                item.garageListItemContent.XPriceGameObject.SetActive(true);
            }
        }

        [OnEventFire]
        public void EnableUpgrade(NodeAddedEvent e, GarageListUserUpgradeItemNode item, [JoinAll] ScreenWithVisibleItemsInfoNode screen)
        {
            item.garageListItemContent.UpgradeGameObject.SetActive(true);
        }

        [OnEventFire]
        public void ResetNonImagedItem(NodeAddedEvent e, GarageListNonImagedMarketItemNode listItemNode, [JoinByParentGroup] DefaultSkinNode skin)
        {
            string spriteUid = skin.imageItem.SpriteUid;
            listItemNode.garageListItemContent.AddPreview(spriteUid);
        }

        [OnEventFire]
        public void ResetNonImagedItem(NodeAddedEvent e, GarageListNonImagedUserItemNode listItemNode, [JoinByParentGroup] MountedSkinNode skin)
        {
            string spriteUid = skin.imageItem.SpriteUid;
            listItemNode.garageListItemContent.AddPreview(spriteUid);
        }

        [OnEventFire]
        public void SetContainerBundleItemImages(NodeAddedEvent e, GarageListContainerBundleItemNode listItemNode)
        {
            foreach (MarketItemBundle bundle in listItemNode.bundleContainerContentItem.MarketItems)
            {
                Entity entity = Flow.Current.EntityRegistry.GetEntity(bundle.MarketItem);
                if (entity.HasComponent<ImageItemComponent>())
                {
                    listItemNode.garageListItemContent.AddPreview(entity.GetComponent<ImageItemComponent>().SpriteUid, (long) bundle.Amount);
                }
            }
            if (listItemNode.descriptionBundleItem.Names.ContainsKey(listItemNode.bundleContainerContentItem.NameLokalizationKey))
            {
                listItemNode.garageListItemContent.Header.text = listItemNode.descriptionBundleItem.Names[listItemNode.bundleContainerContentItem.NameLokalizationKey];
            }
        }

        [OnEventFire]
        public void SetContainerItemImage(NodeAddedEvent e, GarageListContainerSimpleItemNode listItemNode)
        {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(listItemNode.simpleContainerContentItem.MarketItemId);
            if (entity.HasComponent<ImageItemComponent>())
            {
                listItemNode.garageListItemContent.AddPreview(entity.GetComponent<ImageItemComponent>().SpriteUid);
            }
            listItemNode.garageListItemContent.Header.text = ((listItemNode.simpleContainerContentItem.NameLokalizationKey == null) || !listItemNode.descriptionBundleItem.Names.ContainsKey(listItemNode.simpleContainerContentItem.NameLokalizationKey)) ? entity.GetComponent<DescriptionItemComponent>().Name : listItemNode.descriptionBundleItem.Names[listItemNode.simpleContainerContentItem.NameLokalizationKey];
        }

        [OnEventFire]
        public void SetContentSlotItem(NodeAddedEvent e, GarageListSlotItemNode slot, [JoinAll] SingleNode<SlotsTextsComponent> slotsTexts, [JoinAll] SingleNode<ModuleTypesImagesComponent> moduleTypesImages)
        {
            slot.garageListItemContent.Header.text = slotsTexts.component.Slot2modules[slot.slotUserItemInfo.ModuleBehaviourType];
            GarageListItemContentPreviewComponent component = slot.garageListItemContent.AddPreview(moduleTypesImages.component.moduleType2image[slot.slotUserItemInfo.ModuleBehaviourType]);
            Color color = new Color();
            ColorUtility.TryParseHtmlString(moduleTypesImages.component.moduleType2color[slot.slotUserItemInfo.ModuleBehaviourType], out color);
            component.Image.color = color;
        }

        [OnEventFire]
        public void SetDescriptionItem(ListItemSelectedEvent e, GarageItemNode item, [JoinAll] ItemDescriptionNode descriptionNode)
        {
            descriptionNode.displayDescriptionItem.SetDescription(item.descriptionItem.Description);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListNotChildItemNode itemNode, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            topPanel.component.NewHeader = itemNode.descriptionItem.Name;
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListContainerSimpleItemNode listItemNode, [JoinAll] ContainerContentScreenNode screen, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(listItemNode.simpleContainerContentItem.MarketItemId);
            topPanel.component.NewHeader = entity.GetComponent<DescriptionItemComponent>().Name;
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListGraffitiItemNode itemNode, [JoinAll] GarageItemsScreenNode screen, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.GraffitiItemsHeaderText, new object[0]);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, SingleNode<SkinItemComponent> itemNode, [JoinByParentGroup] HullMarketItemNode hullMarketItemNode, [JoinAll] GarageItemsScreenNode screen, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.HullSkinItemsHeaderText, hullMarketItemNode.descriptionItem.Name);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, SingleNode<SkinItemComponent> itemNode, [JoinByParentGroup] WeaponMarketItemNode weaponMarketItem, [JoinAll] GarageItemsScreenNode screen, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.WeaponSkinItemsHeaderText, weaponMarketItem.descriptionItem.Name);
        }

        [OnEventFire]
        public void SetHeaderOnFirstSelectItem(SelectGarageItemEvent e, GarageListShellItemNode itemNode, [JoinByParentGroup] WeaponMarketItemNode weaponMarketItem, [JoinAll] GarageItemsScreenNode screen, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            topPanel.component.NewHeader = string.Format(screen.garageItemsScreenText.ShellItemsHeaderText, weaponMarketItem.descriptionItem.Name);
        }

        [OnEventFire]
        public void SetImageItem(NodeAddedEvent e, GarageListImagedItemNode listItemNode)
        {
            string spriteUid = listItemNode.imageItem.SpriteUid;
            if (!string.IsNullOrEmpty(spriteUid))
            {
                listItemNode.garageListItemContent.AddPreview(spriteUid);
            }
        }

        [OnEventFire]
        public void SetItemName(NodeAddedEvent e, GarageListItemNode listItemNode)
        {
            DescriptionItemComponent descriptionItem = listItemNode.descriptionItem;
            listItemNode.garageListItemContent.Header.text = descriptionItem.Name;
        }

        [OnEventFire]
        public void SetPrice(NodeAddedEvent e, GarageListItemPriceNode item)
        {
            SetPriceEvent eventInstance = new SetPriceEvent {
                Price = item.priceItem.Price,
                OldPrice = item.priceItem.OldPrice,
                XPrice = item.xPriceItem.Price,
                OldXPrice = item.xPriceItem.OldPrice
            };
            base.ScheduleEvent(eventInstance, item);
        }

        [OnEventFire]
        public void SetUpgradeLevel(NodeAddedEvent e, GarageListItemUpgradeNode item)
        {
            item.garageListItemContent.UpgradeLevel.text = item.upgradeLevelItem.Level.ToString();
            float num2 = item.experienceToLevelUpItem.FinalLevelExperience - item.experienceToLevelUpItem.InitLevelExperience;
            float num4 = (num2 - item.experienceToLevelUpItem.RemainingExperience) / num2;
            item.garageListItemContent.ProgressBar.ProgressValue = num4;
            item.garageListItemContent.Arrow.gameObject.SetActive(item.upgradeLevelItem.Level > item.upgradeLevelItem.Level);
        }

        [OnEventFire]
        public void ShowRareText(NodeAddedEvent e, RareGarageListItemNode item)
        {
            item.garageListItemContent.RareTextVisibility = true;
        }

        [OnEventFire]
        public void UpdateHeader(ListItemSelectedEvent e, GarageListContainerBundleItemNode listItemNode, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            topPanel.component.CurrentHeader = listItemNode.descriptionBundleItem.Names[listItemNode.bundleContainerContentItem.NameLokalizationKey];
        }

        [OnEventFire]
        public void UpdateHeader(ListItemSelectedEvent e, GarageListContainerSimpleItemNode listItemNode, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(listItemNode.simpleContainerContentItem.MarketItemId);
            if (!entity.HasComponent<PaintItemComponent>())
            {
                topPanel.component.CurrentHeader = entity.GetComponent<DescriptionItemComponent>().Name;
            }
            else if (!string.IsNullOrEmpty(listItemNode.simpleContainerContentItem.NameLokalizationKey))
            {
                topPanel.component.CurrentHeader = listItemNode.descriptionBundleItem.Names[listItemNode.simpleContainerContentItem.NameLokalizationKey];
            }
        }

        [OnEventFire]
        public void UpdateHeader(ListItemSelectedEvent e, GarageListNotChildItemNode item, [JoinAll] SingleNode<TopPanelComponent> topPanel)
        {
            topPanel.component.CurrentHeader = item.descriptionItem.Name;
        }

        public class ContainerContentScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public ContainerContentScreenComponent containerContentScreen;
        }

        public class DefaultSkinNode : GarageListItemContentSystem.SkinNode
        {
            public DefaultSkinItemComponent defaultSkinItem;
        }

        public class GarageItemNode : Node
        {
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
        }

        public class GarageItemsScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenTextComponent garageItemsScreenText;
        }

        public class GarageListContainerBundleItemNode : Node
        {
            public BundleContainerContentItemComponent bundleContainerContentItem;
            public GarageListItemContentComponent garageListItemContent;
            public DescriptionBundleItemComponent descriptionBundleItem;
        }

        public class GarageListContainerSimpleItemNode : Node
        {
            public SimpleContainerContentItemComponent simpleContainerContentItem;
            public GarageListItemContentComponent garageListItemContent;
            public DescriptionBundleItemComponent descriptionBundleItem;
        }

        public class GarageListGraffitiItemNode : Node
        {
            public GraffitiItemComponent graffitiItem;
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
            public GarageListItemComponent garageListItem;
        }

        public class GarageListImagedItemNode : GarageListItemContentSystem.GarageListItemNode
        {
            public ImageItemComponent imageItem;
        }

        public class GarageListItemNode : Node
        {
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
            public GarageListItemContentComponent garageListItemContent;
        }

        public class GarageListItemPriceNode : Node
        {
            public MarketItemComponent marketItem;
            public GarageListItemContentComponent garageListItemContent;
            public PriceItemComponent priceItem;
            public XPriceItemComponent xPriceItem;
            public PriceLabelComponent priceLabel;
            public XPriceLabelComponent xPriceLabel;
        }

        public class GarageListItemUpgradeNode : Node
        {
            public GarageListItemContentComponent garageListItemContent;
            public UpgradeLevelItemComponent upgradeLevelItem;
            public ExperienceToLevelUpItemComponent experienceToLevelUpItem;
        }

        [Not(typeof(ImageItemComponent))]
        public class GarageListNonImagedItemNode : GarageListItemContentSystem.GarageListItemNode
        {
        }

        public class GarageListNonImagedMarketItemNode : GarageListItemContentSystem.GarageListNonImagedItemNode
        {
            public MarketItemComponent marketItem;
        }

        public class GarageListNonImagedUserItemNode : GarageListItemContentSystem.GarageListNonImagedItemNode
        {
            public UserItemComponent userItem;
        }

        [Not(typeof(GraffitiItemComponent)), Not(typeof(SkinItemComponent)), Not(typeof(ShellItemComponent))]
        public class GarageListNotChildItemNode : Node
        {
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
            public GarageListItemComponent garageListItem;
        }

        public class GarageListShellItemNode : Node
        {
            public ShellItemComponent shellItem;
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
            public GarageListItemComponent garageListItem;
        }

        public class GarageListSlotItemNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public SlotTankPartComponent slotTankPart;
            public GarageListItemComponent garageListItem;
            public GarageListItemContentComponent garageListItemContent;
        }

        public class GarageListUserUpgradeItemNode : Node
        {
            public UserItemComponent userItem;
            public GarageListItemContentComponent garageListItemContent;
        }

        public class HullMarketItemNode : Node
        {
            public MarketItemComponent marketItem;
            public TankItemComponent tankItem;
            public ParentGroupComponent parentGroup;
            public DescriptionItemComponent descriptionItem;
        }

        public class ItemDescriptionNode : Node
        {
            public ItemsListScreenComponent itemsListScreen;
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
            public DisplayDescriptionItemComponent displayDescriptionItem;
        }

        public class MountedSkinNode : GarageListItemContentSystem.SkinNode
        {
            public MountedItemComponent mountedItem;
        }

        public class RareGarageListItemNode : Node
        {
            public RareContainerContentItemComponent rareContainerContentItem;
            public GarageListItemContentComponent garageListItemContent;
        }

        [Not(typeof(ContainerContentScreenComponent)), Not(typeof(ContainersScreenComponent))]
        public class ScreenWithVisibleItemsInfoNode : Node
        {
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }

        public class SkinNode : Node
        {
            public SkinItemComponent skinItem;
            public ImageItemComponent imageItem;
        }

        public class WeaponMarketItemNode : Node
        {
            public MarketItemComponent marketItem;
            public WeaponItemComponent weaponItem;
            public ParentGroupComponent parentGroup;
            public DescriptionItemComponent descriptionItem;
        }
    }
}

