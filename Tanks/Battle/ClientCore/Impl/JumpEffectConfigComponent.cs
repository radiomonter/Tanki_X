namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16638a29a2bL)]
    public class JumpEffectConfigComponent : Component
    {
        public float ForceUpgradeMult { get; set; }
    }
}

