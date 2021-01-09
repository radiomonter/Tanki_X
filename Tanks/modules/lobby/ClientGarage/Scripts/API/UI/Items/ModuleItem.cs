namespace tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;

    public class ModuleItem : GarageItem
    {
        public string Description() => 
            this.MarketItem.GetComponent<DescriptionItemComponent>().Description;

        public ModulePrice GetUpgradePrice(long level) => 
            (level >= this.MaxLevel) ? this.MarketItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[this.MaxLevel] : this.MarketItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[(int) level];

        public bool ImproveAvailable() => 
            (this.Level < this.MaxLevel) && (this.UserCardCount >= this.GetUpgradePrice(this.Level).Cards);

        public bool IsMutable() => 
            !this.MarketItem.HasComponent<ImmutableModuleItemComponent>();

        public bool ResearchAvailable() => 
            this.UserCardCount >= this.CraftPrice.Cards;

        public Tanks.Lobby.ClientGarage.API.ModuleBehaviourType ModuleBehaviourType =>
            this.MarketItem.GetComponent<ModuleBehaviourTypeComponent>().Type;

        public int TierNumber =>
            this.MarketItem.GetComponent<ModuleTierComponent>().TierNumber;

        public Tanks.Lobby.ClientGarage.API.TankPartModuleType TankPartModuleType =>
            this.MarketItem.GetComponent<ModuleTankPartComponent>().TankPart;

        public long Level =>
            (base.UserItem != null) ? base.UserItem.GetComponent<ModuleUpgradeLevelComponent>().Level : 0L;

        public int MaxLevel =>
            (base.UserItem != null) ? base.UserItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices.Count : this.MarketItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices.Count;

        public ModulePrice CraftPrice =>
            this.MarketItem.GetComponent<ModuleCardsCompositionComponent>().CraftPrice;

        public int UpgradePrice =>
            (base.UserItem != null) ? ((this.Level >= this.MaxLevel) ? 0 : base.UserItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[(int) this.Level].Cards) : 0;

        public int UpgradePriceCRY =>
            (base.UserItem != null) ? ((this.Level >= this.MaxLevel) ? 0 : base.UserItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[(int) this.Level].Crystals) : 0;

        public int UpgradePriceXCRY =>
            (base.UserItem != null) ? ((this.Level >= this.MaxLevel) ? 0 : base.UserItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[(int) this.Level].XCrystals) : 0;

        public Entity CardItem { get; set; }

        public long UserCardCount =>
            (this.CardItem != null) ? this.CardItem.GetComponent<UserItemCounterComponent>().Count : 0L;

        public string CardSpriteUid =>
            (this.MarketCardItem != null) ? this.MarketCardItem.GetComponent<ImageItemComponent>().SpriteUid : string.Empty;

        public Entity Property { get; set; }

        public Entity MarketCardItem { get; set; }

        public List<ModuleVisualProperty> properties =>
            this.Property?.GetComponent<ModuleVisualPropertiesComponent>().Properties;

        public Entity Slot { get; set; }
    }
}

