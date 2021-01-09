namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CommunicatorActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        public void RegisterSystemsAndTemplates()
        {
            ECSBehaviour.EngineService.RegisterSystem(new ChatSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ChatScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SendMessageSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ReceiveMessageSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LobbyChatUISystem());
            ECSBehaviour.EngineService.RegisterSystem(new CreateChatSystem());
            TemplateRegistry.Register<ChatTemplate>();
            TemplateRegistry.Register<GeneralChatTemplate>();
            TemplateRegistry.Register<PersonalChatTemplate>();
            TemplateRegistry.Register<CustomChatTemplate>();
            TemplateRegistry.Register<SquadChatTemplate>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

