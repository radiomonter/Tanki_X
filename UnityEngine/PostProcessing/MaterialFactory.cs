namespace UnityEngine.PostProcessing
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class MaterialFactory : IDisposable
    {
        private Dictionary<string, Material> m_Materials = new Dictionary<string, Material>();

        public void Dispose()
        {
            Dictionary<string, Material>.Enumerator enumerator = this.m_Materials.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, Material> current = enumerator.Current;
                Material material = current.Value;
                GraphicsUtils.Destroy(material);
            }
            this.m_Materials.Clear();
        }

        public Material Get(string shaderName)
        {
            Material material;
            if (!this.m_Materials.TryGetValue(shaderName, out material))
            {
                Shader shader = Shader.Find(shaderName);
                if (shader == null)
                {
                    throw new ArgumentException($"Shader not found ({shaderName})");
                }
                material = new Material(shader) {
                    name = $"PostFX - {shaderName.Substring(shaderName.LastIndexOf("/") + 1)}",
                    hideFlags = HideFlags.DontSave
                };
                this.m_Materials.Add(shaderName, material);
            }
            return material;
        }
    }
}

