namespace Tanks.Lobby.ClientPayment.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d424c9b0bdc532L)]
    public interface PersonalSpecialOfferPropertiesTemplate : Template
    {
        [AutoAdded]
        OrderItemComponent orderItem();
        PaymentIntentComponent paymentIntent();
        [AutoAdded]
        PersonalSpecialOfferPropertiesComponent PersonalSpecialOfferProperties();
        SpecialOfferEndTimeComponent specialOfferEndTime();
        SpecialOfferGroupComponent SpecialOfferGroup();
        SpecialOfferRemainingTimeComponent SpecialOfferRemainingTime();
        SpecialOfferVisibleComponent SpecialOfferVisible();
        UserGroupComponent userGroup();
    }
}

