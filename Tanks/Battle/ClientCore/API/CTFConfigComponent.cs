namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x152eeefebbdL)]
    public class CTFConfigComponent : Component
    {
        public float minDistanceFromMineToBase { get; set; }

        public float enemyFlagActionMinIntervalSec { get; set; }

        public float flagScaleOnGround { get; set; }

        public float flagScaleOnTank { get; set; }

        public float flagOriginPositionOnTank { get; set; }

        public float upsideDownMarkScale { get; set; }

        public float distanceForRotateFlag { get; set; }

        public float upsideDownMarkSpeed { get; set; }

        public float upsideDownMarkTimer { get; set; }

        public float upsideDownMarkOrigin { get; set; }
    }
}

