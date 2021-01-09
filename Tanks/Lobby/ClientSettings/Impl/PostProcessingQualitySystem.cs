namespace Tanks.Lobby.ClientSettings.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class PostProcessingQualitySystem : ECSSystem
    {
        private static readonly string CUSTOM_SETTINGS_MODE_KEY = "CUSTOM_SETTINGS_MODE";
        private static readonly int CUSTOM_SETTINGS_DEFAULT_VALUE;
        private static readonly string AMBIENT_OCCLUSION_MODE_KEY = "AMBIENT_OCCLUSION_MODE";
        private static readonly int AMBIENT_OCCLUSION_DEFAULT_VALUE;
        private static readonly string BLOOM_MODE_KEY = "BLOOM_MODE";
        private static readonly int BLOOM_DEFAULT_VALUE;
        private static readonly string CHROMATIC_ABERRATION_MODE_KEY = "CHROMATIC_ABERRATION_MODE";
        private static readonly int CHROMATIC_ABERRATION_DEFAULT_VALUE;
        private static readonly string GRAIN_MODE_KEY = "GRAIN_MODE";
        private static readonly int GRAIN_DEFAULT_VALUE;
        private static readonly string VIGNETTE_MODE_KEY = "VIGNETTE_MODE";
        private static readonly int VIGNETTE_DEFAULT_VALUE;
        private static readonly string LOW_RENDER_RESOLUTION_MODE_KEY = "LOW_RENDER_RESOLUTION_MODE";
        private static readonly int LOW_RENDER_RESOLUTION_DEFAULT_VALUE;
        private static readonly string DISABLED_BATTLE_NOTIFICATIONS_KEY = "DISABLED_BATTLED_NOTIFICATIONS";
        private static readonly int DISABLED_BATTLE_NOTIFICATIONS_DEFAULT_VALUE;

        [OnEventFire]
        public void InitPostProcessingQualitySettings(NodeAddedEvent e, SingleNode<PostProcessingQualityVariantComponent> postProcessingQualitySettings)
        {
            postProcessingQualitySettings.component.CustomSettings = PlayerPrefs.GetInt(CUSTOM_SETTINGS_MODE_KEY, CUSTOM_SETTINGS_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.AmbientOcclusion = PlayerPrefs.GetInt(AMBIENT_OCCLUSION_MODE_KEY, AMBIENT_OCCLUSION_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.Bloom = PlayerPrefs.GetInt(BLOOM_MODE_KEY, BLOOM_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.ChromaticAberration = PlayerPrefs.GetInt(CHROMATIC_ABERRATION_MODE_KEY, CHROMATIC_ABERRATION_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.Grain = PlayerPrefs.GetInt(GRAIN_MODE_KEY, GRAIN_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.Vignette = PlayerPrefs.GetInt(VIGNETTE_MODE_KEY, VIGNETTE_DEFAULT_VALUE) > 0;
            postProcessingQualitySettings.component.DisableBattleNotifications = PlayerPrefs.GetInt(DISABLED_BATTLE_NOTIFICATIONS_KEY, DISABLED_BATTLE_NOTIFICATIONS_DEFAULT_VALUE) > 0;
        }

        [OnEventFire]
        public void PostProcessingQualitySettingsChanged(SettingsChangedEvent<PostProcessingQualityVariantComponent> e, Node any)
        {
            PlayerPrefs.SetInt(CUSTOM_SETTINGS_MODE_KEY, !e.Data.CustomSettings ? 0 : 1);
            PlayerPrefs.SetInt(AMBIENT_OCCLUSION_MODE_KEY, !e.Data.AmbientOcclusion ? 0 : 1);
            PlayerPrefs.SetInt(BLOOM_MODE_KEY, !e.Data.Bloom ? 0 : 1);
            PlayerPrefs.SetInt(CHROMATIC_ABERRATION_MODE_KEY, !e.Data.ChromaticAberration ? 0 : 1);
            PlayerPrefs.SetInt(GRAIN_MODE_KEY, !e.Data.Grain ? 0 : 1);
            PlayerPrefs.SetInt(VIGNETTE_MODE_KEY, !e.Data.Vignette ? 0 : 1);
            PlayerPrefs.SetInt(DISABLED_BATTLE_NOTIFICATIONS_KEY, !e.Data.DisableBattleNotifications ? 0 : 1);
        }
    }
}

