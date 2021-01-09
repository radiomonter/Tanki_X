namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class VegetationSettingsComponent : Component
    {
        public int Value { get; set; }

        public bool FarFoliageEnabled { get; set; }

        public bool BillboardTreesShadowCasting { get; set; }

        public bool BillboardTreesShadowReceiving { get; set; }
    }
}

