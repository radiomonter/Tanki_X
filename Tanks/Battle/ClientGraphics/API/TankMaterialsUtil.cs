namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public static class TankMaterialsUtil
    {
        private static string MAIN_MATERIAL_SUFFIX = "main";
        private static string NITRO_DETAILS_MATERIAL_SUFFIX = "nitro";
        private static string DD_DETAILS_MATERIAL_SUFFIX = "dd";
        private static string TRACKS_MATERIAL_MARK = "tracks";

        public static void ApplyColoring(Renderer renderer, ColoringComponent coloring)
        {
            Material mainMaterial = GetMainMaterial(renderer);
            if (mainMaterial)
            {
                AnimatedPaintComponent component = coloring.GetComponent<AnimatedPaintComponent>();
                if (component != null)
                {
                    component.AddMaterial(mainMaterial);
                }
                mainMaterial.EnableKeyword(TankMaterialPropertyNames.COLORING_KEYWORD);
                mainMaterial.SetColor(TankMaterialPropertyNames.COLORING_ID, (Color) coloring.color);
                if (coloring.coloringTexture != null)
                {
                    mainMaterial.SetTexture(TankMaterialPropertyNames.COLORING_MAP, coloring.coloringTexture);
                }
                else
                {
                    mainMaterial.SetTexture(TankMaterialPropertyNames.COLORING_MAP, null);
                }
                mainMaterial.SetFloat(TankMaterialPropertyNames.METALLIC_COLORING_ID, coloring.metallic);
                mainMaterial.SetFloat(TankMaterialPropertyNames.COLORING_MAP_ALPHA_DEF_ID, (float) coloring.coloringTextureAlphaMode);
                if (coloring.coloringNormalMap != null)
                {
                    mainMaterial.SetTexture(TankMaterialPropertyNames.COLORING_BUMP, coloring.coloringNormalMap);
                    mainMaterial.SetFloat(TankMaterialPropertyNames.COLORING_BUMP_SCALE_ID, coloring.coloringNormalScale);
                    mainMaterial.SetFloat(TankMaterialPropertyNames.COLORING_BUMP_MAP_DEF_ID, 1f);
                }
                else
                {
                    mainMaterial.SetTexture(TankMaterialPropertyNames.COLORING_BUMP, null);
                    mainMaterial.SetFloat(TankMaterialPropertyNames.COLORING_BUMP_SCALE_ID, 1f);
                    mainMaterial.SetFloat(TankMaterialPropertyNames.COLORING_BUMP_MAP_DEF_ID, 0f);
                }
                if (coloring.overwriteSmoothness)
                {
                    mainMaterial.SetFloat(TankMaterialPropertyNames.COLORING_SMOOTHNESS_ID, coloring.smoothnessStrength);
                }
                else
                {
                    mainMaterial.SetFloat(TankMaterialPropertyNames.COLORING_SMOOTHNESS_ID, -1f);
                }
                if (coloring.useColoringIntensityThreshold)
                {
                    mainMaterial.SetFloat(TankMaterialPropertyNames.COLORING_MASK_THRESHOLD_ID, coloring.coloringMaskThreshold);
                }
                else
                {
                    mainMaterial.SetFloat(TankMaterialPropertyNames.COLORING_MASK_THRESHOLD_ID, -1f);
                }
                if (coloring.overrideEmission)
                {
                    mainMaterial.SetFloat(TankMaterialPropertyNames.EMISSION_INTENSITY_ID, coloring.emissionIntensity);
                    mainMaterial.SetColor(TankMaterialPropertyNames.EMISSION_COLOR_ID, coloring.emissionColor);
                }
            }
        }

        public static void ApplyColoring(Renderer tankRenderer, Renderer weaponRenderer, ColoringComponent tankColoring, ColoringComponent weaponColoring)
        {
            ApplyColoring(tankRenderer, tankColoring);
            if (weaponColoring.gameObject.GetComponent<IgnoreColoringBehaviour>())
            {
                ApplyColoring(weaponRenderer, tankColoring);
            }
            else
            {
                ApplyColoring(weaponRenderer, weaponColoring);
            }
        }

        public static float GetAlpha(Renderer renderer) => 
            renderer.material.GetFloat(TankMaterialPropertyNames.ALPHA);

        public static Material GetMainMaterial(Renderer renderer)
        {
            Material[] materials = renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material material = materials[i];
                if (IsMainMaterial(material))
                {
                    return material;
                }
            }
            return renderer.material;
        }

        public static Material GetMaterialForDoubleDamageDetails(Renderer renderer)
        {
            Material[] materials = renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material material = materials[i];
                if (MaterialNameContainsString(material, DD_DETAILS_MATERIAL_SUFFIX))
                {
                    return material;
                }
            }
            return null;
        }

        public static Material GetMaterialForNitroDetails(Renderer renderer)
        {
            Material[] materials = renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material material = materials[i];
                if (MaterialNameContainsString(material, NITRO_DETAILS_MATERIAL_SUFFIX))
                {
                    return material;
                }
            }
            return null;
        }

        public static Material GetTrackMaterial(Renderer renderer)
        {
            Material[] materials = renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material material = materials[i];
                if (IsTracksMaterial(material))
                {
                    return material;
                }
            }
            return null;
        }

        public static bool IsMainMaterial(Material material) => 
            MaterialNameContainsString(material, MAIN_MATERIAL_SUFFIX);

        public static bool IsTracksMaterial(Material material) => 
            MaterialNameContainsString(material, TRACKS_MATERIAL_MARK);

        private static bool MaterialNameContainsString(Material material, string suffix) => 
            material.name.ToLower().Contains(suffix);

        public static void ReplaceMainMaterial(Renderer renderer, Material newMaterial)
        {
            Material[] materials = renderer.materials;
            int length = materials.Length;
            int index = 0;
            while (true)
            {
                if (index < length)
                {
                    Material material = materials[index];
                    if (!IsMainMaterial(material))
                    {
                        index++;
                        continue;
                    }
                    materials[index] = newMaterial;
                }
                return;
            }
        }

        public static void SetAlpha(Renderer renderer, float alpha)
        {
            SetAlpha(renderer.materials, alpha);
        }

        public static void SetAlpha(Material[] materials, float alpha)
        {
            float clampedAlpha = Mathf.Clamp01(alpha);
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material material = materials[i];
                SetAlpha(material, clampedAlpha);
            }
        }

        public static void SetAlpha(Material material, float clampedAlpha)
        {
            material.SetFloat(TankMaterialPropertyNames.ALPHA, clampedAlpha);
        }

        public static void SetTemperature(Renderer renderer, float temperature)
        {
            Material[] sharedMaterials = renderer.sharedMaterials;
            int length = sharedMaterials.Length;
            for (int i = 0; i < length; i++)
            {
                Material material = sharedMaterials[i];
                if ((IsMainMaterial(material) || IsTracksMaterial(material)) && material.HasProperty(TankMaterialPropertyNames.TEMPERATURE_ID))
                {
                    material.SetFloat(TankMaterialPropertyNames.TEMPERATURE_ID, temperature);
                }
            }
        }
    }
}

