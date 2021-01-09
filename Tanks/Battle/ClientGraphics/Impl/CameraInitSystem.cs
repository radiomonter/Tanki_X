namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class CameraInitSystem : ECSSystem
    {
        private void ActivatePostEffects(CameraComponent camera)
        {
            string str = QualitySettings.names[QualitySettings.GetQualityLevel()].ToLower();
            PostEffectsSet[] postEffectsSets = camera.PostEffectsSets;
            foreach (PostEffectsSet set in postEffectsSets)
            {
                if (set.qualityName != str)
                {
                    set.SetActive(false);
                }
            }
            foreach (PostEffectsSet set2 in postEffectsSets)
            {
                if (set2.qualityName == str)
                {
                    set2.SetActive(true);
                    camera.DepthTextureMode = set2.depthTextureMode;
                }
            }
        }

        [OnEventFire]
        public void SetPostEffectsQuality(NodeAddedEvent e, SingleNode<CameraComponent> cameraNode)
        {
            CameraComponent camera = cameraNode.component;
            camera.DepthTextureMode = DepthTextureMode.Depth;
            this.ActivatePostEffects(camera);
        }
    }
}

