namespace Platform.Library.ClientLocale.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LocaleComponent : Component
    {
        public string Code { get; set; }

        public string Caption { get; set; }

        public string LocalizedCaption { get; set; }
    }
}

