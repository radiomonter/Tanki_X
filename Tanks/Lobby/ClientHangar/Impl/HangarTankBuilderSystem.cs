namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class HangarTankBuilderSystem : HangarTankBaseSystem
    {
        private void ApplyPaint(GameObject tankInstance, GameObject weaponInstance, GameObject tankPaintInstance, GameObject weaponPaintInstance)
        {
            TankMaterialsUtil.ApplyColoring(TankBuilderUtil.GetHullRenderer(tankInstance), TankBuilderUtil.GetWeaponRenderer(weaponInstance), tankPaintInstance.GetComponent<ColoringComponent>(), weaponPaintInstance.GetComponent<ColoringComponent>());
        }

        [OnEventFire]
        public void BuildTank(NodeAddedEvent e, HangarTankBaseSystem.HangarNode hangar, HangarTankBaseSystem.WeaponSkinItemPreviewLoadedNode weaponSkin, [Context, JoinByParentGroup] HangarTankBaseSystem.WeaponItemPreviewNode weaponItem, HangarTankBaseSystem.HullSkinItemPreviewLoadedNode tankSkin, [Context, JoinByParentGroup] HangarTankBaseSystem.TankItemPreviewNode tankItem, TankPaintItemPreviewLoadedNode tankPaint, WeaponPaintItemPreviewLoadedNode weaponPaint, HangarTankBaseSystem.HangarCameraNode hangarCamera, SingleNode<SupplyEffectSettingsComponent> settings)
        {
            Transform root = hangar.hangarTankPosition.transform;
            root.DestroyChildren();
            GameObject tankInstance = (GameObject) Object.Instantiate(tankSkin.resourceData.Data);
            tankInstance.transform.SetParent(root);
            tankInstance.transform.localPosition = Vector3.zero;
            tankInstance.transform.localRotation = Quaternion.identity;
            tankSkin.hangarItemPreview.Instance = tankInstance;
            tankInstance.GetComponentInChildren<NitroEffectComponent>().InitEffect(settings.component);
            Transform mountPoint = tankInstance.GetComponent<MountPointComponent>().MountPoint;
            GameObject weaponInstance = (GameObject) Object.Instantiate(weaponSkin.resourceData.Data);
            weaponInstance.transform.SetParent(tankInstance.transform);
            weaponInstance.transform.localPosition = mountPoint.localPosition;
            weaponInstance.transform.localRotation = mountPoint.localRotation;
            weaponSkin.hangarItemPreview.Instance = weaponInstance;
            GameObject tankPaintInstance = (GameObject) Object.Instantiate(tankPaint.resourceData.Data);
            tankPaintInstance.transform.SetParent(tankInstance.transform);
            tankPaintInstance.transform.localPosition = Vector3.zero;
            GameObject weaponPaintInstance = (GameObject) Object.Instantiate(weaponPaint.resourceData.Data);
            weaponPaintInstance.transform.SetParent(tankInstance.transform);
            weaponPaintInstance.transform.localPosition = Vector3.zero;
            PhysicsUtil.SetGameObjectLayer(root.gameObject, Layers.STATIC);
            this.ApplyPaint(tankInstance, weaponInstance, tankPaintInstance, weaponPaintInstance);
            weaponInstance.GetComponentInChildren<DoubleDamageEffectComponent>().InitEffect(settings.component);
            weaponInstance.GetComponentInChildren<DoubleDamageSoundEffectComponent>().RecalculatePlayingParameters();
            Renderer weaponRenderer = TankBuilderUtil.GetWeaponRenderer(weaponInstance);
            weaponRenderer.tag = "TankWeapon";
            Renderer hullRenderer = TankBuilderUtil.GetHullRenderer(tankInstance);
            hullRenderer.tag = "TankHull";
            weaponRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            weaponRenderer.lightProbeUsage = LightProbeUsage.Off;
            hullRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            hullRenderer.lightProbeUsage = LightProbeUsage.Off;
            BurningTargetBloom componentInChildren = hangarCamera.cameraRootTransform.Root.GetComponentInChildren<BurningTargetBloom>();
            componentInChildren.targets.Clear();
            componentInChildren.targets.Add(weaponRenderer);
            componentInChildren.targets.Add(hullRenderer);
            base.ScheduleEvent<HangarTankBuildedEvent>(hangar);
        }

        public class TankPaintItemPreviewLoadedNode : HangarTankBuilderSystem.TankPaintItemPreviewNode
        {
            public ResourceDataComponent resourceData;
        }

        public class TankPaintItemPreviewNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public TankPaintItemComponent tankPaintItem;
        }

        public class WeaponPaintItemPreviewLoadedNode : HangarTankBuilderSystem.WeaponPaintItemPreviewNode
        {
            public ResourceDataComponent resourceData;
        }

        public class WeaponPaintItemPreviewNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public WeaponPaintItemComponent weaponPaintItem;
        }
    }
}

