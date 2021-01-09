namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class UrlLoaderComponent : Component
    {
        public UrlLoaderComponent(Platform.Library.ClientUnityIntegration.API.Loader loader)
        {
            this.Loader = loader;
        }

        public UrlLoaderComponent(Platform.Library.ClientUnityIntegration.API.Loader loader, bool noErrorEvent)
        {
            this.Loader = loader;
            this.NoErrorEvent = noErrorEvent;
        }

        public Platform.Library.ClientUnityIntegration.API.Loader Loader { get; set; }

        public bool NoErrorEvent { get; set; }
    }
}

