namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TanksPaymentActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            base.CreateEntity<PaymentSectionTemplate>("payment");
        }

        public void RegisterSystemsAndTemplates()
        {
            TemplateRegistry.Register<PaymentNotificationTemplate>();
            TemplateRegistry.Register<SaleEndNotificationTemplate>();
            TemplateRegistry.Register<XCrystalsPackTemplate>();
            TemplateRegistry.Register<BaseStarterPackSpecialOfferTemplate>();
            TemplateRegistry.Register<LegendaryTankSpecialOfferTemplate>();
            TemplateRegistry.Register<StarterPackSpecialOfferTemplate>();
            TemplateRegistry.Register<FullGarageSpecialOfferTemplate>();
            TemplateRegistry.Register<LeagueFirstEntranceSpecialOfferTemplate>();
            TemplateRegistry.Register<PaymentSectionTemplate>();
            TemplateRegistry.Register<PersonalSpecialOfferPropertiesTemplate>();
            TemplateRegistry.Register<PremiumOfferTemplate>();
            TemplateRegistry.Register<GoldBonusOfferTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new PaymentNotificationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new GoodsPriceSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SteamSystem());
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

