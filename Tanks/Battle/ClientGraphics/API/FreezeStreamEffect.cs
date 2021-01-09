namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class FreezeStreamEffect : StreamEffectBehaviour
    {
        public ParticleSystem muzzleParticleSystem;
        public ParticleSystem mistParticleSystem;
        public ParticleSystem snowParticleSystem;

        public override void AddCollisionLayer(int layer)
        {
            ParticleSystem.CollisionModule collision = this.mistParticleSystem.collision;
            collision.collidesWith = LayerMasksUtils.AddLayerToMask((int) collision.collidesWith, layer);
            collision = this.snowParticleSystem.collision;
            collision.collidesWith = LayerMasksUtils.AddLayerToMask((int) collision.collidesWith, layer);
        }

        public override void ApplySettings(StreamWeaponSettingsComponent streamWeaponSettings)
        {
            base.ApplySettings(streamWeaponSettings);
            this.muzzleParticleSystem.maxParticles = streamWeaponSettings.FreezeMuzzleMaxParticles;
            this.mistParticleSystem.maxParticles = streamWeaponSettings.FreezeMistMaxParticles;
            this.snowParticleSystem.maxParticles = streamWeaponSettings.FreezeSnowMaxParticles;
        }
    }
}

