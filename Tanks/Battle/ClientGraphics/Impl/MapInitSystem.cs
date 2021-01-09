﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class MapInitSystem : ECSSystem
    {
        [OnEventFire]
        public void SetGrassQuality(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, SingleNode<CameraComponent> cameraNode)
        {
            GraphicsSettings iNSTANCE = GraphicsSettings.INSTANCE;
            if (iNSTANCE.CurrentGrassFarDrawDistance > 0.1f)
            {
                ShadowCastingMode mode = !iNSTANCE.CurrentGrassCastsShadow ? ShadowCastingMode.Off : ShadowCastingMode.On;
                foreach (GrassGenerator generator in map.component.SceneRoot.GetComponentsInChildren<GrassGenerator>())
                {
                    generator.SetCulling(iNSTANCE.CurrentGrassFarDrawDistance, iNSTANCE.CurrentGrassNearDrawDistance, iNSTANCE.CurrentGrassFadeRange, iNSTANCE.CurrentGrassDensityMultiplier);
                    generator.Generate();
                    this.SetShadowCastingMode(generator.transform, mode);
                }
            }
            map.component.SceneRoot.AddComponent<ShadowCasterCreatorBehaviour>();
        }

        [OnEventFire]
        public void SetMaterialsQuality(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<MaterialsSettingsComponent> materialsSettings)
        {
            Shader.globalMaximumLOD = materialsSettings.component.GlobalShadersMaximumLOD;
        }

        [OnEventFire]
        public void SetPostProcessing(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, SingleNode<CameraComponent> cameraNode, SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            GraphicsSettings.INSTANCE.customSettings = settings.component.CustomSettings;
            GraphicsSettings.INSTANCE.currentAmbientOcclusion = settings.component.AmbientOcclusion;
            GraphicsSettings.INSTANCE.currentBloom = settings.component.Bloom;
            GraphicsSettings.INSTANCE.currentChromaticAberration = settings.component.ChromaticAberration;
            GraphicsSettings.INSTANCE.currentGrain = settings.component.Grain;
            GraphicsSettings.INSTANCE.currentVignette = settings.component.Vignette;
        }

        private void SetShadowCastingMode(Transform root, ShadowCastingMode mode)
        {
            foreach (MeshRenderer renderer in root.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.shadowCastingMode = mode;
            }
        }

        [OnEventFire]
        public void SetWaterQuality(NodeAddedEvent e, SingleNode<WaterComponent> water, [JoinAll] SingleNode<WaterSettingsComponent> waterSettings)
        {
            if (!waterSettings.component.HasReflection)
            {
                water.component.DisableReflection();
            }
            water.component.EdgeBlend = waterSettings.component.EdgeBlend;
        }

        public class CameraNode : Node
        {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
            public CameraTransformDataComponent cameraTransformData;
            public CameraOffsetConfigComponent cameraOffsetConfig;
            public BezierPositionComponent bezierPosition;
            public CameraESMComponent cameraEsm;
        }
    }
}

