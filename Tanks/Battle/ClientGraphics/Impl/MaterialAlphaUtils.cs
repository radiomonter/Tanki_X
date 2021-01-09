namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class MaterialAlphaUtils
    {
        public const float MIN_ALPHA = 0f;
        public const float MAX_ALPHA = 1f;

        public static Material[] GetAllMaterials(GameObject gameObject)
        {
            Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
            int num = 0;
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                num += componentsInChildren[i].materials.Length;
            }
            int num3 = 0;
            Material[] materialArray = new Material[num];
            int index = 0;
            while (index < componentsInChildren.Length)
            {
                int num5 = 0;
                while (true)
                {
                    if (num5 >= componentsInChildren[index].materials.Length)
                    {
                        index++;
                        break;
                    }
                    materialArray[num3++] = componentsInChildren[index].materials[num5];
                    num5++;
                }
            }
            return materialArray;
        }

        public static float GetAlpha(this Material material) => 
            material.color.a;

        public static Material GetMaterial(GameObject gameObject) => 
            gameObject.GetComponentsInChildren<Renderer>()[0].material;

        public static void SetAlpha(this Material material, float alpha)
        {
            Color color = material.color;
            color.a = Mathf.Clamp(alpha, 0f, 1f);
            material.color = color;
        }

        public static void SetAlpha(this Material[] materials, float alpha)
        {
            foreach (Material material in materials)
            {
                material.SetAlpha(alpha);
            }
        }

        public static void SetFullOpacity(this Material material)
        {
            material.SetAlpha(1f);
        }

        public static void SetFullTransparent(this Material material)
        {
            material.SetAlpha(0f);
        }

        public static void SetOverrideTag(this Material[] materials, string tag, string value)
        {
            foreach (Material material in materials)
            {
                material.SetOverrideTag(tag, value);
            }
        }
    }
}

