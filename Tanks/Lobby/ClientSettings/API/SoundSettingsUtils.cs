namespace Tanks.Lobby.ClientSettings.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SoundSettingsUtils
    {
        public static readonly string VOLUME_PARAM_KEY = "VolumeParam";
        public static readonly string LAZY_UI_VOLUME_PARAM_KEY = "LazyUIVolumeParam";
        public static readonly string MUSIC_VOLUME_PARAM_KEY = "MusicVolumeParam";
        public static readonly string SFX_VOLUME_PP_KEY = "SFX_VOLUME";
        public static readonly string MUSIC_VOLUME_PP_KEY = "MUSIC_VOLUME";
        public static readonly string UI_VOLUME_PP_KEY = "UI_VOLUME";
        public static readonly string SFX_MUTE_FLAG_PP_KEY = "SFX_MUTED";
        public static readonly string MUSIC_MUTE_FLAG_PP_KEY = "MUSIC_MUTED";
        public static readonly string UI_MUTE_FLAG_PP_KEY = "UI_MUTED";
        public static readonly float MUTED_VOLUME_VALUE = -80f;
        public static readonly float SFX_VOLUME_DEFAULT_VALUE = 0f;
        public static readonly float MUSIC_VOLUME_DEFAULT_VALUE = -3f;
        public static readonly float UI_VOLUME_DEFAULT_VALUE = 0f;
        private static readonly Dictionary<SoundType, SoundSettingsKeys> keys;

        static SoundSettingsUtils()
        {
            Dictionary<SoundType, SoundSettingsKeys> dictionary = new Dictionary<SoundType, SoundSettingsKeys> {
                { 
                    SoundType.SFX,
                    new SoundSettingsKeys(SFX_VOLUME_PP_KEY, SFX_VOLUME_DEFAULT_VALUE, SFX_MUTE_FLAG_PP_KEY)
                },
                { 
                    SoundType.Music,
                    new SoundSettingsKeys(MUSIC_VOLUME_PP_KEY, MUSIC_VOLUME_DEFAULT_VALUE, MUSIC_MUTE_FLAG_PP_KEY)
                },
                { 
                    SoundType.UI,
                    new SoundSettingsKeys(UI_VOLUME_PP_KEY, UI_VOLUME_DEFAULT_VALUE, UI_MUTE_FLAG_PP_KEY)
                }
            };
            keys = dictionary;
        }

        public static bool GetSavedMuteFlag(SoundType type) => 
            PlayerPrefs.GetInt(keys[type].muteFlagKey, 0) == 1;

        public static float GetSavedVolume(SoundType type) => 
            PlayerPrefs.GetFloat(keys[type].volumeKey, keys[type].defaultVolumeValue);

        public static void SaveMuteFlag(SoundType type, bool flag)
        {
            PlayerPrefs.SetInt(keys[type].muteFlagKey, !flag ? 0 : 1);
        }

        public static void SaveVolume(SoundType type, float volume)
        {
            PlayerPrefs.SetFloat(keys[type].volumeKey, volume);
        }

        public class SoundSettingsKeys
        {
            public string volumeKey;
            public float defaultVolumeValue;
            public string muteFlagKey;

            public SoundSettingsKeys(string volumeKey, float defaultVolumeValue, string muteFlagKey)
            {
                this.volumeKey = volumeKey;
                this.defaultVolumeValue = defaultVolumeValue;
                this.muteFlagKey = muteFlagKey;
            }
        }
    }
}

