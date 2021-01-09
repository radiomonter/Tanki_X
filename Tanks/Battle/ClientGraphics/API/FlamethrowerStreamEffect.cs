namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class FlamethrowerStreamEffect : StreamEffectBehaviour
    {
        public ParticleSystem muzzleParticleSystem;
        public ParticleSystem flameParticleSystem;
        public ParticleSystem smokeParticleSystem;

        public override void AddCollisionLayer(int layer)
        {
            ParticleSystem.CollisionModule collision = this.flameParticleSystem.collision;
            collision.collidesWith = LayerMasksUtils.AddLayerToMask((int) collision.collidesWith, layer);
            collision = this.smokeParticleSystem.collision;
            collision.collidesWith = LayerMasksUtils.AddLayerToMask((int) collision.collidesWith, layer);
        }

        public override void ApplySettings(StreamWeaponSettingsComponent streamWeaponSettings)
        {
            base.ApplySettings(streamWeaponSettings);
            this.muzzleParticleSystem.maxParticles = streamWeaponSettings.FlamethrowerMuzzleMaxParticles;
            this.flameParticleSystem.maxParticles = streamWeaponSettings.FlamethrowerFlameMaxParticles;
            this.smokeParticleSystem.maxParticles = streamWeaponSettings.FlamethrowerSmokeMaxParticles;
        }
    }
}

