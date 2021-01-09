namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    internal class CustomTankBuilderSystem : ECSSystem
    {
        private void ApplyPaint(GameObject tankInstance, GameObject weaponInstance, GameObject tankPaintInstance, GameObject weaponPaintInstance)
        {
            TankMaterialsUtil.ApplyColoring(TankBuilderUtil.GetHullRenderer(tankInstance), TankBuilderUtil.GetWeaponRenderer(weaponInstance), tankPaintInstance.GetComponent<ColoringComponent>(), weaponPaintInstance.GetComponent<ColoringComponent>());
        }

        [OnEventFire]
        public void BuildTank(BuildBattleResultTankEvent e, Node node, [JoinAll] SingleNode<CustomTankBuilder> tankBuilder)
        {
            RenderTexture newRenderTexture = new RenderTexture(0x400, 0x400, 0x18, RenderTextureFormat.ARGB32);
            e.tankPreviewRenderTexture = newRenderTexture;
            tankBuilder.component.BuildTank(e.HullGuid, e.WeaponGuid, e.PaintGuid, e.CoverGuid, e.BestPlayerScreen, newRenderTexture);
        }

        [OnEventFire]
        public void BuildTankBattleResults(NodeAddedEvent e, BuildBattleResultsHullNode hull, BuildBattleResultsWeaponNode weapon, BuildBattleResultsPaintNode paint, BuildBattleResultsCoverNode cover)
        {
            hull.Entity.RemoveComponent<BattleResultsHullPositionComponent>();
            weapon.Entity.RemoveComponent<BattleResultsWeaponPositionComponent>();
            paint.Entity.RemoveComponent<BattleResultsPaintPositionComponent>();
            cover.Entity.RemoveComponent<BattleResultsCoverPositionComponent>();
            GameObject tankInstance = (GameObject) Object.Instantiate(hull.resourceData.Data);
            tankInstance.transform.SetParent(hull.battleResultsHullPosition.transform, false);
            tankInstance.transform.localPosition = Vector3.zero;
            tankInstance.transform.localRotation = Quaternion.identity;
            Transform mountPoint = tankInstance.GetComponent<MountPointComponent>().MountPoint;
            GameObject weaponInstance = (GameObject) Object.Instantiate(weapon.resourceData.Data);
            weaponInstance.transform.SetParent(weapon.battleResultsWeaponPosition.transform, false);
            weaponInstance.transform.localPosition = mountPoint.localPosition;
            weaponInstance.transform.localRotation = mountPoint.localRotation;
            GameObject tankPaintInstance = (GameObject) Object.Instantiate(paint.resourceData.Data);
            tankPaintInstance.transform.SetParent(tankInstance.transform, false);
            tankPaintInstance.transform.localPosition = Vector3.zero;
            GameObject weaponPaintInstance = (GameObject) Object.Instantiate(cover.resourceData.Data);
            weaponPaintInstance.transform.SetParent(tankInstance.transform, false);
            weaponPaintInstance.transform.localPosition = Vector3.zero;
            this.ApplyPaint(tankInstance, weaponInstance, tankPaintInstance, weaponPaintInstance);
            this.ChangeLayer(hull.battleResultsHullPosition.gameObject);
            this.ChangeLayer(weapon.battleResultsWeaponPosition.gameObject);
        }

        private void ChangeLayer(GameObject go)
        {
            CustomTankBuilderLayerSetterComponent component = go.GetComponent<CustomTankBuilderLayerSetterComponent>();
            if (component != null)
            {
                this.SetLayerRecursively(go, component.Layer);
            }
        }

        [OnEventFire]
        public void ClearTank(ClearBattleResultTankEvent e, Node node, [JoinAll] SingleNode<CustomTankBuilder> tankBuilder)
        {
            tankBuilder.component.ClearContainer();
        }

        [OnEventFire]
        public void PrepareTankBattleResults(NodeAddedEvent e, BattleResultsTankPositionNode tank, BattleResultsHullPositionNode hullPosition, BattleResultsWeaponPositionNode weaponPosition, BattleResultsPaintPositionNode paintPosition, BattleResultsCoverPositionNode coverPosition)
        {
            hullPosition.battleResultsHullPosition.transform.DestroyChildren();
            Entity entity = hullPosition.Entity;
            string hullGuid = tank.battleResultsTankPosition.hullGuid;
            hullPosition.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(hullGuid)));
            hullPosition.Entity.AddComponent<AssetRequestComponent>();
            weaponPosition.battleResultsWeaponPosition.transform.DestroyChildren();
            Entity entity2 = weaponPosition.Entity;
            string weaponGuid = tank.battleResultsTankPosition.weaponGuid;
            weaponPosition.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(weaponGuid)));
            weaponPosition.Entity.AddComponent<AssetRequestComponent>();
            paintPosition.battleResultsPaintPosition.transform.DestroyChildren();
            Entity entity3 = paintPosition.Entity;
            string paintGuid = tank.battleResultsTankPosition.paintGuid;
            paintPosition.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(paintGuid)));
            paintPosition.Entity.AddComponent<AssetRequestComponent>();
            coverPosition.battleResultsCoverPosition.transform.DestroyChildren();
            Entity entity4 = coverPosition.Entity;
            string coverGuid = tank.battleResultsTankPosition.coverGuid;
            coverPosition.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(coverGuid)));
            coverPosition.Entity.AddComponent<AssetRequestComponent>();
        }

        private void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (null != obj)
            {
                obj.layer = newLayer;
                IEnumerator enumerator = obj.transform.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Transform current = (Transform) enumerator.Current;
                        if (null != current)
                        {
                            this.SetLayerRecursively(current.gameObject, newLayer);
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public class BattleResultsCoverPositionNode : Node
        {
            public BattleResultsCoverPositionComponent battleResultsCoverPosition;
        }

        public class BattleResultsHullPositionNode : Node
        {
            public BattleResultsHullPositionComponent battleResultsHullPosition;
        }

        public class BattleResultsPaintPositionNode : Node
        {
            public BattleResultsPaintPositionComponent battleResultsPaintPosition;
        }

        public class BattleResultsTankPositionNode : Node
        {
            public BattleResultsTankPositionComponent battleResultsTankPosition;
        }

        public class BattleResultsWeaponPositionNode : Node
        {
            public BattleResultsWeaponPositionComponent battleResultsWeaponPosition;
        }

        public class BuildBattleResultsCoverNode : CustomTankBuilderSystem.BattleResultsCoverPositionNode
        {
            public AssetReferenceComponent assetReference;
            public ResourceDataComponent resourceData;
        }

        public class BuildBattleResultsHullNode : CustomTankBuilderSystem.BattleResultsHullPositionNode
        {
            public AssetReferenceComponent assetReference;
            public ResourceDataComponent resourceData;
        }

        public class BuildBattleResultsPaintNode : CustomTankBuilderSystem.BattleResultsPaintPositionNode
        {
            public AssetReferenceComponent assetReference;
            public ResourceDataComponent resourceData;
        }

        public class BuildBattleResultsWeaponNode : CustomTankBuilderSystem.BattleResultsWeaponPositionNode
        {
            public AssetReferenceComponent assetReference;
            public ResourceDataComponent resourceData;
        }
    }
}

