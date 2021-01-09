namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d47ce5c85b742eL)]
    public class TankFallingCameraShakerConfigComponent : CameraShakerConfigComponent
    {
        public float MinFallingPower { get; set; }

        public float MaxFallingPower { get; set; }

        public float MinFallingPowerForHUD { get; set; }
    }
}

