namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.impl;
    using UnityEngine;

    public class HammerDecalHitSystem : ECSSystem
    {
        private void DrawHammerHitDecals(Vector3 shotDirection, PelletThrowingGraphicsNode weapon, SingleNode<DecalManagerComponent> decalManagerNode, DecalSettingsComponent settings)
        {
            if (settings.EnableDecals && (settings.MaxDecalsForHammer > 0))
            {
                DecalMeshBuilder decalMeshBuilder = decalManagerNode.component.DecalMeshBuilder;
                BulletHoleDecalManager bulletHoleDecalManager = decalManagerNode.component.BulletHoleDecalManager;
                MuzzlePointComponent muzzlePoint = weapon.muzzlePoint;
                HammerDecalProjectorComponent hammerDecalProjector = weapon.hammerDecalProjector;
                Vector3 barrelOriginWorld = new MuzzleVisualAccessor(muzzlePoint).GetBarrelOriginWorld();
                decalMeshBuilder.Clean();
                DecalProjection projection2 = new DecalProjection {
                    HalfSize = hammerDecalProjector.CombineHalfSize,
                    Distantion = hammerDecalProjector.Distance,
                    Ray = new Ray(barrelOriginWorld - shotDirection, shotDirection)
                };
                DecalProjection decalProjection = projection2;
                if (decalMeshBuilder.CompleteProjectionByRaycast(decalProjection) && decalMeshBuilder.CollectPolygons(decalProjection))
                {
                    Vector3 localDirection = muzzlePoint.Current.InverseTransformVector(shotDirection);
                    Vector3[] vectorArray = PelletDirectionsCalculator.GetRandomDirections(weapon.hammerPelletCone, muzzlePoint.Current.rotation, localDirection);
                    List<Mesh> list = new List<Mesh>(vectorArray.Length);
                    for (int i = 0; i < Math.Min(vectorArray.Length, settings.MaxDecalsForHammer); i++)
                    {
                        Vector3 direction = vectorArray[i];
                        projection2 = new DecalProjection {
                            AtlasHTilesCount = hammerDecalProjector.AtlasHTilesCount,
                            AtlasVTilesCount = hammerDecalProjector.AtlasVTilesCount,
                            SurfaceAtlasPositions = hammerDecalProjector.SurfaceAtlasPositions,
                            HalfSize = hammerDecalProjector.HalfSize,
                            Up = hammerDecalProjector.Up,
                            Distantion = hammerDecalProjector.Distance,
                            Ray = new Ray(barrelOriginWorld - shotDirection, direction)
                        };
                        DecalProjection projection3 = projection2;
                        if (decalMeshBuilder.CompleteProjectionByRaycast(projection3))
                        {
                            decalMeshBuilder.BuilldDecalFromCollectedPolygons(projection3);
                            Mesh mesh = null;
                            if (decalMeshBuilder.GetResultToMesh(ref mesh))
                            {
                                list.Add(mesh);
                            }
                        }
                    }
                    if (list.Count != 0)
                    {
                        CombineInstance[] combine = new CombineInstance[list.Count];
                        for (int j = 0; j < list.Count; j++)
                        {
                            combine[j].mesh = list[j];
                        }
                        Mesh decalMesh = new Mesh();
                        decalMesh.CombineMeshes(combine, true, false);
                        decalMesh.RecalculateBounds();
                        bulletHoleDecalManager.AddDecal(decalMesh, hammerDecalProjector.Material, hammerDecalProjector.Color, hammerDecalProjector.LifeTime);
                    }
                }
            }
        }

        [OnEventFire]
        public void DrawHammerHitDecalsRemote(RemoteHammerShotEvent evt, PelletThrowingGraphicsNode weapon, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode, [JoinAll] SingleNode<DecalSettingsComponent> settings)
        {
            weapon.hammerPelletCone.ShotSeed = evt.RandomSeed;
            this.DrawHammerHitDecals(evt.ShotDirection, weapon, decalManagerNode, settings.component);
        }

        [OnEventFire]
        public void DrawHammerHitDecalsSelf(SelfHammerShotEvent evt, PelletThrowingGraphicsNode weapon, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode, [JoinAll] SingleNode<DecalSettingsComponent> settings)
        {
            this.DrawHammerHitDecals(evt.ShotDirection, weapon, decalManagerNode, settings.component);
        }

        public class PelletThrowingGraphicsNode : Node
        {
            public HammerPelletConeComponent hammerPelletCone;
            public MuzzlePointComponent muzzlePoint;
            public WeaponUnblockedComponent weaponUnblocked;
            public HammerDecalProjectorComponent hammerDecalProjector;
        }
    }
}

