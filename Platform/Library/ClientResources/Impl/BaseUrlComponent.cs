namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BaseUrlComponent : Component
    {
        public BaseUrlComponent()
        {
        }

        public BaseUrlComponent(string url)
        {
            this.Url = url;
        }

        public string Url { get; set; }
    }
}

