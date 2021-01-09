namespace UnityEngine.PostProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public sealed class RenderTextureFactory : IDisposable
    {
        private HashSet<RenderTexture> m_TemporaryRTs = new HashSet<RenderTexture>();

        public void Dispose()
        {
            this.ReleaseAll();
        }

        public RenderTexture Get(RenderTexture baseRenderTexture) => 
            this.Get(baseRenderTexture.width, baseRenderTexture.height, baseRenderTexture.depth, baseRenderTexture.format, !baseRenderTexture.sRGB ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB, baseRenderTexture.filterMode, baseRenderTexture.wrapMode, "FactoryTempTexture");

        public RenderTexture Get(int width, int height, int depthBuffer = 0, RenderTextureFormat format = 2, RenderTextureReadWrite rw = 0, FilterMode filterMode = 1, TextureWrapMode wrapMode = 1, string name = "FactoryTempTexture")
        {
            RenderTexture item = RenderTexture.GetTemporary(width, height, depthBuffer, format);
            item.filterMode = filterMode;
            item.wrapMode = wrapMode;
            item.name = name;
            this.m_TemporaryRTs.Add(item);
            return item;
        }

        public void Release(RenderTexture rt)
        {
            if (rt != null)
            {
                if (!this.m_TemporaryRTs.Contains(rt))
                {
                    throw new ArgumentException($"Attempting to remove a RenderTexture that was not allocated: {rt}");
                }
                this.m_TemporaryRTs.Remove(rt);
                RenderTexture.ReleaseTemporary(rt);
            }
        }

        public void ReleaseAll()
        {
            HashSet<RenderTexture>.Enumerator enumerator = this.m_TemporaryRTs.GetEnumerator();
            while (enumerator.MoveNext())
            {
                RenderTexture.ReleaseTemporary(enumerator.Current);
            }
            this.m_TemporaryRTs.Clear();
        }
    }
}

