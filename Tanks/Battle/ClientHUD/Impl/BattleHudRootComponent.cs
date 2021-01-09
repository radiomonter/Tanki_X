namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientProfile.API;
    using UnityEngine;

    public class BattleHudRootComponent : MonoBehaviour
    {
        public CameraShaker shaker;

        public void ShakeHUDOnDeath(GameCameraShakerSettingsComponent settings, TankCameraShakerConfigOnDeathComponent config)
        {
            if (this.ValidateShake(settings, config))
            {
                CameraShakeInstance instance = this.shaker.ShakeOnce(config.Magnitude, config.Roughness, config.FadeInTime, config.FadeOutTime, new Vector3(config.PosInfluenceX, config.PosInfluenceY, config.PosInfluenceZ), new Vector3(config.RotInfluenceX, config.RotInfluenceY, config.RotInfluenceZ));
            }
        }

        public void ShakeHUDOnFalling(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent config)
        {
            if (this.ValidateShake(settings, config))
            {
                CameraShakeInstance instance = this.shaker.ShakeOnce(config.Magnitude, config.Roughness, config.FadeInTime, config.FadeOutTime, new Vector3(config.PosInfluenceX, config.PosInfluenceY, config.PosInfluenceZ), new Vector3(config.RotInfluenceX, config.RotInfluenceY, config.RotInfluenceZ));
            }
        }

        public void ShakeHUDOnImpact(GameCameraShakerSettingsComponent settings, ImpactCameraShakerConfigComponent config)
        {
            if (this.ValidateShake(settings, config))
            {
                CameraShakeInstance instance = this.shaker.ShakeOnce(config.Magnitude, config.Roughness, config.FadeInTime, config.FadeOutTime, new Vector3(config.PosInfluenceX, config.PosInfluenceY, config.PosInfluenceZ), new Vector3(config.RotInfluenceX, config.RotInfluenceY, config.RotInfluenceZ));
            }
        }

        public void ShakeHUDOnShot(GameCameraShakerSettingsComponent settings, KickbackCameraShakerConfigComponent config)
        {
            if (this.ValidateShake(settings, config))
            {
                CameraShakeInstance instance = this.shaker.ShakeOnce(config.Magnitude, config.Roughness, config.FadeInTime, config.FadeOutTime, new Vector3(config.PosInfluenceX, config.PosInfluenceY, config.PosInfluenceZ), new Vector3(config.RotInfluenceX, config.RotInfluenceY, config.RotInfluenceZ));
            }
        }

        private bool ValidateShake(GameCameraShakerSettingsComponent settings, CameraShakerConfigComponent cameraShakerConfig) => 
            settings.Enabled && cameraShakerConfig.Enabled;
    }
}

