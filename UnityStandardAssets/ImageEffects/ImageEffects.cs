﻿namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [AddComponentMenu("")]
    public class ImageEffects
    {
        [Obsolete("Use Graphics.Blit(source,dest) instead")]
        public static void Blit(RenderTexture source, RenderTexture dest)
        {
            Graphics.Blit(source, dest);
        }

        [Obsolete("Use Graphics.Blit(source, destination, material) instead")]
        public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
        {
            Graphics.Blit(source, dest, material);
        }

        public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
        {
            if (source.texelSize.y < 0f)
            {
                center.y = 1f - center.y;
                angle = -angle;
            }
            Matrix4x4 matrixx = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one);
            material.SetMatrix("_RotationMatrix", matrixx);
            material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
            material.SetFloat("_Angle", angle * 0.01745329f);
            Graphics.Blit(source, destination, material);
        }
    }
}

