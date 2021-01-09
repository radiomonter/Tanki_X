namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientProfile.API;
    using UnityEngine;

    public class CameraShakerComponent : Component
    {
        private const float MIN_FRONT_IMPACT_LENGTH = 0.005f;
        private CameraShaker cameraShaker;

        public CameraShakerComponent(CameraShaker cameraShaker)
        {
            this.cameraShaker = cameraShaker;
        }

        private ImpactAlignedInfluence CalculateInfluence(Vector3 impactFrontDirectionWorldSpace, CameraShakerConfigComponent cameraShakerConfig)
        {
            Vector3 up = this.cameraShaker.transform.up;
            Vector3 lhs = (Vector3.Cross(impactFrontDirectionWorldSpace, up).magnitude <= 0.01f) ? this.cameraShaker.transform.right : up;
            lhs = Vector3.Normalize(lhs - (impactFrontDirectionWorldSpace * Vector3.Dot(lhs, impactFrontDirectionWorldSpace)));
            Vector3 vector4 = Vector3.Cross(lhs, impactFrontDirectionWorldSpace);
            Matrix4x4 matrixx = new Matrix4x4();
            Matrix4x4 worldToLocalMatrix = this.cameraShaker.transform.worldToLocalMatrix;
            matrixx.SetColumn(0, new Vector4(vector4.x, vector4.y, vector4.z, 0f));
            matrixx.SetColumn(1, new Vector4(lhs.x, lhs.y, lhs.z, 0f));
            matrixx.SetColumn(2, new Vector4(impactFrontDirectionWorldSpace.x, impactFrontDirectionWorldSpace.y, impactFrontDirectionWorldSpace.z, 0f));
            matrixx.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
            Matrix4x4 matrixx3 = worldToLocalMatrix * matrixx;
            return new ImpactAlignedInfluence(matrixx3.MultiplyVector(new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ)), matrixx3.MultiplyVector(new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ)));
        }

        public void ShakeByDiscreteImpact(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig, Vector3 impactFrontDirectionWorldSpace, float weakeningCoeff)
        {
            if (this.ValidateShake(settings, cameraShakerConfig) && (impactFrontDirectionWorldSpace.magnitude >= 0.005f))
            {
                ImpactAlignedInfluence influence = this.CalculateInfluence(impactFrontDirectionWorldSpace, cameraShakerConfig);
                this.cameraShaker.ShakeOnce(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime, cameraShakerConfig.FadeOutTime, influence.posInfluence, influence.rotInfluence).ScaleMagnitude = weakeningCoeff;
            }
        }

        public void ShakeByFalling(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig, float weakeningCoeff)
        {
            if (this.ValidateShake(settings, cameraShakerConfig))
            {
                this.cameraShaker.ShakeOnce(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime, cameraShakerConfig.FadeOutTime, new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ), new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ)).ScaleMagnitude = weakeningCoeff;
            }
        }

        public void ShakeOnce(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig)
        {
            if (this.ValidateShake(settings, cameraShakerConfig))
            {
                this.cameraShaker.ShakeOnce(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime, cameraShakerConfig.FadeOutTime, new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ), new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ));
            }
        }

        public void ShakeOnce(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig, float cooldownTime)
        {
            if (this.ValidateShake(settings, cameraShakerConfig))
            {
                float fadeInTime = cameraShakerConfig.FadeInTime;
                float fadeOutTime = cameraShakerConfig.FadeOutTime;
                if ((fadeInTime + fadeOutTime) > cooldownTime)
                {
                    if (fadeInTime <= cooldownTime)
                    {
                        fadeOutTime = cooldownTime - fadeInTime;
                    }
                    else
                    {
                        fadeInTime = 0f;
                        fadeOutTime = cooldownTime;
                    }
                }
                this.cameraShaker.ShakeOnce(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, fadeInTime, fadeOutTime, new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ), new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ));
            }
        }

        public CameraShakeInstance StartShake(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig)
        {
            if (!this.ValidateShake(settings, cameraShakerConfig))
            {
                return null;
            }
            CameraShakeInstance instance = this.cameraShaker.StartShake(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime, new Vector3(cameraShakerConfig.PosInfluenceX, cameraShakerConfig.PosInfluenceY, cameraShakerConfig.PosInfluenceZ), new Vector3(cameraShakerConfig.RotInfluenceX, cameraShakerConfig.RotInfluenceY, cameraShakerConfig.RotInfluenceZ));
            instance.deleteOnInactive = false;
            return instance;
        }

        public CameraShakeInstance UpdateImpactShakeInstance(GameCameraShakerSettingsComponent settings, CameraShakeInstance instance, CameraShakerConfigComponent cameraShakerConfig, Vector3 frontDir, float weakening)
        {
            if (this.ValidateShake(settings, cameraShakerConfig))
            {
                if (frontDir.magnitude < 0.005f)
                {
                    return instance;
                }
                ImpactAlignedInfluence influence = this.CalculateInfluence(frontDir, cameraShakerConfig);
                if (instance == null)
                {
                    instance = this.cameraShaker.StartShake(cameraShakerConfig.Magnitude, cameraShakerConfig.Roughness, cameraShakerConfig.FadeInTime, influence.posInfluence, influence.rotInfluence);
                    instance.deleteOnInactive = false;
                }
                instance.ScaleMagnitude = weakening;
                instance.positionInfluence = influence.posInfluence;
                instance.rotationInfluence = influence.rotInfluence;
            }
            return instance;
        }

        private bool ValidateShake(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig) => 
            settings.Enabled && cameraShakerConfig.Enabled;

        [StructLayout(LayoutKind.Sequential)]
        private struct ImpactAlignedInfluence
        {
            public Vector3 posInfluence;
            public Vector3 rotInfluence;
            public ImpactAlignedInfluence(Vector3 posInfluence, Vector3 rotInfluence)
            {
                this.posInfluence = posInfluence;
                this.rotInfluence = rotInfluence;
            }
        }
    }
}

