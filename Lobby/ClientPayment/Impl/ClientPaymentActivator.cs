namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientPaymentActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            base.CreateEntity<CountriesTemplate>("payment/country");
        }

        private static void RegisterSystems()
        {
            ECSBehaviour.EngineService.RegisterSystem(new GoToUrlSystem());
        }

        public void RegisterSystemsAndTemplates()
        {
            this.RegisterTemplates();
            RegisterSystems();
        }

        private void RegisterTemplates()
        {
            TemplateRegistry.Register<GameCurrencyPackTemplate>();
            TemplateRegistry.Register<PaymentMethodTemplate>();
            TemplateRegistry.Register<SectionTemplate>();
            TemplateRegistry.Register<CountriesTemplate>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

