namespace Tanks.Lobby.ClientSettings.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GraphicsSettings
    {
        private const string WIPE_GRAPHICS_SETTINGS_KEY = "WIPE_GRAPHICS_SETTINGS";
        private const string SATURATION_SHADER_PARAMETER = "_GraphicsSettingsSaturationLevel";
        private const string QUALITY_LEVEL_KEY = "QUALITY_LEVEL_INDEX";
        private const string VEGETATION_LEVEL_KEY = "VEGETATION_LEVEL_INDEX";
        private const string FAR_FOLIAGE_ENABLE_KEY = "FAR_FOLIAGE_ENABLE";
        private const string BILLBOARD_TREES_SHADOWCASTING_KEY = "BILLBOARD_TREES_SHADOWCASTING";
        private const string TREES_SHADOW_RECIEVING = "TREES_SHADOW_RECIEVING";
        private const string GRASS_LEVEL_KEY = "GRASS_LEVEL_INDEX";
        private const string GRASS_FAR_DRAW_DISTANCE_KEY = "GRASS_FAR_DRAW_DISTANCE";
        private const string GRASS_NEAR_DRAW_DISTANCE_KEY = "GRASS_NEAR_DRAW_DISTANCE";
        private const string GRASS_FADE_RANGE_KEY = "GRASS_FADE_RANGE";
        private const string GRASS_DENSITY_MULTIPLIER_KEY = "GRASS_DENSITY_MULTIPLIER";
        private const string GRASS_CASTS_SHADOW_KEY = "GRASS_CASTS_SHADOW";
        private const string SCREEN_RESOLUTION_WIDTH_KEY = "SCREEN_RESOLUTION_WIDTH";
        private const string SCREEN_RESOLUTION_HEIGHT_KEY = "SCREEN_RESOLUTION_HEIGHT";
        private const string SATURATION_LEVEL_KEY = "SATURATION_LEVEL";
        private const string RENDER_RESOLUTION_QUALITY_KEY = "RENDER_RESOLUTION_QUALITY";
        private const string ANTIALIASING_LEVEL_KEY = "ANTIALIASING_LEVEL";
        private const string CARTRIDGE_CASE_AMOUNT_KEY = "CARTRIDGE_CASE_AMOUNT";
        private const string VSYNC_KEY = "VSYNC";
        private const string WINDOW_MODE_KEY = "WINDOW_MODE";
        private const string NO_COMPACT_WINDOW_KEY = "NO_COMPACT_WINDOW";
        private const string ANISOTROPIC_LEVEL_KEY = "ANISOTROPIC_LEVEL";
        private const string SHADOW_LEVEL_KEY = "SHADOW_LEVEL";
        private const string TEXTURE_LEVEL_KEY = "TEXTURE_LEVEL";
        private const string PARTICLE_LEVEL_KEY = "PARTICLE_LEVEL";
        private const string ULTRA_LOW_SETTINGS_FLAG = "ULTRA_LOW_SETTINGS_FLAG";
        private int currentQualityLevel;
        private int currentVegetationLevel;
        private bool currentFarFoliageEnabled;
        private bool currentBillboardTreesShadowCasting;
        private bool currentTreesShadowRecieving;
        private int currentGrassLevel;
        private float currentGrassFarDrawDistance;
        private float currentGrassNearDrawDistance;
        private float currentGrassFadeRange;
        private float currentGrassDensityMultiplier;
        private bool currentGrassCastsShadow;
        private float currentSaturationLevel;
        private int currentRenderResolutionQuality;
        private int currentAntialiasingQuality;
        private int currentAnisotropicQuality;
        private int currentCartridgeCaseAmount;
        private int currentVSyncQuality;
        private int currentShadowQuality;
        private int currentTextureQuality;
        private int currentParticleQuality;
        private Resolution currentResolution;
        private CompactScreenBehaviour compactScreen;
        public bool customSettings;
        public bool currentAmbientOcclusion;
        public bool currentBloom;
        public bool currentChromaticAberration;
        public bool currentGrain;
        public bool currentVignette;
        private int CurrentSettingsVersion = 0x14d;

        public void ApplyAnisotropicQuality(int currentValue)
        {
            this.CurrentAnisotropicQuality = currentValue;
        }

        public void ApplyAnisotropicQuality(int currentValue, int defaultValue)
        {
            this.DefaultAnisotropicQuality = defaultValue;
            this.ApplyAnisotropicQuality(currentValue);
        }

        public void ApplyAntialiasingQuality(int currentValue)
        {
            this.CurrentAntialiasingQuality = currentValue;
        }

        public void ApplyAntialiasingQuality(int currentValue, int defaultValue)
        {
            this.DefaultAntialiasingQuality = defaultValue;
            this.ApplyAntialiasingQuality(currentValue);
        }

        public void ApplyBillboardTreesShadowCasting(bool currentValue)
        {
            this.CurrentBillboardTreesShadowCasting = currentValue;
        }

        public void ApplyBillboardTreesShadowCasting(bool currentValue, bool defaultValue)
        {
            this.DefaultBillboardTreesShadowCasting = defaultValue;
            this.ApplyBillboardTreesShadowCasting(currentValue);
        }

        public void ApplyCartridgeCaseAmount(int currentValue)
        {
            this.CurrentCartridgeCaseAmount = currentValue;
        }

        public void ApplyCartridgeCaseAmount(int currentValue, int defaultValue)
        {
            this.DefaultCartridgeCaseAmount = defaultValue;
            this.ApplyCartridgeCaseAmount(currentValue);
        }

        public void ApplyFarFoliageEnabled(bool currentValue)
        {
            this.CurrentFarFoliageEnabled = true;
        }

        public void ApplyFarFoliageEnabled(bool currentValue, bool defaultValue)
        {
            this.DefaultFarFoliageEnabled = defaultValue;
            this.ApplyFarFoliageEnabled(currentValue);
        }

        public void ApplyGrassCastsShadow(bool currentValue)
        {
            this.CurrentGrassCastsShadow = currentValue;
        }

        public void ApplyGrassCastsShadow(bool currentValue, bool defaultValue)
        {
            this.DefaultGrassCastsShadow = defaultValue;
            this.ApplyGrassCastsShadow(currentValue);
        }

        public void ApplyGrassDensityMultiplier(float currentValue)
        {
            this.CurrentGrassDensityMultiplier = currentValue;
        }

        public void ApplyGrassDensityMultiplier(float currentValue, float defaultValue)
        {
            this.DefaultGrassDensityMultiplier = defaultValue;
            this.ApplyGrassDensityMultiplier(currentValue);
        }

        public void ApplyGrassFadeRange(float currentValue)
        {
            this.CurrentGrassFadeRange = currentValue;
        }

        public void ApplyGrassFadeRange(float currentValue, float defaultValue)
        {
            this.DefaultGrassFadeRange = defaultValue;
            this.ApplyGrassFadeRange(currentValue);
        }

        public void ApplyGrassFarDrawDistance(float currentValue)
        {
            this.CurrentGrassFarDrawDistance = currentValue;
        }

        public void ApplyGrassFarDrawDistance(float currentValue, float defaultValue)
        {
            this.DefaultGrassFarDrawDistance = defaultValue;
            this.ApplyGrassFarDrawDistance(currentValue);
        }

        public void ApplyGrassLevel(int currentValue)
        {
            this.CurrentGrassLevel = currentValue;
        }

        public void ApplyGrassLevel(int currentValue, int defaultValue)
        {
            this.DefaultGrassLevel = defaultValue;
            this.ApplyGrassLevel(currentValue);
        }

        public void ApplyGrassNearDrawDistance(float currentValue)
        {
            this.CurrentGrassNearDrawDistance = currentValue;
        }

        public void ApplyGrassNearDrawDistance(float currentValue, float defaultValue)
        {
            this.DefaultGrassNearDrawDistance = defaultValue;
            this.ApplyGrassNearDrawDistance(currentValue);
        }

        public void ApplyInitialScreenResolutionData()
        {
            Screen.SetResolution(this.currentResolution.width, this.currentResolution.height, !this.InitialWindowed);
        }

        public void ApplyParticleQuality(int currentValue)
        {
            this.CurrentParticleQuality = currentValue;
        }

        public void ApplyParticleQuality(int currentValue, int defaultValue)
        {
            this.DefaultParticleQuality = defaultValue;
            this.ApplyParticleQuality(currentValue);
        }

        public void ApplyQualityLevel(int qualityLevel)
        {
            this.CurrentQualityLevel = qualityLevel;
        }

        public void ApplyRenderResolutionQuality(int currentValue)
        {
            this.CurrentRenderResolutionQuality = currentValue;
        }

        public void ApplyRenderResolutionQuality(int currentValue, int defaultValue)
        {
            this.DefaultRenderResolutionQuality = defaultValue;
            this.ApplyRenderResolutionQuality(currentValue);
        }

        public void ApplySaturationLevel(float currentValue)
        {
            this.CurrentSaturationLevel = currentValue;
            Shader.SetGlobalFloat("_GraphicsSettingsSaturationLevel", this.currentSaturationLevel);
        }

        public void ApplySaturationLevel(float currentValue, float defaultValue)
        {
            this.DefaultSaturationLevel = defaultValue;
            this.ApplySaturationLevel(currentValue);
        }

        public void ApplyScreenResolution(int width, int height, bool windowed)
        {
            PlayerPrefs.SetInt("SCREEN_RESOLUTION_WIDTH", width);
            PlayerPrefs.SetInt("SCREEN_RESOLUTION_HEIGHT", height);
            Screen.SetResolution(width, height, !windowed);
        }

        public void ApplyShadowQuality(int currentValue)
        {
            this.CurrentShadowQuality = currentValue;
        }

        public void ApplyShadowQuality(int currentValue, int defaultValue)
        {
            this.DefaultShadowQuality = defaultValue;
            this.ApplyShadowQuality(currentValue);
        }

        public void ApplyTextureQuality(int currentValue)
        {
            this.CurrentTextureQuality = currentValue;
        }

        public void ApplyTextureQuality(int currentValue, int defaultValue)
        {
            this.DefaultTextureQuality = defaultValue;
            this.ApplyTextureQuality(currentValue);
        }

        public void ApplyTreesShadowRecieving(bool currentValue)
        {
            this.CurrentTreesShadowRecieving = currentValue;
        }

        public void ApplyTreesShadowRecieving(bool currentValue, bool defaultValue)
        {
            this.DefaultTreesShadowRecieving = defaultValue;
            this.ApplyTreesShadowRecieving(currentValue);
        }

        public void ApplyVegetationLevel(int currentValue)
        {
            this.CurrentVegetationLevel = currentValue;
        }

        public void ApplyVegetationLevel(int currentValue, int defaultValue)
        {
            this.DefaultVegetationLevel = defaultValue;
            this.ApplyVegetationLevel(currentValue);
        }

        public void ApplyVSyncQuality(int currentValue)
        {
            this.CurrentVSyncQuality = currentValue;
        }

        public void ApplyVSyncQuality(int currentValue, int defaultValue)
        {
            this.DefaultVSyncQuality = defaultValue;
            this.ApplyVSyncQuality(currentValue);
        }

        public void ApplyWindowMode(bool windowed)
        {
            Screen.fullScreen = !windowed;
        }

        private bool DefineScreenMode()
        {
            if (!PlayerPrefs.HasKey("WINDOW_MODE"))
            {
                return Screen.fullScreen;
            }
            bool flag = !Convert.ToBoolean(PlayerPrefs.GetInt("WINDOW_MODE"));
            PlayerPrefs.DeleteKey("WINDOW_MODE");
            return flag;
        }

        public void DisableCompactScreen()
        {
            if (this.compactScreen != null)
            {
                this.compactScreen.DisableCompactMode();
                PlayerPrefs.SetInt("NO_COMPACT_WINDOW", 1);
                if (PlayerPrefs.HasKey("WINDOW_MODE"))
                {
                    PlayerPrefs.DeleteKey("WINDOW_MODE");
                }
            }
        }

        public void EnableCompactScreen(CompactScreenBehaviour compactScreen)
        {
            this.compactScreen = compactScreen;
            compactScreen.InitCompactMode();
        }

        public void InitAnisotropicQualitySettings(int defaultAnisotropicQuality)
        {
            this.DefaultAnisotropicQuality = defaultAnisotropicQuality;
            if (PlayerPrefs.HasKey("ANISOTROPIC_LEVEL"))
            {
                this.currentAnisotropicQuality = PlayerPrefs.GetInt("ANISOTROPIC_LEVEL");
            }
            else
            {
                this.CurrentAnisotropicQuality = this.DefaultAnisotropicQuality;
            }
        }

        public void InitAntialiasingQualitySettings(int defaultAntialiasingQuality)
        {
            this.DefaultAntialiasingQuality = defaultAntialiasingQuality;
            if (PlayerPrefs.HasKey("ANTIALIASING_LEVEL"))
            {
                this.currentAntialiasingQuality = PlayerPrefs.GetInt("ANTIALIASING_LEVEL");
            }
            else
            {
                this.CurrentAntialiasingQuality = this.DefaultAntialiasingQuality;
            }
        }

        public void InitBillboardTreesShadowCasting(bool defaultBillboardTreesShadowCasting)
        {
            this.DefaultBillboardTreesShadowCasting = defaultBillboardTreesShadowCasting;
            if (!PlayerPrefs.HasKey("BILLBOARD_TREES_SHADOWCASTING"))
            {
                this.CurrentBillboardTreesShadowCasting = this.DefaultBillboardTreesShadowCasting;
            }
            else
            {
                if (PlayerPrefs.GetInt("BILLBOARD_TREES_SHADOWCASTING") == 0)
                {
                    this.currentBillboardTreesShadowCasting = false;
                }
                if (PlayerPrefs.GetInt("BILLBOARD_TREES_SHADOWCASTING") == 1)
                {
                    this.currentBillboardTreesShadowCasting = true;
                }
            }
        }

        public void InitCartridgeCaseAmount(int defaultAmount)
        {
            this.currentCartridgeCaseAmount = defaultAmount;
            if (PlayerPrefs.HasKey("CARTRIDGE_CASE_AMOUNT"))
            {
                this.currentCartridgeCaseAmount = PlayerPrefs.GetInt("CARTRIDGE_CASE_AMOUNT");
            }
            else
            {
                this.CurrentCartridgeCaseAmount = this.DefaultCartridgeCaseAmount;
            }
        }

        public void InitFarFoliageEnabled(bool defaultFarFoliageEnabled)
        {
            this.DefaultFarFoliageEnabled = defaultFarFoliageEnabled;
            if (!PlayerPrefs.HasKey("FAR_FOLIAGE_ENABLE"))
            {
                this.CurrentFarFoliageEnabled = this.DefaultFarFoliageEnabled;
            }
            else
            {
                if (PlayerPrefs.GetInt("FAR_FOLIAGE_ENABLE") == 0)
                {
                    this.currentFarFoliageEnabled = false;
                }
                if (PlayerPrefs.GetInt("FAR_FOLIAGE_ENABLE") == 1)
                {
                    this.currentFarFoliageEnabled = true;
                }
            }
        }

        public void InitGrassCastsShadow(bool defaultGrassCastsShadow)
        {
            this.DefaultGrassCastsShadow = defaultGrassCastsShadow;
            if (!PlayerPrefs.HasKey("GRASS_CASTS_SHADOW"))
            {
                this.CurrentGrassCastsShadow = this.DefaultGrassCastsShadow;
            }
            else
            {
                if (PlayerPrefs.GetInt("GRASS_CASTS_SHADOW") == 0)
                {
                    this.currentGrassCastsShadow = false;
                }
                if (PlayerPrefs.GetInt("GRASS_CASTS_SHADOW") == 1)
                {
                    this.currentGrassCastsShadow = true;
                }
            }
        }

        public void InitGrassDensityMultiplier(float defaultGrassDensityMultiplier)
        {
            this.DefaultGrassDensityMultiplier = defaultGrassDensityMultiplier;
            if (PlayerPrefs.HasKey("GRASS_DENSITY_MULTIPLIER"))
            {
                this.currentGrassDensityMultiplier = PlayerPrefs.GetFloat("GRASS_DENSITY_MULTIPLIER");
            }
            else
            {
                this.CurrentGrassDensityMultiplier = this.DefaultGrassDensityMultiplier;
            }
        }

        public void InitGrassFadeRange(float defaultGrassFadeRange)
        {
            this.DefaultGrassFadeRange = defaultGrassFadeRange;
            if (PlayerPrefs.HasKey("GRASS_FADE_RANGE"))
            {
                this.currentGrassFadeRange = PlayerPrefs.GetFloat("GRASS_FADE_RANGE");
            }
            else
            {
                this.CurrentGrassFadeRange = this.DefaultGrassFadeRange;
            }
        }

        public void InitGrassFarDrawDistance(float defaultGrassFarDrawDistance)
        {
            this.DefaultGrassFarDrawDistance = defaultGrassFarDrawDistance;
            if (PlayerPrefs.HasKey("GRASS_FAR_DRAW_DISTANCE"))
            {
                this.currentGrassFarDrawDistance = PlayerPrefs.GetFloat("GRASS_FAR_DRAW_DISTANCE");
            }
            else
            {
                this.CurrentGrassFarDrawDistance = this.DefaultGrassFarDrawDistance;
            }
        }

        public void InitGrassLevelSettings(int defaultGrassLevel)
        {
            this.DefaultGrassLevel = defaultGrassLevel;
            if (PlayerPrefs.HasKey("GRASS_LEVEL_INDEX"))
            {
                this.currentGrassLevel = PlayerPrefs.GetInt("GRASS_LEVEL_INDEX");
            }
            else
            {
                this.CurrentGrassLevel = this.DefaultGrassLevel;
            }
        }

        public void InitGrassNearDrawDistance(float defaultGrassNearDrawDistance)
        {
            this.DefaultGrassNearDrawDistance = defaultGrassNearDrawDistance;
            if (PlayerPrefs.HasKey("GRASS_NEAR_DRAW_DISTANCE"))
            {
                this.currentGrassNearDrawDistance = PlayerPrefs.GetFloat("GRASS_NEAR_DRAW_DISTANCE");
            }
            else
            {
                this.CurrentGrassNearDrawDistance = this.DefaultGrassNearDrawDistance;
            }
        }

        public void InitParticleQualitySettings(int defaultParticleQuality)
        {
            this.DefaultParticleQuality = defaultParticleQuality;
            if (PlayerPrefs.HasKey("PARTICLE_LEVEL"))
            {
                this.currentParticleQuality = PlayerPrefs.GetInt("PARTICLE_LEVEL");
            }
            else
            {
                this.CurrentParticleQuality = this.DefaultParticleQuality;
            }
        }

        public void InitQualitySettings(Quality defaultQuality, bool ultraEnabled)
        {
            this.DefaultQuality = defaultQuality;
            this.UltraEnabled = ultraEnabled;
            this.WipeSettings(this.CurrentSettingsVersion);
            if (PlayerPrefs.HasKey("QUALITY_LEVEL_INDEX"))
            {
                this.currentQualityLevel = !PlayerPrefs.HasKey("ULTRA_LOW_SETTINGS_FLAG") ? (this.currentQualityLevel + 1) : PlayerPrefs.GetInt("QUALITY_LEVEL_INDEX");
            }
            else
            {
                this.CurrentQualityLevel = this.DefaultQualityLevel;
            }
            QualitySettings.SetQualityLevel(this.CurrentQualityLevel, true);
        }

        public void InitRenderResolutionQualitySettings(int defaultRenderResolutionQuality)
        {
            this.DefaultRenderResolutionQuality = defaultRenderResolutionQuality;
            if (PlayerPrefs.HasKey("RENDER_RESOLUTION_QUALITY"))
            {
                this.currentRenderResolutionQuality = PlayerPrefs.GetInt("RENDER_RESOLUTION_QUALITY");
            }
            else
            {
                this.CurrentRenderResolutionQuality = this.DefaultRenderResolutionQuality;
            }
        }

        public void InitSaturationLevelSettings(float defaultSaturationLevel)
        {
            this.DefaultSaturationLevel = defaultSaturationLevel;
            if (PlayerPrefs.HasKey("SATURATION_LEVEL"))
            {
                this.currentSaturationLevel = PlayerPrefs.GetFloat("SATURATION_LEVEL");
            }
            else
            {
                this.CurrentSaturationLevel = this.DefaultSaturationLevel;
            }
        }

        public void InitScreenResolutionSettings(List<Resolution> avaiableResolutions, Resolution defaultResolution)
        {
            this.ScreenResolutions = avaiableResolutions;
            this.DefaultResolution = defaultResolution;
            bool flag = false;
            if (!PlayerPrefs.HasKey("SCREEN_RESOLUTION_WIDTH") || !PlayerPrefs.HasKey("SCREEN_RESOLUTION_HEIGHT"))
            {
                this.CurrentResolution = this.DefaultResolution;
            }
            else
            {
                flag = true;
                int @int = PlayerPrefs.GetInt("SCREEN_RESOLUTION_WIDTH");
                int num2 = PlayerPrefs.GetInt("SCREEN_RESOLUTION_HEIGHT");
                Resolution resolution = new Resolution();
                this.currentResolution = resolution;
                this.currentResolution.width = @int;
                this.currentResolution.height = num2;
                this.CurrentResolution = (from r in this.ScreenResolutions
                    orderby Mathf.Abs((int) (r.width - this.currentResolution.width)) + Mathf.Abs((int) (r.height - this.currentResolution.height))
                    select r).First<Resolution>();
            }
            bool flag2 = true;
            if (!this.NeedCompactWindow())
            {
                flag2 = !flag ? !this.WindowedByDefault : this.DefineScreenMode();
            }
            this.InitialWindowed = !flag2;
        }

        public void InitShadowQualitySettings(int defaultShadowQuality)
        {
            this.DefaultShadowQuality = defaultShadowQuality;
            if (PlayerPrefs.HasKey("SHADOW_LEVEL"))
            {
                this.currentShadowQuality = PlayerPrefs.GetInt("SHADOW_LEVEL");
            }
            else
            {
                this.CurrentShadowQuality = this.DefaultShadowQuality;
            }
        }

        public void InitTextureQualitySettings(int defaultTextureQuality)
        {
            this.DefaultTextureQuality = defaultTextureQuality;
            if (PlayerPrefs.HasKey("TEXTURE_LEVEL"))
            {
                this.currentTextureQuality = PlayerPrefs.GetInt("TEXTURE_LEVEL");
            }
            else
            {
                this.CurrentTextureQuality = this.DefaultTextureQuality;
            }
        }

        public void InitTreesShadowRecieving(bool defaultTreesShadowRecieving)
        {
            this.DefaultTreesShadowRecieving = defaultTreesShadowRecieving;
            if (!PlayerPrefs.HasKey("TREES_SHADOW_RECIEVING"))
            {
                this.CurrentTreesShadowRecieving = this.DefaultTreesShadowRecieving;
            }
            else
            {
                if (PlayerPrefs.GetInt("TREES_SHADOW_RECIEVING") == 0)
                {
                    this.currentBillboardTreesShadowCasting = false;
                }
                if (PlayerPrefs.GetInt("TREES_SHADOW_RECIEVING") == 1)
                {
                    this.currentBillboardTreesShadowCasting = true;
                }
            }
        }

        public void InitVegetationLevelSettings(int defaultVegetationLevel)
        {
            this.DefaultVegetationLevel = defaultVegetationLevel;
            if (PlayerPrefs.HasKey("VEGETATION_LEVEL_INDEX"))
            {
                this.currentVegetationLevel = PlayerPrefs.GetInt("VEGETATION_LEVEL_INDEX");
            }
            else
            {
                this.CurrentVegetationLevel = this.DefaultVegetationLevel;
            }
        }

        public void InitVSyncQualitySettings(int defaultVSyncQuality)
        {
            this.DefaultVSyncQuality = defaultVSyncQuality;
            if (PlayerPrefs.HasKey("VSYNC"))
            {
                this.currentVSyncQuality = PlayerPrefs.GetInt("VSYNC");
            }
            else
            {
                this.CurrentVSyncQuality = this.DefaultVSyncQuality;
            }
        }

        public void InitWindowModeSettings(bool isWindowedByDefault)
        {
            this.WindowedByDefault = isWindowedByDefault;
        }

        public bool NeedCompactWindow() => 
            !PlayerPrefs.HasKey("NO_COMPACT_WINDOW");

        public void SaveWindowModeOnQuit()
        {
            PlayerPrefs.SetInt("WINDOW_MODE", Convert.ToInt32(this.InitialWindowed));
        }

        public void WipeSettings(int version)
        {
            if (PlayerPrefs.GetInt("WIPE_GRAPHICS_SETTINGS") != version)
            {
                PlayerPrefs.DeleteKey("QUALITY_LEVEL_INDEX");
                PlayerPrefs.DeleteKey("TEXTURE_LEVEL");
                PlayerPrefs.DeleteKey("PARTICLE_LEVEL");
                PlayerPrefs.DeleteKey("SHADOW_LEVEL");
                PlayerPrefs.DeleteKey("ANISOTROPIC_LEVEL");
                PlayerPrefs.DeleteKey("ANTIALIASING_LEVEL");
                PlayerPrefs.DeleteKey("VEGETATION_LEVEL_INDEX");
                PlayerPrefs.DeleteKey("FAR_FOLIAGE_ENABLE");
                PlayerPrefs.DeleteKey("BILLBOARD_TREES_SHADOWCASTING");
                PlayerPrefs.DeleteKey("TREES_SHADOW_RECIEVING");
                PlayerPrefs.DeleteKey("GRASS_LEVEL_INDEX");
                PlayerPrefs.DeleteKey("GRASS_FAR_DRAW_DISTANCE");
                PlayerPrefs.DeleteKey("GRASS_NEAR_DRAW_DISTANCE");
                PlayerPrefs.DeleteKey("GRASS_FADE_RANGE");
                PlayerPrefs.DeleteKey("GRASS_DENSITY_MULTIPLIER");
                PlayerPrefs.DeleteKey("GRASS_CASTS_SHADOW");
                PlayerPrefs.DeleteKey("CUSTOM_SETTINGS_MODE");
                PlayerPrefs.DeleteKey("AMBIENT_OCCLUSION_MODE");
                PlayerPrefs.DeleteKey("BLOOM_MODE");
                PlayerPrefs.DeleteKey("CHROMATIC_ABERRATION_MODE");
                PlayerPrefs.DeleteKey("GRAIN_MODE");
                PlayerPrefs.DeleteKey("VIGNETTE_MODE");
                PlayerPrefs.DeleteKey("LOW_RENDER_RESOLUTION_MODE");
                PlayerPrefs.SetInt("WIPE_GRAPHICS_SETTINGS", version);
            }
        }

        public static GraphicsSettings INSTANCE { get; set; }

        public bool WindowedByDefault { get; private set; }

        public bool InitialWindowed { get; private set; }

        public int DefaultQualityLevel =>
            this.DefaultQuality.Level;

        public Quality DefaultQuality { get; private set; }

        public bool UltraEnabled { get; private set; }

        public int CurrentQualityLevel
        {
            get => 
                this.currentQualityLevel;
            private set
            {
                this.currentQualityLevel = value;
                PlayerPrefs.SetInt("QUALITY_LEVEL_INDEX", this.currentQualityLevel);
                PlayerPrefs.SetInt("ULTRA_LOW_SETTINGS_FLAG", this.currentQualityLevel);
            }
        }

        public int DefaultVegetationLevel { get; private set; }

        public int CurrentVegetationLevel
        {
            get => 
                this.currentVegetationLevel;
            private set
            {
                this.currentVegetationLevel = value;
                PlayerPrefs.SetInt("VEGETATION_LEVEL_INDEX", this.currentVegetationLevel);
            }
        }

        public bool DefaultFarFoliageEnabled { get; private set; }

        public bool CurrentFarFoliageEnabled
        {
            get => 
                this.currentFarFoliageEnabled;
            private set
            {
                this.currentFarFoliageEnabled = value;
                PlayerPrefs.SetInt("FAR_FOLIAGE_ENABLE", !this.currentFarFoliageEnabled ? 1 : 0);
            }
        }

        public bool DefaultBillboardTreesShadowCasting { get; private set; }

        public bool CurrentBillboardTreesShadowCasting
        {
            get => 
                this.currentBillboardTreesShadowCasting;
            private set
            {
                this.currentBillboardTreesShadowCasting = value;
                PlayerPrefs.SetInt("BILLBOARD_TREES_SHADOWCASTING", !this.currentBillboardTreesShadowCasting ? 1 : 0);
            }
        }

        public bool DefaultTreesShadowRecieving { get; private set; }

        public bool CurrentTreesShadowRecieving
        {
            get => 
                this.currentTreesShadowRecieving;
            private set
            {
                this.currentTreesShadowRecieving = value;
                PlayerPrefs.SetInt("TREES_SHADOW_RECIEVING", !this.currentTreesShadowRecieving ? 1 : 0);
            }
        }

        public int DefaultGrassLevel { get; private set; }

        public int CurrentGrassLevel
        {
            get => 
                this.currentGrassLevel;
            private set
            {
                this.currentGrassLevel = value;
                PlayerPrefs.SetInt("GRASS_LEVEL_INDEX", this.currentGrassLevel);
            }
        }

        public float DefaultGrassFarDrawDistance { get; private set; }

        public float CurrentGrassFarDrawDistance
        {
            get => 
                this.currentGrassFarDrawDistance;
            private set
            {
                this.currentGrassFarDrawDistance = value;
                PlayerPrefs.SetFloat("GRASS_FAR_DRAW_DISTANCE", this.currentGrassFarDrawDistance);
            }
        }

        public float DefaultGrassNearDrawDistance { get; private set; }

        public float CurrentGrassNearDrawDistance
        {
            get => 
                this.currentGrassNearDrawDistance;
            private set
            {
                this.currentGrassNearDrawDistance = value;
                PlayerPrefs.SetFloat("GRASS_NEAR_DRAW_DISTANCE", this.currentGrassNearDrawDistance);
            }
        }

        public float DefaultGrassFadeRange { get; private set; }

        public float CurrentGrassFadeRange
        {
            get => 
                this.currentGrassFadeRange;
            private set
            {
                this.currentGrassFadeRange = value;
                PlayerPrefs.SetFloat("GRASS_FADE_RANGE", this.currentGrassFadeRange);
            }
        }

        public float DefaultGrassDensityMultiplier { get; private set; }

        public float CurrentGrassDensityMultiplier
        {
            get => 
                this.currentGrassDensityMultiplier;
            private set
            {
                this.currentGrassDensityMultiplier = value;
                PlayerPrefs.SetFloat("GRASS_DENSITY_MULTIPLIER", this.currentGrassDensityMultiplier);
            }
        }

        public bool DefaultGrassCastsShadow { get; private set; }

        public bool CurrentGrassCastsShadow
        {
            get => 
                this.currentGrassCastsShadow;
            private set
            {
                this.currentGrassCastsShadow = value;
                PlayerPrefs.SetInt("GRASS_CASTS_SHADOW", !this.currentGrassCastsShadow ? 1 : 0);
            }
        }

        public float DefaultSaturationLevel { get; private set; }

        public float CurrentSaturationLevel
        {
            get => 
                this.currentSaturationLevel;
            private set
            {
                this.currentSaturationLevel = value;
                PlayerPrefs.SetFloat("SATURATION_LEVEL", this.currentSaturationLevel);
            }
        }

        public int DefaultAnisotropicQuality { get; private set; }

        public int CurrentAnisotropicQuality
        {
            get => 
                this.currentAnisotropicQuality;
            private set
            {
                this.currentAnisotropicQuality = value;
                PlayerPrefs.SetInt("ANISOTROPIC_LEVEL", this.currentAnisotropicQuality);
            }
        }

        public int DefaultVSyncQuality { get; private set; }

        public int CurrentVSyncQuality
        {
            get => 
                this.currentVSyncQuality;
            private set
            {
                this.currentVSyncQuality = value;
                PlayerPrefs.SetInt("VSYNC", this.currentVSyncQuality);
            }
        }

        public int DefaultShadowQuality { get; private set; }

        public int CurrentShadowQuality
        {
            get => 
                this.currentShadowQuality;
            private set
            {
                this.currentShadowQuality = value;
                PlayerPrefs.SetInt("SHADOW_LEVEL", this.currentShadowQuality);
            }
        }

        public int DefaultTextureQuality { get; private set; }

        public int CurrentTextureQuality
        {
            get => 
                this.currentTextureQuality;
            private set
            {
                this.currentTextureQuality = value;
                PlayerPrefs.SetInt("TEXTURE_LEVEL", this.currentTextureQuality);
            }
        }

        public int DefaultParticleQuality { get; private set; }

        public int CurrentParticleQuality
        {
            get => 
                this.currentParticleQuality;
            private set
            {
                this.currentParticleQuality = value;
                PlayerPrefs.SetInt("PARTICLE_LEVEL", this.currentParticleQuality);
            }
        }

        public int DefaultRenderResolutionQuality { get; private set; }

        public int CurrentRenderResolutionQuality
        {
            get => 
                this.currentRenderResolutionQuality;
            private set
            {
                this.currentRenderResolutionQuality = value;
                PlayerPrefs.SetInt("RENDER_RESOLUTION_QUALITY", this.currentRenderResolutionQuality);
            }
        }

        public int DefaultAntialiasingQuality { get; private set; }

        public int CurrentAntialiasingQuality
        {
            get => 
                this.currentAntialiasingQuality;
            private set
            {
                this.currentAntialiasingQuality = value;
                PlayerPrefs.SetInt("ANTIALIASING_LEVEL", this.currentAntialiasingQuality);
            }
        }

        public int DefaultCartridgeCaseAmount { get; private set; }

        public int CurrentCartridgeCaseAmount
        {
            get => 
                this.currentCartridgeCaseAmount;
            private set
            {
                this.currentCartridgeCaseAmount = value;
                PlayerPrefs.SetInt("CARTRIDGE_CASE_AMOUNT", this.currentCartridgeCaseAmount);
            }
        }

        public List<Resolution> ScreenResolutions { get; private set; }

        public Resolution DefaultResolution { get; private set; }

        public Resolution CurrentResolution
        {
            get => 
                this.currentResolution;
            private set
            {
                this.currentResolution = value;
                PlayerPrefs.SetInt("SCREEN_RESOLUTION_WIDTH", this.currentResolution.width);
                PlayerPrefs.SetInt("SCREEN_RESOLUTION_HEIGHT", this.currentResolution.height);
            }
        }
    }
}

