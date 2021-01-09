namespace Platform.Library.ClientLocale.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d2e6e0ee9793daL)]
    public class LocaleListComponent : Component
    {
        public List<string> Locales { get; set; }
    }
}

