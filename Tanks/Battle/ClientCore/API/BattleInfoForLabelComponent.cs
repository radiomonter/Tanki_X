namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15590a3f82cL)]
    public class BattleInfoForLabelComponent : Component
    {
        public string BattleMode { get; set; }
    }
}

