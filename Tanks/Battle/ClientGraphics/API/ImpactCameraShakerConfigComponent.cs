namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d47b723aa1e4c4L)]
    public class ImpactCameraShakerConfigComponent : CameraShakerConfigComponent
    {
        public float MinDistanceMagnitude { get; set; }

        public float MaxDistanceMagnitude { get; set; }
    }
}

