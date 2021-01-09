namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class StreamingDecalHitSystem : AbstractDecalHitSystem
    {
        [OnEventComplete]
        public void DrawDecals(UpdateEvent evt, StreaminWeaponNode weaponNode, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode)
        {
            if (decalManagerNode.component.EnableDecals)
            {
                StreamingDecalProjectorComponent streamingDecalProjector = weaponNode.streamingDecalProjector;
                DecalManagerComponent component = decalManagerNode.component;
                if ((weaponNode.streamHit.StaticHit != null) && ((streamingDecalProjector.LastDecalCreationTime + streamingDecalProjector.DecalCreationPeriod) <= Time.time))
                {
                    streamingDecalProjector.LastDecalCreationTime = Time.time;
                    Vector3 barrelOriginWorld = new MuzzleVisualAccessor(weaponNode.muzzlePoint).GetBarrelOriginWorld();
                    Vector3 normalized = (weaponNode.streamHit.StaticHit.Position - barrelOriginWorld).normalized;
                    DecalProjection decalProjection = new DecalProjection {
                        AtlasHTilesCount = streamingDecalProjector.AtlasHTilesCount,
                        AtlasVTilesCount = streamingDecalProjector.AtlasVTilesCount,
                        SurfaceAtlasPositions = streamingDecalProjector.SurfaceAtlasPositions,
                        HalfSize = streamingDecalProjector.HalfSize,
                        Up = streamingDecalProjector.Up,
                        Distantion = streamingDecalProjector.Distance,
                        Ray = new Ray(barrelOriginWorld - normalized, normalized)
                    };
                    Mesh mesh = null;
                    if (component.DecalMeshBuilder.Build(decalProjection, ref mesh))
                    {
                        component.BulletHoleDecalManager.AddDecal(mesh, streamingDecalProjector.Material, streamingDecalProjector.Color, streamingDecalProjector.LifeTime);
                    }
                }
            }
        }

        public class StreaminWeaponNode : Node
        {
            public StreamHitComponent streamHit;
            public MuzzlePointComponent muzzlePoint;
            public StreamingDecalProjectorComponent streamingDecalProjector;
        }
    }
}

