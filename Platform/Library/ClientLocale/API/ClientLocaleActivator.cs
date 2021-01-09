namespace Platform.Library.ClientLocale.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientLocaleActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        public void RegisterSystemsAndTemplates()
        {
            TemplateRegistry.Register<LocaleTemplate>();
            TemplateRegistry.Register<LocaleListTemplate>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

