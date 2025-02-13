﻿namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class ScreenSpaceReflectionModel : PostProcessingModel
    {
        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        public override void Reset()
        {
            this.m_Settings = Settings.defaultSettings;
        }

        public Settings settings
        {
            get => 
                this.m_Settings;
            set => 
                this.m_Settings = value;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct IntensitySettings
        {
            [Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based."), Range(0f, 2f)]
            public float reflectionMultiplier;
            [Tooltip("How far away from the maxDistance to begin fading SSR."), Range(0f, 1000f)]
            public float fadeDistance;
            [Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor."), Range(0f, 1f)]
            public float fresnelFade;
            [Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle."), Range(0.1f, 10f)]
            public float fresnelFadePower;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct ReflectionSettings
        {
            [Tooltip("How the reflections are blended into the render.")]
            public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;
            [Tooltip("Half resolution SSRR is much faster, but less accurate.")]
            public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;
            [Tooltip("Maximum reflection distance in world units."), Range(0.1f, 300f)]
            public float maxDistance;
            [Tooltip("Max raytracing length."), Range(16f, 1024f)]
            public int iterationCount;
            [Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes."), Range(1f, 16f)]
            public int stepSize;
            [Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind."), Range(0.01f, 10f)]
            public float widthModifier;
            [Tooltip("Blurriness of reflections."), Range(0.1f, 8f)]
            public float reflectionBlur;
            [Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
            public bool reflectBackfaces;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct ScreenEdgeMask
        {
            [Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion."), Range(0f, 1f)]
            public float intensity;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Settings
        {
            public ScreenSpaceReflectionModel.ReflectionSettings reflection;
            public ScreenSpaceReflectionModel.IntensitySettings intensity;
            public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
            public static ScreenSpaceReflectionModel.Settings defaultSettings
            {
                get
                {
                    ScreenSpaceReflectionModel.Settings settings = new ScreenSpaceReflectionModel.Settings();
                    ScreenSpaceReflectionModel.ReflectionSettings settings2 = new ScreenSpaceReflectionModel.ReflectionSettings {
                        blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
                        reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
                        maxDistance = 100f,
                        iterationCount = 0x100,
                        stepSize = 3,
                        widthModifier = 0.5f,
                        reflectionBlur = 1f,
                        reflectBackfaces = false
                    };
                    settings.reflection = settings2;
                    ScreenSpaceReflectionModel.IntensitySettings settings3 = new ScreenSpaceReflectionModel.IntensitySettings {
                        reflectionMultiplier = 1f,
                        fadeDistance = 100f,
                        fresnelFade = 1f,
                        fresnelFadePower = 1f
                    };
                    settings.intensity = settings3;
                    ScreenSpaceReflectionModel.ScreenEdgeMask mask = new ScreenSpaceReflectionModel.ScreenEdgeMask {
                        intensity = 0.03f
                    };
                    settings.screenEdgeMask = mask;
                    return settings;
                }
            }
        }

        public enum SSRReflectionBlendType
        {
            PhysicallyBased,
            Additive
        }

        public enum SSRResolution
        {
            High = 0,
            Low = 2
        }
    }
}

