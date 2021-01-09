namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TankPartItem : GarageItem
    {
        private List<VisualItem> skins = new List<VisualItem>();

        public int UpgradeLevel =>
            (base.UserItem != null) ? base.UserItem.GetComponent<UpgradeLevelItemComponent>().Level : 0;

        public int AbsExperience
        {
            get
            {
                ExperienceToLevelUpItemComponent component = base.UserItem.GetComponent<ExperienceToLevelUpItemComponent>();
                return (component.FinalLevelExperience - component.RemainingExperience);
            }
        }

        public ExperienceToLevelUpItemComponent Experience =>
            base.UserItem.GetComponent<ExperienceToLevelUpItemComponent>();

        public string Feature =>
            this.MarketItem.GetComponent<VisualPropertiesComponent>().Feature;

        public List<MainVisualProperty> MainProperties =>
            this.MarketItem.GetComponent<VisualPropertiesComponent>().MainProperties;

        public List<VisualProperty> Properties =>
            this.MarketItem.GetComponent<VisualPropertiesComponent>().Properties;

        public TankPartItemType Type { get; private set; }

        public override Entity MarketItem
        {
            get => 
                base.MarketItem;
            set
            {
                base.MarketItem = value;
                if (value.HasComponent<WeaponItemComponent>())
                {
                    this.Type = TankPartItemType.Turret;
                }
                else
                {
                    if (!value.HasComponent<TankItemComponent>())
                    {
                        throw new Exception("Invalid tank part type");
                    }
                    this.Type = TankPartItemType.Hull;
                }
            }
        }

        public ICollection<VisualItem> Skins =>
            this.skins;

        public enum TankPartItemType
        {
            Turret,
            Hull
        }
    }
}

