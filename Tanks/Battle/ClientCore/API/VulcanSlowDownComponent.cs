namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;

    [Shared, SerialVersionUID(-6843896944033144903L)]
    public class VulcanSlowDownComponent : TimeValidateComponent
    {
        public VulcanSlowDownComponent()
        {
        }

        public VulcanSlowDownComponent(bool isAfterShooting)
        {
            this.IsAfterShooting = isAfterShooting;
        }

        public bool IsAfterShooting { get; set; }
    }
}

