namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class LazySkyboxLoadingSystem : ECSSystem
    {
        private bool GoodSystem() => 
            (SystemInfo.graphicsMemorySize > 0x200) || (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D9);

        private bool HDRCompressedTexturesSupported() => 
            SystemInfo.SupportsTextureFormat(TextureFormat.BC6H) && SystemInfo.SupportsTextureFormat(TextureFormat.BC7);

        [OnEventFire]
        public void RequestSkyboxResource(NodeAddedEvent e, SingleNode<MapInstanceComponent> mapInstance, SingleNode<LazySkyboxComponet> lazySkybox)
        {
            if (this.GoodSystem() && ((GraphicsSettings.INSTANCE.CurrentQualityLevel > 1) && this.HDRCompressedTexturesSupported()))
            {
                if (mapInstance.Entity.HasComponent<SkyBoxDataComponent>())
                {
                    RenderSettings.skybox = (Material) mapInstance.Entity.GetComponent<SkyBoxDataComponent>().Data;
                }
                else
                {
                    base.ScheduleEvent(new AssetRequestEvent().Init<SkyBoxDataComponent>(lazySkybox.component.SkyBoxReference.AssetGuid), mapInstance);
                }
            }
        }

        [OnEventFire]
        public void SetSkybox(NodeAddedEvent e, SingleNode<SkyBoxDataComponent> skyBox)
        {
            RenderSettings.skybox = (Material) skyBox.component.Data;
        }

        public class SkyBoxDataComponent : ResourceDataComponent
        {
        }
    }
}

