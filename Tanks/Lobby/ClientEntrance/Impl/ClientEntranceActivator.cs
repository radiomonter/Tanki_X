namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;

    public class ClientEntranceActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            Engine engine = ECSBehaviour.EngineService.Engine;
            engine.CreateEntity(typeof(AuthentificationTemplate), "/lobby/entrance/authentication");
            engine.CreateEntity(typeof(RanksExperiencesConfigTemplate), "/ranksconfig");
            engine.CreateEntity(typeof(RanksNamesTemplate), "/ui/element/ranksnames");
        }

        private void RegisterSystems()
        {
            ECSBehaviour.EngineService.RegisterSystem(new SelfUserSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UserMoneyIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DependentInteractivitySystem());
            ECSBehaviour.EngineService.RegisterSystem(new EntryPointSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EntranceSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EntranceScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EntranceInputValidationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RegistrationScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RegistrationInputValidationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InviteScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InviteInputValidationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SaveLoginSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EntranceStatisticsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UidInputValidationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CodeInputValidationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SelfUserToLoggerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SubscribeCheckboxSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SteamAuthenticationSystem());
        }

        public void RegisterSystemsAndTemplates()
        {
            this.RegisterTemplates();
            this.RegisterSystems();
        }

        private void RegisterTemplates()
        {
            TemplateRegistry.Register<LobbyTemplate>();
            TemplateRegistry.Register<AuthentificationTemplate>();
            TemplateRegistry.Register<RanksExperiencesConfigTemplate>();
            TemplateRegistry.Register<RanksNamesTemplate>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

