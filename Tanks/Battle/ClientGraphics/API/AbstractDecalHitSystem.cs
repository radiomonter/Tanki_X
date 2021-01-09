namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public abstract class AbstractDecalHitSystem : ECSSystem
    {
        protected AbstractDecalHitSystem()
        {
        }

        protected void DrawHitDecal(HitEvent evt, DecalManagerComponent decalManager, DynamicDecalProjectorComponent decalProjector, MuzzlePointComponent muzzlePoint)
        {
            if (evt.StaticHit != null)
            {
                Vector3 barrelOriginWorld = new MuzzleVisualAccessor(muzzlePoint).GetBarrelOriginWorld();
                this.DrawHitDecal(decalManager, decalProjector, barrelOriginWorld, (evt.StaticHit.Position - barrelOriginWorld).normalized);
            }
        }

        protected void DrawHitDecal(DecalManagerComponent managerComponent, DynamicDecalProjectorComponent decalProjector, Vector3 position, Vector3 direction)
        {
            DecalProjection decalProjection = new DecalProjection {
                AtlasHTilesCount = decalProjector.AtlasHTilesCount,
                AtlasVTilesCount = decalProjector.AtlasVTilesCount,
                SurfaceAtlasPositions = decalProjector.SurfaceAtlasPositions,
                HalfSize = decalProjector.HalfSize,
                Up = decalProjector.Up,
                Distantion = decalProjector.Distance,
                Ray = new Ray(position, direction)
            };
            Mesh mesh = null;
            if (managerComponent.DecalMeshBuilder.Build(decalProjection, ref mesh))
            {
                managerComponent.BulletHoleDecalManager.AddDecal(mesh, decalProjector.Material, decalProjector.Color, decalProjector.LifeTime);
            }
        }
    }
}

