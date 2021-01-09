namespace Tanks.Lobby.ClientPayment.Impl
{
    using Lobby.ClientPayment.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x9207930ea1b068bL)]
    public interface SpecialOfferBaseTemplate : GoodsTemplate, Template
    {
        CountableItemsPackComponent countableItemsPack();
        CrystalsPackComponent crystalsPack();
        ItemsPackFromConfigComponent itemsPackFromConfig();
        [AutoAdded, PersistentConfig("order", false)]
        OrderItemComponent orderItem();
        [AutoAdded, PersistentConfig("", false)]
        ReceiptTextComponent receiptText();
        [AutoAdded]
        SpecialOfferComponent specialOffer();
        [AutoAdded, PersistentConfig("", false)]
        SpecialOfferContentComponent specialOfferContent();
        [AutoAdded, PersistentConfig("", false)]
        SpecialOfferContentLocalizationComponent specialOfferContentLocalization();
        SpecialOfferDurationComponent specialOfferDuration();
        SpecialOfferEndTimeComponent specialOfferEndTime();
        SpecialOfferGroupComponent specialOfferGroup();
        [AutoAdded, PersistentConfig("", false)]
        SpecialOfferScreenLocalizationComponent specialOfferScreenLocalization();
        XCrystalsPackComponent xCrystalsPack();
    }
}

