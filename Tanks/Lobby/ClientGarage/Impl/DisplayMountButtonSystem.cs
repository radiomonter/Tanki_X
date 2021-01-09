namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine.EventSystems;

    public class DisplayMountButtonSystem : ECSSystem
    {
        [OnEventFire]
        public void AddMountLabel(NodeAddedEvent e, SingleNode<MountLabelComponent> mountLabel, [JoinAll] ScreenNode screen)
        {
            mountLabel.component.GetComponent<Text>().text = screen.garageItemsScreenText.MountedText;
        }

        private void HideMountButton(ScreenNode screenNode)
        {
            screenNode.garageItemsScreen.MountLabel.gameObject.SetActive(false);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void HideMountButton(ListItemSelectedEvent e, BuyableMarketItemNode item, [JoinAll] ScreenNode screenNode)
        {
            this.HideMountButton(screenNode);
        }

        [OnEventFire]
        public void HideMountButton(ListItemSelectedEvent e, NotMountedNotSkinUserItemWithRestrictionNode item, [JoinAll] ScreenNode screenNode)
        {
            this.ShowMountButtonForRestrictedItem(screenNode);
        }

        [OnEventFire]
        public void HideMountButton(ListItemSelectedEvent e, NotMountedSkinUserItemWithRestrictionNode item, [JoinAll] ScreenNode screenNode)
        {
            this.ShowMountButtonForSkinItem(screenNode, true);
        }

        [OnEventFire]
        public void HideMountButton(NodeAddedEvent e, NotMountedUserItemWithRestrictionNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode)
        {
            if (ReferenceEquals(selectedItemNode.component.SelectedItem, item.Entity))
            {
                this.HideMountButton(screenNode);
            }
        }

        [OnEventFire]
        public void SetMountText(NodeAddedEvent e, MountItemButtonNode node)
        {
            node.textMapping.Text = node.mountItemButtonText.MountText;
        }

        [OnEventFire]
        public void ShowEquippedButton(ListItemSelectedEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode)
        {
            this.ShowMountButton(screenNode, false);
        }

        [OnEventFire]
        public void ShowEquippedButton(NodeAddedEvent e, MountedUserItemNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode)
        {
            if (ReferenceEquals(selectedItemNode.component.SelectedItem, item.Entity))
            {
                this.ShowMountButton(screenNode, false);
            }
        }

        private void ShowMountButton(ScreenNode screenNode, bool interactable)
        {
            screenNode.garageItemsScreen.MountLabel.gameObject.SetActive(!interactable);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetActive(interactable);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetInteractable(interactable);
            if (interactable)
            {
                EventSystem.current.SetSelectedGameObject(screenNode.garageItemsScreen.MountItemButton.gameObject);
            }
        }

        [OnEventFire]
        public void ShowMountButton(ListItemSelectedEvent e, NotMountedNotSkinUserItemNode item, [JoinAll] ScreenNode screenNode)
        {
            this.ShowMountButton(screenNode, true);
        }

        [OnEventFire]
        public void ShowMountButton(ListItemSelectedEvent e, NotMountedSkinUserItemNode item, [JoinAll] ScreenNode screenNode)
        {
            this.ShowMountButtonForSkinItem(screenNode, false);
        }

        [OnEventFire]
        public void ShowMountButton(NodeAddedEvent e, NotMountedUserItemNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode)
        {
            if (ReferenceEquals(selectedItemNode.component.SelectedItem, item.Entity))
            {
                this.ShowMountButton(screenNode, true);
            }
        }

        [OnEventFire]
        public void ShowMountButton(NodeRemoveEvent e, NotMountedUserItemWithRestrictionNode item, [JoinAll] ScreenNode screenNode, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItemNode)
        {
            if (ReferenceEquals(selectedItemNode.component.SelectedItem, item.Entity))
            {
                this.ShowMountButton(screenNode, true);
            }
        }

        private void ShowMountButtonForRestrictedItem(ScreenNode screenNode)
        {
            screenNode.garageItemsScreen.MountLabel.gameObject.SetActive(false);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetInteractable(false);
            screenNode.garageItemsScreen.MountItemButton.gameObject.SetActive(true);
        }

        private void ShowMountButtonForSkinItem(ScreenNode screenNode, bool hasRestriction)
        {
            if (hasRestriction)
            {
                this.ShowMountButtonForRestrictedItem(screenNode);
            }
            else
            {
                this.ShowMountButton(screenNode, true);
            }
        }

        public class BuyableMarketItemNode : Node
        {
            public MarketItemComponent marketItem;
            public PriceItemComponent priceItem;
        }

        public class MountedUserItemNode : DisplayMountButtonSystem.UserItemNode
        {
            public MountedItemComponent mountedItem;
        }

        public class MountItemButtonNode : Node
        {
            public MountItemButtonComponent mountItemButton;
            public TextMappingComponent textMapping;
            public MountItemButtonTextComponent mountItemButtonText;
        }

        public class NotMountedGraffitiUserItemWithRestrictionNode : DisplayMountButtonSystem.NotMountedUserItemWithRestrictionNode
        {
            public GraffitiItemComponent graffitiItem;
        }

        [Not(typeof(SkinItemComponent))]
        public class NotMountedNotSkinUserItemNode : DisplayMountButtonSystem.NotMountedUserItemNode
        {
        }

        [Not(typeof(SkinItemComponent))]
        public class NotMountedNotSkinUserItemWithRestrictionNode : DisplayMountButtonSystem.NotMountedUserItemWithRestrictionNode
        {
        }

        public class NotMountedSkinUserItemNode : DisplayMountButtonSystem.NotMountedUserItemNode
        {
            public SkinItemComponent skinItem;
        }

        public class NotMountedSkinUserItemWithRestrictionNode : DisplayMountButtonSystem.NotMountedUserItemWithRestrictionNode
        {
            public SkinItemComponent skinItem;
        }

        [Not(typeof(MountedItemComponent)), Not(typeof(RestrictedByUpgradeLevelComponent))]
        public class NotMountedUserItemNode : DisplayMountButtonSystem.UserItemNode
        {
        }

        [Not(typeof(MountedItemComponent))]
        public class NotMountedUserItemWithRestrictionNode : DisplayMountButtonSystem.UserItemNode
        {
            public RestrictedByUpgradeLevelComponent restrictedByUpgradeLevel;
        }

        public class ScreenNode : Node
        {
            public GarageItemsScreenComponent garageItemsScreen;
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenTextComponent garageItemsScreenText;
        }

        public class TankUserItemNode : DisplayMountButtonSystem.UserItemNode
        {
            public TankItemComponent tankItem;
        }

        public class UserItemNode : Node
        {
            public UserItemComponent userItem;
        }

        public class WeaponUserItemNode : DisplayMountButtonSystem.UserItemNode
        {
            public WeaponItemComponent weaponItem;
        }
    }
}

