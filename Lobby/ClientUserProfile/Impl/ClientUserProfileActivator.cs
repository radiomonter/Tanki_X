namespace Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientUserProfileActivator : DefaultActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
        }

        public void RegisterSystemsAndTemplates()
        {
            TemplateRegistry.Register<UserTemplate>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

