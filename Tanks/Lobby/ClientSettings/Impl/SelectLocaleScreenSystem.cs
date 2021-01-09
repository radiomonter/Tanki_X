namespace Tanks.Lobby.ClientSettings.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientLocale.API;
    using Platform.Library.ClientLocale.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientSettings.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class SelectLocaleScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void Apply(ButtonClickEvent e, SingleNode<ApplyButtonComponent> button, [JoinByScreen] SingleNode<SelectLocaleScreenComponent> screen, SingleNode<ApplyButtonComponent> buttonA, [JoinByScreen] SelectedLocaleNode selected)
        {
            LocaleUtils.SaveLocaleCode(selected.selectedLocale.Code);
            base.ScheduleEvent<SwitchToEntranceSceneEvent>(button);
        }

        [OnEventFire]
        public void ClearLocaleList(NodeRemoveEvent e, LocaleListNode list)
        {
            list.simpleHorizontalList.ClearItems(false);
        }

        [OnEventFire]
        public void CreateLocaleEntities(NodeAddedEvent e, LocaleListNode node, [JoinAll] UserNode user)
        {
            foreach (string str in node.localeList.Locales)
            {
                char[] separator = new char[] { '/' };
                string str2 = str.Split(separator)[1];
                if (!"tr".Equals(str2) || (user.userPublisher.Publisher == Publisher.CONSALA))
                {
                    Entity entity = base.CreateEntity<LocaleTemplate>(str);
                    Debug.Log("Language accepted!");
                    node.simpleHorizontalList.AddItem(entity);
                    entity.AddComponent(new ScreenGroupComponent(node.screenGroup.Key));
                }
            }
        }

        [OnEventFire]
        public void DestroyLocaleEntities(NodeRemoveEvent e, SingleNode<SelectLocaleScreenComponent> screen, [JoinAll] ICollection<SingleNode<LocaleComponent>> locales)
        {
            foreach (SingleNode<LocaleComponent> node in locales)
            {
                base.DeleteEntity(node.Entity);
            }
            screen.component.DisableButtons();
        }

        [OnEventFire]
        public void InitLocaleItem(ListItemSelectedEvent e, LocaleItemNode node, [JoinByScreen] SelectedLocaleNode selected, LocaleItemNode nodeA, [JoinByScreen] SingleNode<SelectLocaleScreenComponent> screen)
        {
            selected.selectedLocale.Code = node.locale.Code;
            if (node.locale.Code == LocaleUtils.GetSavedLocaleCode())
            {
                screen.component.DisableButtons();
            }
            else
            {
                screen.component.EnableButtons();
            }
        }

        [OnEventFire]
        public void InitSelectedLocaleItem(NodeAddedEvent e, LocaleItemNode node, [Context, JoinByScreen] SelectedLocaleNode selected, [Context, JoinByScreen] LocaleListNode localesList)
        {
            LocaleComponent locale = node.locale;
            node.localeItem.SetText(locale.Caption, locale.LocalizedCaption);
            string savedLocaleCode = LocaleUtils.GetSavedLocaleCode();
            if (locale.Code == savedLocaleCode)
            {
                this.SetLocaleText(selected, node.locale);
                localesList.simpleHorizontalList.Select(node.Entity);
            }
        }

        private void SetLocaleText(SelectedLocaleNode destination, LocaleComponent source)
        {
            destination.selectedLocale.Code = source.Code;
        }

        public class LocaleItemNode : Node
        {
            public LocaleComponent locale;
            public LocaleItemComponent localeItem;
            public ScreenGroupComponent screenGroup;
        }

        public class LocaleListNode : Node
        {
            public LocaleListComponent localeList;
            public SimpleHorizontalListComponent simpleHorizontalList;
            public ScreenGroupComponent screenGroup;
        }

        public class SelectedLocaleNode : Node
        {
            public SelectedLocaleComponent selectedLocale;
            public ScreenGroupComponent screenGroup;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserPublisherComponent userPublisher;
        }
    }
}

