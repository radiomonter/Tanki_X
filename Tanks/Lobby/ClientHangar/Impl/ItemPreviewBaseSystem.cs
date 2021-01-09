namespace Tanks.Lobby.ClientHangar.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;

    public class ItemPreviewBaseSystem : ECSSystem
    {
        protected void PreviewItem(Entity item)
        {
            base.ScheduleEvent<PrewievEvent>(item);
        }

        public class GraffitiPreviewNode : ItemPreviewBaseSystem.PreviewNode
        {
            public GraffitiItemComponent graffitiItem;
        }

        public class HulLPreviewNode : Node
        {
            public TankItemComponent tankItem;
        }

        public class MountedUserItemNode : ItemPreviewBaseSystem.UserItemNode
        {
            public MountedItemComponent mountedItem;
        }

        [Not(typeof(GraffitiItemComponent))]
        public class NotGraffitiNode : Node
        {
            public GarageItemComponent garageItem;
        }

        public class PreviewNode : Node
        {
            public GarageItemComponent garageItem;
            public HangarItemPreviewComponent hangarItemPreview;
        }

        public class PrewievEvent : Event
        {
        }

        public class UserItemNode : Node
        {
            public UserGroupComponent userGroup;
            public UserItemComponent userItem;
            public GarageItemComponent garageItem;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class WeaponNotPreviewNode : Node
        {
            public WeaponItemComponent weaponItem;
        }
    }
}

