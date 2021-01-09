namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class GarageItemsScreenTextComponent : Component
    {
        public string MountedText { get; set; }

        public string UserRankRestrictionDescription { get; set; }

        public string UpgradeLevelRestrictionDescription { get; set; }

        public string WeaponUpgradeLevelRestrictionDescription { get; set; }

        public string HullUpgradeLevelRestrictionDescription { get; set; }

        public string ShellItemsHeaderText { get; set; }

        public string WeaponSkinItemsHeaderText { get; set; }

        public string HullSkinItemsHeaderText { get; set; }

        public string GraffitiItemsHeaderText { get; set; }

        public string OnlyInContainerText { get; set; }
    }
}

