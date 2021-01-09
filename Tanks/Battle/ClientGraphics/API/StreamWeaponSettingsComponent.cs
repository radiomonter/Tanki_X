﻿namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class StreamWeaponSettingsComponent : Component
    {
        public bool LightIsEnabled { get; set; }

        public int FlamethrowerMuzzleMaxParticles { get; set; }

        public int FlamethrowerFlameMaxParticles { get; set; }

        public int FlamethrowerSmokeMaxParticles { get; set; }

        public int FreezeMuzzleMaxParticles { get; set; }

        public int FreezeMistMaxParticles { get; set; }

        public int FreezeSnowMaxParticles { get; set; }
    }
}

