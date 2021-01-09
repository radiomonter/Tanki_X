namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public static class ClientGraphicsUtil
    {
        public static void ApplyShaderToRenderer(Renderer renderer, Shader shader)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material material = renderer.materials[i];
                material.shader = shader;
            }
        }

        public static void UpdateAlpha(Material material, string propertyName, float alpha)
        {
            Color color = material.GetColor(propertyName);
            Color color2 = new Color(color.r, color.g, color.b, alpha);
            material.SetColor(propertyName, color2);
        }

        public static void UpdateVector(Renderer renderer, string propertyName, Vector4 vector)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].SetVector(propertyName, vector);
            }
        }
    }
}

