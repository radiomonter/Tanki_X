namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientPaymentGUI.Impl.Payguru;
    using Tanks.Lobby.ClientPaymentGUI.Impl.TankRent;

    public class ClientPaymentGUIActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        public void RegisterSystemsAndTemplates()
        {
            ECSBehaviour.EngineService.RegisterSystem(new PaymentSectionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new GoodsSelectionScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SelectCountryScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MethodSelectionScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PaymentProcessingScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BankCardPaymentScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MobilePaymentScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MobilePaymentCheckoutScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new QiwiWalletScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new XCrystalsSaleSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShopXCrystalsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DealsUISystem());
            ECSBehaviour.EngineService.RegisterSystem(new StarterPackSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EnergyBonusUISystem());
            ECSBehaviour.EngineService.RegisterSystem(new PackPurchaseSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankRentOfferSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NewLeagueRewardSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PayguruUISystem());
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

